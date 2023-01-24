public class EnemyBullet : BulletBase
{
    public override void ReturnThis()
    {
        ObjectPoolManager.instance.ReturnenemyBullet(this);
    }
}
