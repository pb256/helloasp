using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : BulletBase
{
    protected int mDamage = 1;

    public SpriteRenderer spr;
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    public override void OnCollisionOutside()
    {
        base.OnCollisionOutside();
        StageManager.instance.BurstBulletParticle(transform.position, transform.localRotation);
    }

    public void SetDamage(int damage)
    {
        mDamage = damage;
    }

    public override void ReturnThis()
    {
        ObjectPoolManager.instance.ReturnBullet(this);
    }

    public void Init()
    {
        spr.transform.DOKill();
        spr.transform.localScale = new Vector3(1, 2, 1);
        spr.transform.DOScaleY(1, 0.25f);
        trail.Clear();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyBase>();

            if (enemy == null)
                return;

            if (!enemy.gameObject.activeInHierarchy)
                return;
            
            enemy.OnHit(mDamage);
            enemy.reactForce = transform.up;// (enemy.transform.position - transform.position).normalized;

            SoundManager.instance.PlaySound(SfxType.Hit);
            StageManager.instance.BurstBulletParticle(transform.position, transform.localRotation);

            ReturnThis();
        }
    }
}
