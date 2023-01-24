using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float distanceRatio = 0.01f;
    public float maxDistance = 6f;

    private readonly float screenHeightDefault = 720f;

    private void Start()
    {
        if (GameManager.instance.platform == DevicePlatform.Mobile)
        {
            transform.SetParent(null);
        }
    }

    void Update()
    {
        if (GameManager.instance.platform == DevicePlatform.Desktop)
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height) * (screenHeightDefault / Screen.height);
            Vector2 mousePos = Input.mousePosition * (screenHeightDefault / Screen.height);

            transform.localPosition = Vector3.ClampMagnitude((mousePos - (screenSize * 0.5f)) * (distanceRatio * 0.01f), maxDistance);
        }
        else if (GameManager.instance.platform == DevicePlatform.Mobile)
        {
            // 원 중심과 플레이어 사이로 조정
            var playerTf = StageManager.instance.playerTf;

            transform.position = Vector3.Lerp(Vector3.zero, playerTf.position, 0.6f);
        }
    }
}
