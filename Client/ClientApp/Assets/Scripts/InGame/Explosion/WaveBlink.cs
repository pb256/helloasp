using System.Collections;
using UnityEngine;

using DG.Tweening;

public class WaveBlink : WaveBase
{
    public float EndScale = 1f;

    public float WaveDuration = 0.5f;

    private Tween waveTween;
    private Sequence sq;

    protected override void OnWaveStart()
    {
        mWaveScale = new Vector3(EndScale, EndScale, EndScale) * 0.9f;
        spr.color = new Color(1, 1, 1, wavePower);

        StartCoroutine(Process());
    }

    private IEnumerator Process()
    {
        waveTween?.Kill();
        sq?.Kill();

        waveTween = DOTween.To(() => mWaveScale, x => mWaveScale = x, new Vector3(EndScale, EndScale, EndScale), WaveDuration).SetEase(Ease.InOutCubic);
        yield return Util.GetYieldSec(WaveDuration * 0.3f);

        sq = DOTween.Sequence();
        sq.Append(spr.DOFade(0f, WaveDuration * 0.7f).SetEase(Ease.InCubic))
            .AppendCallback(OnWaveEnd);
    }
}
