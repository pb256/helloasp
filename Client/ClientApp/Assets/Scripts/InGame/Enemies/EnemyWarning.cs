using System.Collections;
using UnityEngine;

using DG.Tweening;

public class EnemyWarning : MonoBehaviour
{
    public static int warningCount { get; private set; }

    public float warnTime = 2f;

    public SpriteRenderer warnCircle;
    public SpriteRenderer warnLightSpinner;

    public EnemyType enemyType;

    private Sequence disapearSequence;

    public void Init()
    {
        warningCount += 1;

        StopAllCoroutines();
        StartCoroutine(WarnProcess());

        SoundManager.instance.PlaySound(SfxType.Warning);

        warnLightSpinner.transform.localScale = Vector3.one;
        warnLightSpinner.transform.rotation = Quaternion.identity;
        warnCircle.transform.localScale = Vector3.one;
        transform.localScale = Vector3.one;

        warnLightSpinner.transform.DOKill();
        warnLightSpinner.transform.DOScaleX(3f, 0.5f).SetEase(Ease.OutQuint).From();

        var startScaleFrom = 0.2f;
        warnCircle.transform.DOKill();
        warnCircle.transform.DOScale(new Vector3(startScaleFrom, startScaleFrom, startScaleFrom), 0.4f).From();
    }

    private void Update()
    {
        warnLightSpinner.transform.Rotate(Vector3.forward, 360f * Time.deltaTime);
    }

    private IEnumerator WarnProcess()
    {
        yield return Util.GetYieldSec(warnTime);

        EnemyBase enemy = null;
        // 적 생성
        switch (enemyType)
        {
            case EnemyType.Circle:
                enemy = ObjectPoolManager.instance.GetEnemyCircle();
                break;
            case EnemyType.Penta:
                enemy = ObjectPoolManager.instance.GetEnemyPenta();
                break;
            case EnemyType.Tri:
                enemy = ObjectPoolManager.instance.GetEnemyTri();
                break;
            case EnemyType.Diamond:
                enemy = ObjectPoolManager.instance.GetEnemyDiamond();
                break;
            case EnemyType.Star:
                enemy = ObjectPoolManager.instance.GetEnemyStar();
                break;
            case EnemyType.Sun:
                enemy = ObjectPoolManager.instance.GetEnemySun();
                break;
            case EnemyType.Hexa:
                enemy = ObjectPoolManager.instance.GetEnemyHexa();
                break;
            case EnemyType.Line:
                enemy = ObjectPoolManager.instance.GetEnemyLine();
                break;
            default:
                break;
        }

        enemy.transform.position = transform.position;
        enemy.Init();

        // 사라지는 효과
        disapearSequence = DOTween.Sequence();
        disapearSequence.Append(transform.DOScale(Vector3.zero, 0.4f))
            .AppendCallback(ReturnThis);
    }

    public void ReturnThis()
    {
        disapearSequence.Kill();
        warningCount -= 1;
        ObjectPoolManager.instance.ReturnEnemyWarning(this);
    }

    public void SetEnemyType(EnemyType type)
    {
        enemyType = type;
    }
}
