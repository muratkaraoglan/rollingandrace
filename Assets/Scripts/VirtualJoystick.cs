using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImg;
    private Image joysticImg;
    public Vector3 InputDirection { get; set; }
    private void Start()
    {
        bgImg = GetComponent<Image>();
        joysticImg = transform.GetChild(0).GetComponent<Image>();
        InputDirection = Vector3.zero;
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
        (bgImg.rectTransform,
        ped.position,
        ped.pressEventCamera,
        out pos
        ))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDirection = new Vector3(x, 0, y);
            InputDirection = (InputDirection.magnitude > 2) ? InputDirection.normalized : InputDirection;

            joysticImg.rectTransform.anchoredPosition = new Vector3(InputDirection.x * (bgImg.rectTransform.sizeDelta.x / 3)
            , InputDirection.z * (bgImg.rectTransform.sizeDelta.y / 3)
            );
        }
        //Debug.Log(InputDirection);
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        InputDirection = Vector3.zero;
        joysticImg.rectTransform.anchoredPosition = Vector3.zero;
    }
}
