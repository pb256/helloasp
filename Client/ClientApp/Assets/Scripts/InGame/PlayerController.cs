using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer spr;
    public PlayerTrail trail;

    public float speed = 6f;
    public float moveDamping = 10f;
    public Vector3 vecMove;

    public int bulletPower = 1;
    public float bulletSpeed = 20f;
    public float reloadDuration = 0.02f;
    public float bulletSpreadAmount = 0.2f;
    private bool isReloaded = true;

    private Vector3 vecMoveDirectionLerped;

    public DeathExplosion deathExplosion;

    public bool cheatMode;

    private void Start()
    {
        vecMove = Vector3.zero;
        GameManager.instance.playerTf = transform;
    }

    private void OnEnable()
    {
        StartCoroutine(CoReload());
    }

    private void Update()
    {
        // 조작
        Vector3 lookVector = GameManager.instance.inputAttack;

        vecMove = Vector3.Lerp(vecMove, GameManager.instance.inputMove.normalized * speed, moveDamping * Time.deltaTime);
        transform.Translate(vecMove * Time.deltaTime, Space.World);

        // 이동방향 회전
        vecMoveDirectionLerped = Vector3.Slerp(vecMoveDirectionLerped.normalized, vecMove.normalized, moveDamping * Time.deltaTime);
        spr.transform.right = vecMoveDirectionLerped;

        // shoot bullet
        if (GameManager.instance.isPressAttack && isReloaded)
        {
            var bullet = ObjectPoolManager.instance.GetBullet();
            bullet.transform.position = transform.position + lookVector * 0.3f;

            Vector3 bulletForceSpreaded = new Vector3
            {
                x = lookVector.x + Random.Range(-bulletSpreadAmount, bulletSpreadAmount) * 0.1f,
                y = lookVector.y + Random.Range(-bulletSpreadAmount, bulletSpreadAmount) * 0.1f,
                z = 0
            }.normalized;

            bullet.SetForce(bulletForceSpreaded * bulletSpeed);
            bullet.SetDamage(bulletPower);
            bullet.Init();

            isReloaded = false;

            SoundManager.instance.PlaySound(SfxType.Shoot);
        }

        // 스테이지 구역 제한

        var outsideDistance = transform.position.magnitude - StageManager.instance.stageSize * 0.5f;
        if (outsideDistance > 0)
            transform.position +=
                (StageManager.instance.outsideCircle.position - transform.position).normalized
                * outsideDistance;

        trail.emitting = GameManager.instance.isPressMove;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (cheatMode) // test
            return;

        if (!gameObject.activeInHierarchy)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.gameObject.activeInHierarchy)
                return;
            Death(collision.gameObject);
        }
    }

    private IEnumerator CoReload()
    {
        while (true)
        {
            isReloaded = true;
            yield return Util.GetYieldSec(reloadDuration);
        }
    }

    private void Death(GameObject killedBy)
    {
        Sprite enemySprite = killedBy.GetComponentInChildren<SpriteRenderer>().sprite;

        var go = StageManager.instance.deathSign;
        go.SetActive(true);
        var spr = go.GetComponent<SpriteRenderer>();
        spr.sprite = enemySprite;
        spr.transform.position = killedBy.transform.position;
        spr.transform.localRotation = killedBy.transform.localRotation;
        spr.transform.localScale = killedBy.transform.localScale;

        deathExplosion.gameObject.SetActive(true);
        deathExplosion.transform.position = transform.position;
        deathExplosion.Explode();

        gameObject.SetActive(false);

        StageManager.instance.RetryGame();

        SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Lowpass, 0.5f);
        SoundManager.instance.SetBgmPitchTo(0.9f, 0.8f);
    }

}
