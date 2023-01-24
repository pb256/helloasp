using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WavePulse : WaveBase
{
    public float EndScale = 1f;
    public float WaveDuration = 0.5f;

    private Tween waveTween;
    private Sequence sq;

    protected override void OnWaveStart()
    {
        mWaveScale = new Vector3(0, 0, 0);
        spr.color = new Color(1, 1, 1, wavePower);

        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        waveTween?.Kill();
        sq?.Kill();
        
        waveTween = DOTween.To(() => mWaveScale, x => mWaveScale = x, new Vector3(EndScale, EndScale, EndScale), WaveDuration).SetEase(Ease.OutCubic);
        yield return Util.GetYieldSec(WaveDuration * 0.5f);

        sq = DOTween.Sequence();
        sq.Append(spr.DOFade(0f, WaveDuration * 0.5f).SetEase(Ease.InCubic))
            .AppendCallback(OnWaveEnd);
    }
}
