/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 15:20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIVertex : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    public delegate void OneParamDelegate(GameObject target);
    public delegate void DragEventDelegate(PointerEventData eventData);
    public DragEventDelegate OnDrag;
    public OneParamDelegate OnClicked;
    public OneParamDelegate OnReleased;

    private int vertexIndex;
    public int VertexIndex
    {
        set { this.vertexIndex = value; }
        get { return this.vertexIndex; }
    }

    void Start()
    {
        //Fetch the Event Trigger component from your GameObject
        EventTrigger trigger = GetComponent<EventTrigger>();
        //Create a new entry for the Event Trigger
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //Add a Drag type event to the Event Trigger
        entry.eventID = EventTriggerType.Drag;
        //call the OnDragDelegate function when the Event System detects dragging
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        //Add the trigger entry
        trigger.triggers.Add(entry);
    }

    public void OnDragDelegate(PointerEventData data)
    {
        RectTransform selfTransform = this.GetComponent<RectTransform>();
        OnDrag(data);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClicked(this.gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnReleased(this.gameObject);
    }
}
