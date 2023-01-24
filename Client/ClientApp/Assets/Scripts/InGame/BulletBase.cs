using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBase : MonoBehaviour
{
    protected Vector3 moveForce;

    public void SetForce(Vector3 force)
    {
        moveForce = force;
        transform.up = moveForce.normalized;
    }

    protected virtual void Update()
    {
        transform.Translate(moveForce * Time.deltaTime, Space.World);

        if (transform.position.magnitude > StageManager.instance.stageSize * 0.5f)
        {
            OnCollisionOutside();
            ReturnThis();
        }
    }

    public virtual void ReturnThis() { }

    public virtual void OnCollisionOutside() { }
}
