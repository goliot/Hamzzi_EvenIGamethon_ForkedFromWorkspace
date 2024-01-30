using UnityEngine;

// UI 레터박스 설정을 위한 셋팅
public class CameraResolutionForUI : MonoBehaviour
{
    private void Awake()
    {
        AdjustCanvasForLetterbox();
    }

    private void AdjustCanvasForLetterbox()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)1080 / 1920);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
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
}