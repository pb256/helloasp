using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class OutsideCircleEffect : MonoBehaviour
{
    public float delay = 0f;
    public float defaultAlpha = 0.2f;
    public float alphaFadeDuration = 1f;
    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spr.color = new Color(1, 1, 1, defaultAlpha);
        spr.DOFade(defaultAlpha * 2f, alphaFadeDuration).SetLoops(-1).SetDelay(delay);
    }

    private void Update()
    {
        transform.localPosition = -(Camera.main.transform.position * 0.08f) * (transform.localScale.x - 1f);
    }
}
