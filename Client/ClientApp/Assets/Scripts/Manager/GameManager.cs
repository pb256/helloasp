using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public sealed class GameManager : Singleton<GameManager>
{
    static readonly string HIGHSCORE_KEY = "highscore";
    static readonly string ENCRYPT_KEY = "aSda^sDf22e";

    public Transform playerTf;
    public MobileController mobileController;

    public string sceneNameMoveTo { get; private set; } = "";

    // 컨트롤 입력 값
    public Vector3 inputMove { get; private set; }

    public Vector3 inputAttack { get; private set; }


    public bool isPressMove => inputMove.magnitude > float.Epsilon;

    public bool isPressAttack => inputAttack.magnitude > float.Epsilon;

    public DevicePlatform platform { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        platform = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer
            ? DevicePlatform.Mobile : DevicePlatform.Desktop;
    }

    private void Start()
    {
        DOTween.Init().SetCapacity(1250, 312);
        Random.InitState(System.Environment.TickCount);
    }

    private void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (playerTf == null)
        {
            return;
        }

        if (platform == DevicePlatform.Desktop)
        {
            inputMove = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Vector3 mousePos = Input.mousePosition;
            Vector3 playerPosition = Camera.main.WorldToScreenPoint(playerTf.position);
            playerPosition.z = 0f;

            inputAttack = Vector3.zero;

            if (Input.GetMouseButton(0))
            {
                Vector3 playerToMouse = (mousePos - playerPosition).normalized;

                inputAttack = playerToMouse;
            }

        }
        else
        {
            if (mobileController != null)
            {
                inputMove = mobileController.moveVector;
                inputAttack = mobileController.attackVector;
            }

        }
    }

    public void SaveScore(int score)
    {
        var highscore = GetHighscore();
        if (score > highscore)
        {
            PlayerPrefs.SetString(HIGHSCORE_KEY, Encryption.Encrypt(score.ToString(), ENCRYPT_KEY));
            PlayerPrefs.Save();
        }
    }

    public int GetHighscore()
    {
        int highscore;
        try
        {
            highscore = int.Parse(Encryption.Decrypt(PlayerPrefs.GetString(HIGHSCORE_KEY), ENCRYPT_KEY));
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
            highscore = 0;
        }
        return highscore;
    }

    public void SceneGoto(string sceneName)
    {
        sceneNameMoveTo = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
