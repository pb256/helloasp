public class EnemyExplosion : AnimExplosion
{
    protected override void ReturnThis()
    {
        base.ReturnThis();

        ObjectPoolManager.instance.ReturnEnemyExplosion(this);
    }
}