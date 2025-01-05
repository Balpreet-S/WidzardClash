using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public void OnBeginDrag(PointerEventData press){
        Debug.Log("pressing");
    }
    public void OnDrag(PointerEventData drag){
        Debug.Log("Dragging");
    }

    public void OnEndDrag(PointerEventData drop){
        Debug.Log("Dropping");
    }
    public void OnPointerDown(PointerEventData mouse){
        Debug.Log("OnPointerDown");
    }
}
