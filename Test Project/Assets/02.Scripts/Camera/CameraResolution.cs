using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 레터박스 설정을 위한 셋팅
public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float) 1080 / 1920);
        float scaleWidth = 1f / scaleHeight;
        if(scaleHeight<1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        camera.rect = rect;
    }

    private void OnPreCull() => GL.Clear(true, true, Color.black);
}
