using UnityEngine;

public class CameraScreenStretcher : MonoBehaviour
{
    private Camera cachedCamera;

    private void Start() => cachedCamera = Camera.main;

    private void Update()
    {
        var camCtrl = cachedCamera.GetComponent<CameraController>();

        if (camCtrl != null)
        {
            transform.localScale = new Vector3
            {
                x = cachedCamera.orthographicSize * 2f * camCtrl.screenRatio,
                y = cachedCamera.orthographicSize * 2f,
                z = 1
            };
        }
    }
}
