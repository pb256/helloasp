using UnityEngine;

public class Enemy_Sun : EnemyBase
{
    public float rotateSpeed = 30f;
    public float moveSpeed = 2f;
    public SpriteRenderer spr;

    private float moveDirection;
    public float directionRotateSpeed = 2f;

    private bool mIsClockwise;

    protected override void OnCollisionOutside()
    {
        base.OnCollisionOutside();

        mIsClockwise = !mIsClockwise;
        moveDirection += 180;
    }

    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();

        moveDirection = Random.Range(0f, 360f);
        mIsClockwise = Util.RandomBool();
    }

    public override void ReturnThis()
    {
        base.ReturnThis();

        StopAllCoroutines();
        ObjectPoolManager.instance.ReturnEnemySun(this);
    }

    protected override void Update()
    {
        base.Update();
        spr.transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // direction rotate
        moveDirection += (mIsClockwise ? -1 : 1) * directionRotateSpeed * Time.deltaTime;

        Vector3 moveVec = Quaternion.Euler(0, 0, moveDirection) * Vector3.right * moveSpeed;
        transform.Translate(moveVec * Time.deltaTime, Space.World);
    }
}
