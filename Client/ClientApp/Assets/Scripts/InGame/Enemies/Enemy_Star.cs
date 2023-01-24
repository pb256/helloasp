using System.Collections;
using UnityEngine;

public class Enemy_Star : EnemyBase
{
    public float rotateSpeed = 120f;
    public float speed = 2f;
    public SpriteRenderer spr;

    private Vector3 randomDirection;

    private float mScaleDelta = 0;
    public float scaleDeltaIncreaseAmount = 60f;
    [Range(0, 1)]
    public float scaleAmount = 0.2f;

    private float mRotateTo;
    private float mCurRotation;
    public float attackDelayTime = 2f;
    public float rotateDamping = 10;

    public float bulletSpeed = 1f;
    
    protected override void OnCollisionOutside()
    {
        base.OnCollisionOutside();
        randomDirection = Vector3.Reflect(randomDirection, (Vector3.zero - transform.position).normalized);
    }

    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();
        mRotateTo = 0;
        mCurRotation = 0;

        // test
        reactForce = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector3.right * 0.5f;
        //

        StartCoroutine(CoAttackAndRotate());

        Quaternion q = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
        randomDirection = q * Vector3.right;
    }

    public override void ReturnThis()
    {
        base.ReturnThis();

        StopAllCoroutines();
        ObjectPoolManager.instance.ReturnEnemyStar(this);
    }

    protected override void Update()
    {
        base.Update();

        mCurRotation = Mathf.Lerp(mCurRotation, mRotateTo, rotateDamping * Time.deltaTime);

        spr.transform.rotation = Quaternion.identity;
        spr.transform.Rotate(0, 0, mCurRotation, Space.World);

        mScaleDelta += scaleDeltaIncreaseAmount * Time.deltaTime;
        while (mScaleDelta > 360) 
            mScaleDelta -= 360;

        spr.transform.localScale = new Vector3
        {
            x = 1 + Mathf.Sin(Mathf.Deg2Rad * mScaleDelta) * scaleAmount,
            y = 1 + Mathf.Sin(Mathf.Deg2Rad * (mScaleDelta + 90)) * scaleAmount,
            z = 1
        };

        transform.Translate(randomDirection * (speed * Time.deltaTime), Space.World);
    }

    private IEnumerator CoAttackAndRotate()
    {
        while (true)
        {
            yield return Util.GetYieldSec(attackDelayTime);
            // attack
            // 4방향 총알 발사
            for (var i = 0; i < 2; i++)
            {
                Vector3 bulletDirection = Quaternion.Euler(0, 0, mRotateTo + i * 180) * Vector3.up;
                var bullet = ObjectPoolManager.instance.GetEnemyBullet();
                bullet.transform.position = transform.position + bulletDirection * 0.4f;

                bullet.SetForce(bulletDirection * bulletSpeed);
            }

            yield return Util.GetYieldSec(0.1f);
            // rotate
            mRotateTo += 45;
        }
    }
}
