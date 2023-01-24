using UnityEngine;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour
{
    public VirtualJoystick controllerL, controllerR;

    public Vector3 moveVector => controllerL.normalizedVector;

    public Vector3 attackVector => controllerR.normalizedVector;

    private void Start()
    {
        GameManager.instance.mobileController = this;
    }

    private void Update()
    {
        for (var i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (EventSystem.current.IsPointerOverGameObject(i))
                continue;

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < Screen.width * 0.5f)
                {
                    if (controllerL.fingerId == -1) 
                        controllerL.Attach(touch.fingerId);
                }
                else
                {
                    if (controllerR.fingerId == -1) 
                        controllerR.Attach(touch.fingerId);
                }
            }
        }
    }
}
