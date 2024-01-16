using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    private float time;
    private float destroyTime = 0.3f;

    private void Update()
    {
        time += Time.deltaTime;

        // Calculate the interpolation factor based on time
        float t = Mathf.Clamp01(time / destroyTime);

        // Calculate the target position
        Vector3 targetPosition = transform.position + Vector3.up * 0.01f;

        // Lerp the position for smooth movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, t);

        // Calculate the alpha value based on time
        float alpha = Mathf.Lerp(1.0f, 0.0f, t);

        // Set the alpha value for the text color
        Text textComponent = GetComponent<Text>();
        if (textComponent != null)
        {
            Color textColor = textComponent.color;
            textColor.a = alpha;
            textComponent.color = textColor;
        }

        if (time > destroyTime)
        {
            // 텍스트가 완전히 투명해지면 파괴
            Destroy(gameObject);
        }
    }
}
