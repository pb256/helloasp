using UnityEngine;

using DG.Tweening;

public class PulseEffect : MonoBehaviour
{
    public SpriteRenderer pulse;

    public Ease scaleEaseType = Ease.InOutCubic;
    public Ease fadeEaseType = Ease.InFlash;
    public float pulseScale = 2.5f;
    public float pulseDuration = 1f;

    private void OnEnable()
    {
        pulse.DOKill();
        pulse.transform.DOKill();

        pulse.transform.localScale = Vector3.zero;
        pulse.color = Color.white;

        pulse.transform.DOScale(pulseScale, pulseDuration).SetEase(scaleEaseType);
        pulse.DOFade(0f, pulseDuration).SetEase(fadeEaseType).OnComplete(() => { gameObject.SetActive(false); } );
    }
}
