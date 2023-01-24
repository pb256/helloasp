using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlinkSprite : MonoBehaviour
{
    private bool isBlinked = false;
    private SpriteRenderer spr;

    public float blinkedAlpha = 0;
    public float blinkDelay = 0.4f;
    public bool blinkEnable = true;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        StartCoroutine(CoBlinkProcess());
    }

    private IEnumerator CoBlinkProcess()
    {
        while (true)
        {
            yield return Util.GetYieldSec(blinkDelay);

            if (blinkEnable)
            {
                if (isBlinked)
                    spr.color = Color.white;
                else
                    spr.color = new Color(1, 1, 1, blinkedAlpha);
            }

            isBlinked = !isBlinked;
        }
    }
}
