using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class StageManager : SceneSingleton<StageManager>
{
    public Transform outsideCircle;
    public Transform playerTf;

    public Camera distortionMapCamera;
    public WaveWarnMessage waveWarnMessage;
    public GameObject deathSign;
    public GameObject retryPulseEffect;

    public int defaultLife = 3;

    public DigitText scoreText;
    public DigitText txtLife;

    public Image readyMessage;

    public ParticleSystem psBulletParticle;
    public ParticleSystem psEnemyExplosionParticle;

    public GameOverScreen gameoverScreen;
    public GameObject PausePannelGO;

    [System.Serializable]
    public struct LevelData
    {
        public int entryLevel;
        public StageData[] stages;

        public StageData GetRandomStage()
        {
            return stages[Random.Range(0, stages.Length)];
        }
    }

    public int skipStageEnemyCountLessThan = 4;

    [Space(12)]
    public LevelData[] levels;

    private int currentLevel = 0;
    private StageData currentStage;
    private int countActivedWaveCoroutines = 0;

    private int _playerLife;
    public int playerLife
    {
        get => _playerLife;
        private set
        {
            _playerLife = value;
            txtLife.value = _playerLife;
        }
    }

    private int _score;
    public int score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.value = value;
        }
    }

    public float stageSize
    {
        get
        {
            if (outsideCircle == null)
            {
                return 0f;
            }
            return outsideCircle.transform.localScale.x;
        }
    }

    private void Start()
    {
        SoundManager.instance.PlaySound(BgmType.MainBGM);
        SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Normal);

        readyMessage.GetComponent<Animator>().SetTrigger("appear");
        SoundManager.instance.PlaySound(SfxType.GetReady);

        StartCoroutine(GameInit());

    }

    private IEnumerator GameInit()
    {
        playerTf.position = Vector3.zero;
        playerTf.gameObject.SetActive(true);
        
        playerLife = defaultLife;
        score = 0;

        currentLevel = 0;

        Camera.main.GetComponent<CameraController>().CameraSizeTo(0f, 0.4f);
        SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Normal);

        yield return Util.GetYieldSec(1.5f);

        StartCoroutine(NextStage());
    }

    private void CreateEnemyAt(EnemyType enemy, Vector3 pos)
    {
        pos.z = 0;

        var warn = ObjectPoolManager.instance.GetEnemyWarning();
        warn.SetEnemyType(enemy);
        warn.transform.position = pos;
        warn.Init();

        var outsideDistance = warn.transform.position.magnitude - stageSize * 0.5f;
        if (outsideDistance > 0)
        {
            warn.transform.position += (outsideCircle.position - warn.transform.position).normalized * outsideDistance;
        }
    }

    private StageData GetRandomStage()
    {
        var levelDataIndex = -1;
        var highestStageEntryLevel = -1;

        for (var i = 0; i < levels.Length; i++)
        {
            var isCurrentLevel = levels[i].entryLevel <= currentLevel;
            var isHighestLevel = levels[i].entryLevel > highestStageEntryLevel;

            if (isCurrentLevel && isHighestLevel)
            {
                levelDataIndex = i;
                highestStageEntryLevel = levels[i].entryLevel;
            }
        }

        if (levelDataIndex < 0)
        {
            return null;
        }

        return levels[levelDataIndex].GetRandomStage();
    }

    private IEnumerator NextStage()
    {
        waveWarnMessage.Appear();

        var warnDuration = 1.4f;
        yield return Util.GetYieldSec(warnDuration);

        currentLevel += 1;
        currentStage = GetRandomStage();

        if (currentStage == null)
        {
            Debug.LogError("스테이지 데이터가 없습니다!");
            yield break;
        }

        countActivedWaveCoroutines = 0;
        foreach (var waveData in currentStage.waves)
        {
            StartCoroutine(WaveProcess(waveData));
            countActivedWaveCoroutines += 1;
        }

        StartCoroutine(CoCheckFinishedStage());
    }

    private IEnumerator WaveProcess(StageData.EnemyWaveData waveData)
    {
        yield return Util.GetYieldSec(waveData.delay);

        var randomValue = Random.Range(0f, 1f);

        for (var i = 0; i < waveData.count; i++)
        {
            var process = (float)i / (waveData.count - 1);

            Vector3 pos;

            switch (waveData.posType)
            {
                case StageData.CreatePositionType.Random:
                    pos = Util.GetVector(Random.Range(0f, 360f)) * Random.Range(0f, stageSize * 0.5f);
                    CreateEnemyAt(waveData.enemy, pos);
                    break;
                case StageData.CreatePositionType.RandomFromCenter:
                    pos = Util.GetVector(Random.Range(0f, 360f)) * waveData.range;
                    CreateEnemyAt(waveData.enemy, pos);
                    break;
                case StageData.CreatePositionType.RandomFromPlayer:
                    pos = Util.GetVector(Random.Range(0f, 360f)) * waveData.range;
                    CreateEnemyAt(waveData.enemy, playerTf.position + pos);
                    break;
                case StageData.CreatePositionType.Spiral:
                    {
                        pos = Util.GetVector(360f * process + randomValue * 360f) * Mathf.Lerp(0.2f, stageSize * 0.5f, process);
                        CreateEnemyAt(waveData.enemy, pos);
                        pos = Util.GetVector(360f * process + randomValue * 360f) * -Mathf.Lerp(0.2f, stageSize * 0.5f, process);
                        CreateEnemyAt(waveData.enemy, pos);
                    }
                    break;
                case StageData.CreatePositionType.SpiralInverse:
                    {
                        pos = Util.GetVector(360f * process + randomValue * 360f) * Mathf.Lerp(0.2f, stageSize * 0.5f, 1f - process);
                        CreateEnemyAt(waveData.enemy, pos);
                        pos = Util.GetVector(360f * process + randomValue * 360f) * -Mathf.Lerp(0.2f, stageSize * 0.5f, 1f - process);
                        CreateEnemyAt(waveData.enemy, pos);
                    }
                    break;
                case StageData.CreatePositionType.CircleFromCenter:
                    for (var j = 0; j < waveData.count; j++)
                    {
                        var circleProcess = (float)j / waveData.count;
                        pos = Util.GetVector(360f * circleProcess);
                        CreateEnemyAt(waveData.enemy, pos * waveData.range);
                    }
                    break;
                case StageData.CreatePositionType.CircleFromPlayer:
                    for (var j = 0; j < waveData.count; j++)
                    {
                        var circleProcess = (float)j / waveData.count;
                        pos = Util.GetVector(360f * circleProcess);
                        CreateEnemyAt(waveData.enemy, playerTf.position + pos * waveData.range);
                    }
                    break;
                case StageData.CreatePositionType.NextPlayerPosition:
                    {
                        pos = playerTf.GetComponent<PlayerController>().vecMove * waveData.range;
                        CreateEnemyAt(waveData.enemy, playerTf.position + pos);
                    }
                    break;
                default:
                    break;
            }

            // 한번에 생성
            if (waveData.posType == StageData.CreatePositionType.CircleFromCenter
                || waveData.posType == StageData.CreatePositionType.CircleFromPlayer)
            {
                break;
            }
            else
            {
                yield return Util.GetYieldSec(waveData.duration / waveData.count);
            }

        }

        countActivedWaveCoroutines -= 1;

    }

    private IEnumerator CoCheckFinishedStage()
    {
        while (true)
        {
            // 일정 수치를 두고 그보다 적게 남았으면 스테이지 넘기기
            var isCurrentEnemiesAlmostDead = (EnemyBase.enemyCount < skipStageEnemyCountLessThan);
            if (countActivedWaveCoroutines == 0 && EnemyWarning.warningCount == 0 && isCurrentEnemiesAlmostDead)
            {
                StartCoroutine(NextStage());
                yield break;
            }
            yield return Util.GetYieldSec(0.2f);
        }
    }

    public void RetryGame()
    {
        StopAllCoroutines();

        var warns = FindObjectsOfType<EnemyWarning>();
        var enemies = FindObjectsOfType<EnemyBase>();
        var enemyBullets = FindObjectsOfType<EnemyBullet>();

        foreach (var warn in warns) 
            warn.ReturnThis();

        foreach (var enemy in enemies) 
            enemy.ReturnThis();

        foreach (var enemyBullet in enemyBullets) 
            enemyBullet.ReturnThis();

        StartCoroutine(RetryProcess());
    }

    private IEnumerator RetryProcess()
    {
        yield return Util.GetYieldSec(1.5f);
        deathSign.SetActive(false);

        if (playerLife > 0)
        {
            playerLife -= 1;

            SoundManager.instance.SetBgmPitchTo(1);
            SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Normal);

            playerTf.position = Vector3.zero;
            playerTf.gameObject.SetActive(true);
            playerTf.GetComponent<PlayerController>().trail.Clear();

            retryPulseEffect.SetActive(true);

            SoundManager.instance.PlaySound(SfxType.Retry);

            yield return Util.GetYieldSec(1.5f);
            StartCoroutine(NextStage());
        }
        else
        {
            // gameover
            Camera.main.GetComponent<CameraController>().CameraSizeTo(1.5f, 0.6f);

            GameManager.instance.SaveScore(score);

            gameoverScreen.gameObject.SetActive(true);
            gameoverScreen.Show();
            SoundManager.instance.SetBgmPitchTo(1);
            SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Highpass);
        }
    }

    public void BurstBulletParticle(Vector3 pos, Quaternion q)
    {
        psBulletParticle.transform.position = pos;
        psBulletParticle.transform.localRotation = q;

        var psList = psBulletParticle.GetComponentsInChildren<ParticleSystem>();

        foreach (var ps in psList) 
            ps.Play();
    }

    public void BurstEnemyExplosionParticle(Vector3 pos)
    {
        psEnemyExplosionParticle.transform.position = pos;

        psEnemyExplosionParticle.Play();
    }

    public void OnClickRestartButton()
    {
        SoundManager.instance.PlaySound(SfxType.Start);

        gameoverScreen.Hide();

        readyMessage.GetComponent<Animator>().SetTrigger("appear");
        SoundManager.instance.PlaySound(SfxType.GetReady);

        StartCoroutine(GameInit());
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;

        PausePannelGO.SetActive(true);
        SoundManager.instance.PauseAll();
    }

    public void OnClickResume()
    {
        Time.timeScale = 1;

        PausePannelGO.SetActive(false);
        SoundManager.instance.ResumeAll();
    }
}
