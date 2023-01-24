using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class GaugeBar : MonoBehaviour
{
    public SpriteRenderer gaugeBorder;
    public SpriteRenderer gaugeBar;

    public Color gaugeBorderDefaultColor;
    public Color gaugeBarDefaultColor;

    public bool isAlwaysShow = false;

    public int maxValue = 10;

    [SerializeField]
    private int _value = 0;
    public int value
    {
        get => _value;
        set
        {
            _value = value;
            OnChangeValue();
            prevValue = _value;
        }
    }

    public float appearDuration = 1f;

    private int prevValue;
    private float easedValue;

    protected void Awake()
    {
        prevValue = value;

        if (!isAlwaysShow)
        {
            DisappearImmediate();
        }
    }

    protected void OnChangeValue()
    {
        easedValue = prevValue;
        DOTween.To(() => easedValue, x => easedValue = x, _value, 0.4f);

        Appear();
    }

    private void Appear()
    {
        if (isAlwaysShow)
        {
            return;
        }

        gaugeBorder.color = gaugeBorderDefaultColor;
        gaugeBar.color = gaugeBarDefaultColor;

        var disappearDuration = 0.3f;

        Sequence sq = DOTween.Sequence();

        gaugeBorder.DOKill();
        gaugeBar.DOKill();

        sq.AppendInterval(appearDuration)
            .AppendCallback(() => {
                gaugeBorder.DOFade(0, disappearDuration);
                gaugeBar.DOFade(0, disappearDuration);
            });
    }

    public void DisappearImmediate()
    {
        if (isAlwaysShow)
        {
            return;
        }

        gaugeBorder.color = Color.clear;
        gaugeBar.color = Color.clear;
    }

    private void Update()
    {
        gaugeBar.size = new Vector2(Mathf.Clamp01(easedValue / maxValue), 1);
    }
}
