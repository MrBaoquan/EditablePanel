/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 14:15
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVertexSpawner : MonoBehaviour
{
    public GameObject meshEntity;

    public Dictionary<int, UIVertex> dirtyVertices = new Dictionary<int, UIVertex>();

    public int rulerVertexIndex = -1;
   
   public void SpawnVertices(Vector2[] vertices)
   {
        GameObject vertexUIObject = Resources.Load<GameObject>("Prefabs/Components/vertex");
        for(int index = 0; index < this.gameObject.transform.childCount; ++index)
        {
            GameObject.Destroy(this.gameObject.transform.GetChild(index).gameObject);
        }

        for (int vertexIndex = 0; vertexIndex < vertices.Length; ++vertexIndex) 
        {
            GameObject newUIVertex = GameObject.Instantiate<GameObject>(vertexUIObject);
            newUIVertex.transform.SetParent(this.gameObject.transform);
            RectTransform vertexUITransform = newUIVertex.GetComponent<RectTransform>();
            vertexUITransform.position = new Vector3(vertices[vertexIndex].x, vertices[vertexIndex].y, 0);
            UIVertex uiVertex = newUIVertex.GetComponent<UIVertex>();
            uiVertex.VertexIndex = vertexIndex;
            uiVertex.OnDrag += meshEntity.GetComponent<EditablePanelMesh>().OnDrag;
            uiVertex.OnClicked += meshEntity.GetComponent<EditablePanelMesh>().OnClicked;
            uiVertex.OnReleased += meshEntity.GetComponent<EditablePanelMesh>().OnReleased;
        }
   }

    public void ReCalculateVerticesPosition()
    {
        Vector2[] positions = meshEntity.GetComponent<EditablePanelMesh>().GetVerticesScreenPosition();
        UIVertex[] allVertices = this.transform.GetComponentsInChildren<UIVertex>();

        for (int vertexIndex = 0; vertexIndex < allVertices.Length; ++vertexIndex)
        {
            UIVertex vertex = allVertices[vertexIndex];
            vertex.GetComponent<RectTransform>().position = positions[vertex.VertexIndex];
        }
    }

    public void AddDirtyVertex(int vertexIndex,UIVertex vertex)
    {
        dirtyVertices.Add(vertexIndex, vertex);
        vertex.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/selected");
    }

    public bool IsDirtyVerticesContainsKey(int vertexIndex)
    {
        return dirtyVertices.ContainsKey(vertexIndex);
    }

    public void EmptyDirtyVertices()
    {
        foreach(var vertex in dirtyVertices)
        {
            vertex.Value.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/default");
        }
        dirtyVertices.Clear();
    }

    public void MoveDirtyVertices(Vector3 delta)
    {
        foreach(var vertex in dirtyVertices)
        {
            vertex.Value.GetComponent<RectTransform>().position += delta;
        }
    }

    public void DoChange()
    {
        meshEntity.GetComponent<EditablePanelMesh>().SetVertex(this.dirtyVertices);
    }

    public Vector3 RulerPosition()
    {
        if(dirtyVertices.ContainsKey(rulerVertexIndex))
        {
            return dirtyVertices[rulerVertexIndex].GetComponent<RectTransform>().position;
        }
        return new Vector3(0,0,-1);
    }
}
