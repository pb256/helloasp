using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class DigitText : MonoBehaviour
{
    public float numberChangeDuration = 0.3f;

    private int preValue;
    private Text txt;

    private int _value;
    public int value
    {
        get => _value;
        set
        {
            preValue = _value;
            _value = value;

            DOTween.To(() => shownValue, x => shownValue = x, _value, numberChangeDuration).SetEase(Ease.Linear)
                .OnUpdate(() => { UpdateText(); })
                .OnPlay(() => { transform.DOScale(new Vector3(1.2f, 0.8f, 1f), numberChangeDuration * 0.33f).SetEase(Ease.OutCirc); })
                .OnComplete(() => { transform.DOScale(Vector3.one, numberChangeDuration * 0.66f).SetEase(Ease.OutQuint); });
        }
    }

    private int shownValue;

    private void Awake()
    {
        txt = GetComponent<Text>();
        shownValue = 0;
    }

    private void UpdateText()
    {
        txt.text = string.Format("{0:n0}", shownValue).Replace(',', ' ');
    }

    public void UpdateTextImmediate()
    {
        shownValue = value;
        UpdateText();
    }
}
