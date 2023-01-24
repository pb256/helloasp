using System.Collections;
using UnityEngine;

public class Enemy_Line : EnemyBase
{
    public SpriteRenderer spr;
    public SpriteRenderer blurrySprite;

    private float mRotation;
    private float mRotateSpeed;
    private bool mIsRotate;

    public float maxRotateSpeed = 1200;
    public float rotateSpeedIncrease = 10;

    public float attackDelay = 0.1f;
    public float bulletSpeed = 3f;
    
    protected override void OnCreateEnemy()
    {
        base.OnCreateEnemy();

        blurrySprite.color = Color.clear;
        spr.color = Color.white;

        reactForce = (Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector3.right) * 0.5f;
        mIsRotate = false;
        mRotateSpeed = 0;

        mRotation = Random.Range(0, 360);

        StartCoroutine(CoAttackProcess());
    }

    public override void ReturnThis()
    {
        base.ReturnThis();

        StopAllCoroutines();
        ObjectPoolManager.instance.ReturnEnemyLine(this);
    }

    protected override void Update()
    {
        base.Update();

        if (mIsRotate)
        {
            mRotateSpeed += rotateSpeedIncrease * Time.deltaTime;
            mRotateSpeed = Mathf.Min(mRotateSpeed, maxRotateSpeed);
            mRotation += mRotateSpeed * Time.deltaTime;

            if (mRotation > 360000)
            {
                mRotation -= 360000;
            }

            var blurAmount = Mathf.Pow(Mathf.Clamp01(Mathf.InverseLerp(0, maxRotateSpeed * 0.8f, mRotateSpeed)), 3f);

            blurrySprite.color = new Color(1, 1, 1, blurAmount);
            spr.color = new Color(1, 1, 1, 1f - blurAmount);
        }

        spr.transform.rotation = Quaternion.identity;
        spr.transform.Rotate(Vector3.forward, mRotation);
    }

    private IEnumerator CoAttackProcess()
    {
        yield return Util.GetYieldSec(1);

        mIsRotate = true;

        yield return Util.GetYieldSec(2);

        while (true)
        {
            Vector3 bulletDirection = Quaternion.Euler(0, 0, Random.Range(0f, 360f)) * Vector3.up;
            var bullet = ObjectPoolManager.instance.GetEnemyBullet();
            bullet.transform.position = transform.position + bulletDirection * 0.4f;

            bullet.SetForce(bulletDirection * bulletSpeed);

            yield return Util.GetYieldSec(attackDelay);
        }
    }
}
