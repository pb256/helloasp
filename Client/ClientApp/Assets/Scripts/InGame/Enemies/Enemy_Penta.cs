using UnityEngine;

public class Enemy_Penta : EnemyBase
{
    public float rotateSpeed = 120f;
    public float speed = 2f;
    public SpriteRenderer spr;

    public override void ReturnThis()
    {
        base.ReturnThis();

        StopAllCoroutines();
        ObjectPoolManager.instance.ReturnEnemyPenta(this);
    }

    protected override void Update()
    {
        base.Update();

        spr.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        Vector3 moveDir = (StageManager.instance.playerTf.position - transform.position).normalized;

        transform.Translate(moveDir * (speed * Time.deltaTime));
    }
}
