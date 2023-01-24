using UnityEngine;

public class Enemy_Tri : EnemyBase
{
    public float rotateSpeed = 120f;

    private Vector3 gravity = Vector3.zero;
    public float gravityIncreaseSpeed = 1f;

    public SpriteRenderer spr;

    protected override void OnCollisionOutside()
    {
        base.OnCollisionOutside();
        gravity = Vector3.zero;
    }

    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();
        gravity = Vector3.zero;
    }

    public override void ReturnThis()
    {
        base.ReturnThis();
        ObjectPoolManager.instance.ReturnEnemyTri(this);
    }

    protected override void Update()
    {
        Vector3 gravityDir = (StageManager.instance.playerTf.position - transform.position).normalized;
        gravity += gravityDir * gravityIncreaseSpeed;
        transform.Translate(gravity * Time.deltaTime);
        spr.transform.Rotate(Vector3.forward, gravity.magnitude * rotateSpeed * Time.deltaTime);
    }
}
