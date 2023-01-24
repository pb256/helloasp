using UnityEngine;

public class Enemy_Circle : EnemyBase
{
    public float speed = 1f;
    private Vector3 randomDirection;

    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();

        Quaternion q = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        randomDirection = q * Vector3.right;
    }

    public override void ReturnThis()
    {
        base.ReturnThis();

        ObjectPoolManager.instance.ReturnEnemyCircle(this);
    }

    protected override void Update()
    {
        base.Update();

        transform.Translate(randomDirection * (speed * Time.deltaTime), Space.World);
    }

    protected override void OnCollisionOutside()
    {
        base.OnCollisionOutside();

        randomDirection = (Vector3.zero - transform.position).normalized;
    }
}
