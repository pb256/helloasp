using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Text))]
public sealed class FpsText : MonoBehaviour
{
    private Text txt;
    private int frame = 0;

    private void Awake()
    {
        txt = GetComponent<Text>();
        StartCoroutine(CoUpdateText());
    }

    private void Update()
    {
        frame += 1;
    }

    private IEnumerator CoUpdateText()
    {
        while (true)
        {
            yield return Util.GetYieldSec(1f);

            txt.text = $"fps : { frame }";

            frame = 0;
        }
    }
}
