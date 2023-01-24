using System.Collections;
using UnityEngine;

using DG.Tweening;

public class Enemy_Hexa : EnemyBase
{
    public float rotateSpeed = 180f;
    public SpriteRenderer spr;

    public float targetPosUpdateTime = 1f;
    public float moveOnceDistance = 2f;

    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();

        StartCoroutine(MoveProcess());
    }

    public override void ReturnThis()
    {
        base.ReturnThis();

        StopAllCoroutines();
        transform.DOKill();
        ObjectPoolManager.instance.ReturnEnemyHexa(this);
    }

    protected override void Update()
    {
        base.Update();
        spr.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    private IEnumerator MoveProcess()
    {
        while (true)
        {
            yield return Util.GetYieldSec(targetPosUpdateTime);

            Vector3 randomVector = Quaternion.Euler(0, 0, ((float)Random.Range(0, 8) / 8) * 360) * Vector3.right;

            Vector3 mTargetPos = randomVector * moveOnceDistance;

            transform.DOKill();
            transform.DOMove(transform.position + mTargetPos, targetPosUpdateTime * 0.7f).SetEase(Ease.InOutQuad);
        }
    }
}
