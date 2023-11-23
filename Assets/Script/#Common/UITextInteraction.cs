using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class UITextInteraction : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent
    {

    }

    //text ui에 호출하고 싶은 메소드
    [SerializeField]
    private OnClickEvent onclickEvent;

    //색상이 바뀌고 터치가 되는 텍스트
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
        onclickEvent?.Invoke();
    }
}
