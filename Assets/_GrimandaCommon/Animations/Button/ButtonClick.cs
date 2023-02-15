using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public event Action<string> onPointerClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = new Vector3(0.95f, 0.95f, 1);
        GetComponent<Animator>().SetTrigger("Click");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1, 1, 1);
        GetComponent<Animator>().SetTrigger("Click");
    }

}
