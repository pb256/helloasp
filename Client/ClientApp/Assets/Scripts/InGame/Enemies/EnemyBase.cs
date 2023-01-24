using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    public static int enemyCount { get; private set; }

    public int defaultHp = 5;
    public int killScore = 100;
    protected int mHp;

    public GaugeBar hpBar;

    /// <summary>
    /// 총 맞을 때 밀려나는 힘
    /// </summary>
    public Vector3 reactForce { get; set; }
    public float reactFriction = 0.3f;

    public void Init()
    {
        enemyCount += 1;

        mHp = defaultHp;
        hpBar.maxValue = defaultHp;
        hpBar.value = defaultHp;
        hpBar.DisappearImmediate();

        transform.localScale = new Vector3(2, 2, 2);
        transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutQuad);

        OnCreateEnemy();
    }

    private void OnDie()
    {
        var explosion = ObjectPoolManager.instance.GetEnemyExplosion();
        explosion.transform.position = transform.position;
        explosion.Explode();

        StageManager.instance.BurstEnemyExplosionParticle(transform.position);

        StageManager.instance.score += killScore;

        ReturnThis();
    }

    public virtual void ReturnThis()
    {
        enemyCount -= 1;
    }

    protected virtual void Update()
    {
        // react
        if (reactForce.sqrMagnitude > float.Epsilon)
            reactForce = Vector3.Lerp(reactForce, Vector3.zero, reactFriction * Time.deltaTime * (1000f / 60f));
        transform.Translate(reactForce * (5f * Time.deltaTime), Space.World);
    }

    private void LateUpdate()
    {
        // block move out
        var outsideDistance = transform.position.magnitude - StageManager.instance.stageSize * 0.5f;
        if (outsideDistance > 0)
        {
            transform.position += (StageManager.instance.outsideCircle.position - transform.position).normalized * outsideDistance;
            OnCollisionOutside();
        }
    }

    public void OnHit(int damage)
    {
        mHp -= damage;
        hpBar.value = mHp;
        if (mHp <= 0) 
            OnDie();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 적들끼리 겹치지 않도록
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 vecDiff = (collision.transform.position - transform.position).normalized;
            transform.position -= vecDiff * 0.3f * Time.deltaTime;
        }
    }

    protected virtual void OnCreateEnemy()
    {
        reactForce = Vector3.zero;
    }

    protected virtual void OnCollisionOutside() { }
}
