using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class UITextInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent { }

    [SerializeField]
    private OnClickEvent onClickEvent;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }
}
