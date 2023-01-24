using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BlinkImage : MonoBehaviour
{
    private bool isBlinked = false;
    private Image img;

    public float blinkedAlpha = 0;
    public float blinkDelay = 0.4f;
    public bool blinkEnable = true;

    private void Awake()
    {
        img = GetComponent<Image>();
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
                    img.color = Color.white;
                else
                    img.color = new Color(1, 1, 1, blinkedAlpha);
            }

            isBlinked = !isBlinked;
        }
    }
}
