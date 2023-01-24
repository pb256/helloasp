using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class WaveWarnMessage : MonoBehaviour
{
    public Image effectImage;
    public float duration = 1f;

    private Image image;
    private bool isBlinked;
    public float blinkAlpha = 0.8f;

    private Vector3 defaultLocalScale;

    public float appearDuration = 0.7f;

    private void Awake()
    {
        defaultLocalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        image = GetComponent<Image>();
        isBlinked = false;
    }

    void Start()
    {
        effectImage.color = new Color(1, 1, 1, 0.5f);
        effectImage.DOFade(0, duration).SetEase(Ease.InCubic).SetLoops(-1);
        effectImage.transform.DOScaleX(1.07f, duration).SetEase(Ease.InOutCubic).SetLoops(-1);

        StartCoroutine(BlinkProcess());
    }

    private IEnumerator BlinkProcess()
    {
        while (true)
        {
            if (isBlinked)
                image.color = new Color(1, 1, 1, 1);
            else
                image.color = new Color(1, 1, 1, blinkAlpha);

            isBlinked = !isBlinked;
            yield return Util.GetYieldSec(1f / 60f);
        }   
    }

    public void Appear()
    {
        transform.DOKill();
        transform.localScale = defaultLocalScale;

        Sequence sq = DOTween.Sequence();
        sq.Append(transform.DOScaleY(0f, appearDuration).SetEase(Ease.OutElastic).From())
            .AppendInterval(1.5f)
            .Append(transform.DOScaleY(0f, appearDuration).SetEase(Ease.InElastic));

        SoundManager.instance.PlaySound(SfxType.WaveSiren);
    }
}
