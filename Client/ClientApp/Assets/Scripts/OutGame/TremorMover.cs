using UnityEngine;

public class TremorMover : MonoBehaviour
{
    public float radius = 0.2f;
    public float rotateSpeed = 60f;
    public float axisOffset = 60f;

    private Vector3 startPosition;
    private float rotate;

    private void Start()
    {
        startPosition = transform.position;
    }
    
    private void Update()
    {
        rotate += rotateSpeed * Time.deltaTime;
        if (rotate > 360)
        {
            rotate -= 360;
        }

        transform.position = startPosition + new Vector3(Mathf.Sin(Mathf.Deg2Rad * rotate) * radius, Mathf.Cos(Mathf.Deg2Rad * (rotate + axisOffset)) * radius, 0);
    }
}
