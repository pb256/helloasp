public class DeathExplosion : AnimExplosion
{
    protected override void ReturnThis()
    {
        gameObject.SetActive(false);
    }
}
