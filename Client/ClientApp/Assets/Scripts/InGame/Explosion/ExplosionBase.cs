using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBase : MonoBehaviour
{
    public WaveBase wave;
    protected bool IsWaveAnimationEnd => wave.IsWaveAnimationEnd;

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        Vector3 explosionScreenPosition = Camera.main.WorldToViewportPoint(transform.position);
        Vector3 wavePositionOnDistortCamera = StageManager.instance.distortionMapCamera.ViewportToWorldPoint(explosionScreenPosition);

        wave.transform.position = wavePositionOnDistortCamera;
    }
}
