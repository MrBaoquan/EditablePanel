/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 15:12
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventController : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
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


    private void OnDragDelegate(PointerEventData data)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("EPCamera").SendMessage("OnClickedEmpty");
        UIVertexSpawner[] uiSpawner = GameObject.FindObjectsOfType<UIVertexSpawner>();
        foreach(var spawner in uiSpawner)
        {
            spawner.EmptyDirtyVertices();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float minPosX = Mathf.Min(eventData.pressPosition.x, eventData.position.x);
        float maxPosX = Mathf.Max(eventData.pressPosition.x, eventData.position.x);

        float minPosY = Math.Min(eventData.pressPosition.y, eventData.position.y);
        float maxPosY = Mathf.Max(eventData.pressPosition.y, eventData.position.y);

        UIVertex[] uiVertices = this.FilterByRectangle(GameObject.FindObjectsOfType<UIVertex>(),new Vector3(minPosX,minPosY,0),new Vector3(maxPosX,maxPosY,0));
        
        foreach(var vertex in uiVertices)
        {
            UIVertexSpawner uiSpawner = vertex.transform.parent.gameObject.GetComponent<UIVertexSpawner>();
            uiSpawner.AddDirtyVertex(vertex.VertexIndex, vertex);
        }
    }

    private UIVertex[] FilterByRectangle(UIVertex[] uiVertices, Vector3 min,Vector3 max)
    {
        List<UIVertex> results = new List<UIVertex>();
        foreach(var vertex in uiVertices)
        {
            var vertexPosition = vertex.GetComponent<RectTransform>().position;
            if(vertexPosition.x>=min.x&&vertexPosition.y>=min.y
                && vertexPosition.x <= max.x && vertexPosition.y <= max.y)
            {
                results.Add(vertex);
            }
        }
        return results.ToArray();
    }
}
