using System.Collections;
using UnityEngine;

using DG.Tweening;

public class Enemy_Diamond : EnemyBase
{
    public float rotateSpeed = -80f;
    public SpriteRenderer spr;

    private Vector3 mTargetPos;
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
        ObjectPoolManager.instance.ReturnEnemyDiamond(this);
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

            mTargetPos = (StageManager.instance.playerTf.position - transform.position).normalized * moveOnceDistance;

            transform.DOKill();
            transform.DOMove(transform.position + mTargetPos, targetPosUpdateTime * 0.7f).SetEase(Ease.InBack);
        }
    }
}
