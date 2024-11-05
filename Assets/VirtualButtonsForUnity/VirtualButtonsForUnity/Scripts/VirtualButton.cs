using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pressed = false;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        pressed = false;
    }
}
