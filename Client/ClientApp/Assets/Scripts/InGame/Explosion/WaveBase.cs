using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaveBase : MonoBehaviour
{
    public float waveSize = 1f;
    public float wavePower = 1f;

    protected Vector3 mWaveScale;
    protected SpriteRenderer spr;

    private CameraController mCamRef;
    
    public bool IsWaveAnimationEnd
    {
        get; private set;
    }

    private void Awake()
    {
        mCamRef = Camera.main.GetComponent<CameraController>();
        spr = GetComponent<SpriteRenderer>();
        IsWaveAnimationEnd = false;
    }

    public void Init()
    {
        transform.localPosition = Vector3.zero;

        IsWaveAnimationEnd = false;
        OnWaveStart();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        // 해상도에 따른 이미지 비율 조정

        var invRatio = 1f / mCamRef.screenRatio;

        transform.localScale = new Vector3(mWaveScale.x * invRatio, mWaveScale.y, mWaveScale.z) * waveSize;
    }

    protected virtual void OnWaveStart() { }

    protected void OnWaveEnd() => IsWaveAnimationEnd = true;
}
