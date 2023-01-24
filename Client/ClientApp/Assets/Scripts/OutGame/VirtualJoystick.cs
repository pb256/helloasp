using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class VirtualJoystick : MonoBehaviour
{
    public float touchSensitiveCutoff = 8f;
    public float knobMaxDistance = 120f;
    public float tweenDuration = 0.3f;

    public Image knobImage;
    private Image joystickBaseImage;

    private Color defaultJoystickColor;
    private Color defaultKnobColor;

    public int fingerId { get; private set; } = -1;

    public Vector3 forceVector { get; private set; }

    public Vector3 normalizedVector
    {
        get
        {
            if (forceVector.magnitude < touchSensitiveCutoff)
                return Vector3.zero;
            return forceVector.normalized;
        }
    }

    private void Awake()
    {
        joystickBaseImage = GetComponent<Image>();
    }

    private void Start()
    {
        defaultKnobColor = knobImage.color;
        defaultJoystickColor = joystickBaseImage.color;

        knobImage.color = new Color(knobImage.color.r, knobImage.color.g, knobImage.color.b, 0);
        joystickBaseImage.color = new Color(joystickBaseImage.color.r, joystickBaseImage.color.g, joystickBaseImage.color.b, 0);
    }

    private void Update()
    {
        if (fingerId != -1)
        {
            Touch touch = new Touch();
            foreach (var t in Input.touches)
            {
                if (t.fingerId == fingerId) 
                    touch = t;
            }
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // detach
                fingerId = -1;

                forceVector = Vector3.zero;

                knobImage.DOKill();
                joystickBaseImage.DOKill();
                transform.DOKill();

                knobImage.DOFade(0, tweenDuration);
                joystickBaseImage.DOFade(0, tweenDuration);
                transform.DOScale(0.6f, tweenDuration);
            }
            else
            {
                knobImage.transform.position = touch.position;
                knobImage.transform.localPosition = 
                    Mathf.Min(knobImage.transform.localPosition.magnitude, knobMaxDistance) * knobImage.transform.localPosition.normalized;
                forceVector = knobImage.transform.position - transform.position;
            }
        }
    }

    public void Attach(int fingerId)
    {
        this.fingerId = fingerId;
        Touch touch = new Touch();
        foreach (var t in Input.touches)
        {
            if (t.fingerId == fingerId) 
                touch = t;
        }
        knobImage.DOKill();
        joystickBaseImage.DOKill();
        transform.DOKill();

        knobImage.DOFade(defaultKnobColor.a, tweenDuration);
        joystickBaseImage.DOFade(defaultJoystickColor.a, tweenDuration);
        transform.DOScale(1f, tweenDuration);

        transform.position = touch.position;
    }
}
