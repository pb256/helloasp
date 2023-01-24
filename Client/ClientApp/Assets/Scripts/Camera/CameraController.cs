using System;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    private float defaultCamSize;

    private float cameraShakeForce;
    
    public float screenRatio { get; private set; }

    private float _camSize;
    public float camSize
    {
        get => _camSize;
        set
        {
            if (Math.Abs(_camSize - value) > float.Epsilon)
            {
                _camSize = value;
                Camera.main.orthographicSize = defaultCamSize + _camSize;
            }
        }
    }

    [SerializeField]
    private float shakeDamping = 20f;

    public Transform camTarget = null;
    public RawImage postProcessEffect;

    private Vector3 targetPosition;
    private Vector3 targetPositionTraced;

    [Range(0.0001f, 1f)]
    public float targetTraceSpeed = 0.5f;

    private Tween zoomTween = null;

    private void Awake()
    {
        defaultCamSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        var curScreenRatio = (float)Screen.width / Screen.height;
        if (Math.Abs(curScreenRatio - screenRatio) > float.Epsilon)
        {
            screenRatio = curScreenRatio;
        }
    }

    void LateUpdate()
    {
        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }

        if (camTarget != null)
        {
            targetPosition = new Vector3
            {
                x = camTarget.position.x,
                y = camTarget.position.y,
                z = transform.position.z
            };
        }
        
        targetPositionTraced = Vector3.Lerp(targetPositionTraced, targetPosition, (1000f / 60f * targetTraceSpeed) * Time.deltaTime);

        if (cameraShakeForce > float.Epsilon)
        {
            var randomDirection = Random.Range(0f, Mathf.PI * 2f);
            Vector3 shakeRandomPosition = new Vector3()
            {
                x = Random.Range(0f, cameraShakeForce) * Mathf.Sin(randomDirection),
                y = Random.Range(0f, cameraShakeForce) * Mathf.Cos(randomDirection),
                z = 0
            };

            transform.position = targetPositionTraced - shakeRandomPosition;
            postProcessEffect.material.SetFloat("_ChannelDivision", cameraShakeForce * 20f);

            cameraShakeForce = Mathf.Lerp(cameraShakeForce, 0f, shakeDamping * Time.deltaTime);
        }
        else
        {
            transform.position = targetPositionTraced;
            postProcessEffect.material.SetFloat("_ChannelDivision", 0f);
        }
    }

    public void Shake(float amount)
    {
        // 큰 진동이 작은 진동에 무시되지 않게
        if (cameraShakeForce > amount)
            return;
        cameraShakeForce = amount * 0.1f;
    }

    public void CameraSizeTo(float to, float duration = 1f)
    {
        zoomTween?.Kill();

        zoomTween = DOTween.To(() => camSize, x => camSize = x, to, duration).SetEase(Ease.OutCubic);
    }
}
