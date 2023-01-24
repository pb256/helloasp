using UnityEngine;
using DG.Tweening;

public class OutsideCircle : MonoBehaviour
{
    public Transform hexaPatternMaskTf;
    public Transform lightSpinnerTf;
    public float lightSpinnerRotateSpeed = 60f;
    public float maskMovementDuration = 2f;

    public GameObject circleEffect;
    public int effectCount = 5;
    public float effectScaleIncrease = 0.1f;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        var effectScale = 1f + effectScaleIncrease;
        for (var i = 0; i < effectCount; i++)
        {
            var go = Instantiate(circleEffect);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one * effectScale;
            effectScale += effectScaleIncrease;
            go.GetComponent<OutsideCircleEffect>().delay = 0.2f * i;
        }
    }

    private void Start()
    {
        anim.SetTrigger("appear");

        var maskStartPositionY = hexaPatternMaskTf.localPosition.y;
        
        hexaPatternMaskTf.DOLocalMoveY(-maskStartPositionY, maskMovementDuration).SetLoops(-1).SetEase(Ease.InOutCubic);

        var lightSpinnerSprites = lightSpinnerTf.GetComponentsInChildren<SpriteRenderer>();
        foreach (var spr in lightSpinnerSprites)
        {
            spr.DOFade(0.5f, 0.6f).SetLoops(-1);
        }
    }

    private void Update()
    {
        lightSpinnerTf.Rotate(Vector3.forward, lightSpinnerRotateSpeed * Time.deltaTime);
    }

    public void OnEndAppearAnimation()
    {
        anim.enabled = false;
    }
}
