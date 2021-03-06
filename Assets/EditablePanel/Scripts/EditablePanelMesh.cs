﻿/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 8:52
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class MeshData : System.Object
{
    public SerializableVector3[] vertices;
    public SerializableVector2[] uvs;
    public int[] indices;

    public SerializableVector3 scale = Vector3.one;
    public SerializableVector3 position = Vector3.zero;
}

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class EditablePanelMesh : MonoBehaviour
{
    // texture for plane mesh
    public Texture _MainTex;
   
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] indices;

    private int numColumns = 5;
    
    public int NumRows
    {
        set { this.numRows = value; }
        get { return this.numRows; }
    }
    private int numRows = 5;
    public int NumColumns
    {
        set { this.numColumns = value; }
        get { return this.numColumns; }
    }

    private float curtianWidth = 8;   // unit:meter
    private float curtainHeight = 6;

    private MeshFilter meshFilter;

    public string saveName = "EPMeshFullScreen";

    private Camera EPCamera;

    private GameObject uiSpawner;

    private void Start()
    {
        EPCamera = GameObject.Find("EPCamera").GetComponent<Camera>();
        this.LoadMeshData();
        this.Generate();
    }


    public Vector2[] GetVerticesScreenPosition()
    {
        return this.WorldToScreenPoints(this.meshFilter.mesh.vertices);
    }

    public void SaveMeshData()
    {
        Mesh refMesh = meshFilter.mesh;
        MeshData data = new MeshData();
        data.vertices = SerializableVector3.FromVector3Array(refMesh.vertices);
        data.uvs = SerializableVector2.FromVector2Array(refMesh.uv);
        data.indices = refMesh.triangles;
        data.scale = this.gameObject.transform.localScale;
        data.position = this.gameObject.transform.position;
        BinaryFormatter bf = new BinaryFormatter();

        string dirPath = Application.streamingAssetsPath + "/Saved/EPData";
        if(!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        FileStream fs = new FileStream(dirPath+"/"+saveName, FileMode.Create);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public void LoadMeshData()
    {
        string meshPath = Application.streamingAssetsPath + "/Saved/" + saveName;
        if(File.Exists(meshPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.streamingAssetsPath + "/Saved/" + saveName, FileMode.Open);
            MeshData meshData = bf.Deserialize(fs) as MeshData;
            this.vertices = SerializableVector3.ToVector3Array(meshData.vertices);
            this.uvs = SerializableVector2.ToVector2Array(meshData.uvs);
            this.indices = meshData.indices;
            this.gameObject.transform.localScale = meshData.scale;
            this.gameObject.transform.position = meshData.position;
            fs.Close();
        }
        else
        {
            this.InitializeVertices();
        }
    }


    // Start is called before the first frame update
    public void Generate()
    {
        // Construct mesh
        meshFilter = this.GetComponent<MeshFilter>();
        
        Mesh curtainMesh = new Mesh();
        curtainMesh.vertices = this.vertices;
        curtainMesh.triangles = this.indices;
        curtainMesh.uv = this.uvs;
        meshFilter.mesh = curtainMesh;

        MeshRenderer render = this.GetComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("EPlane/unlitSampler2D"));
        mat.SetTexture("_MainTex", _MainTex);
        render.material = mat;

        // Spawn UI
        GameObject EPUI = GameObject.Find("EPUI");
        GameObject uiSpawnerPrefab = Resources.Load<GameObject>("Prefabs/Components/visualVertexSpawner");
        uiSpawner = GameObject.Instantiate<GameObject>(uiSpawnerPrefab);
        uiSpawner.GetComponent<RectTransform>().SetParent(EPUI.GetComponent<RectTransform>());
        UIVertexSpawner uiSpawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        uiSpawnerComp.meshEntity = this.gameObject;
        uiSpawnerComp.SpawnVertices(this.WorldToScreenPoints(meshFilter.mesh.vertices));   
    }

    public Vector2[] WorldToScreenPoints(Vector3[] vertices)
    {
        Vector2[] uiVertices = new Vector2[vertices.Length];
        for (int vertexIndex = 0; vertexIndex < vertices.Length; ++vertexIndex)
        {
            Vector3 worldPosition = this.transform.localToWorldMatrix.MultiplyPoint(vertices[vertexIndex]);
            uiVertices[vertexIndex] = EPCamera.WorldToScreenPoint(worldPosition);
        }
        return uiVertices;
    }

    private int calcVertexIndex(int rowIndex, int colIndex, int InNumRows, int InNumColumns)
    {
        return rowIndex*InNumColumns + colIndex;
    }

    public void AddLocalScale(Vector3 deltaScale)
    {
        var uiSpawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        if (uiSpawnerComp.dirtyVertices.Count == 0) { return; }
        this.gameObject.transform.localScale += deltaScale;
        uiSpawnerComp.ReCalculateVerticesPosition();
        
        Vector3 position = uiSpawnerComp.RulerPosition();
        if (position.z != -1)
        {
            EPCamera.SendMessage("OnUpdateRulerPosition", position);
        }
    }

    public void AddWorldPosition(Vector3 deltaPosition)
    {
        var uiSpawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        if (uiSpawnerComp.dirtyVertices.Count == 0) { return; }
        this.gameObject.transform.position += deltaPosition;
        uiSpawnerComp.ReCalculateVerticesPosition();

        Vector3 position = uiSpawnerComp.RulerPosition();
        if (position.z != -1)
        {
            EPCamera.SendMessage("OnUpdateRulerPosition", position);
        }
    }

    private void InitializeVertices()
    {
        int verticesCount = this.numColumns * this.numRows;
        this.vertices = new Vector3[verticesCount];
        this.uvs = new Vector2[verticesCount];
        this.transform.position = new Vector3(0, 0, 5);
        this.transform.localScale = Vector3.one;

        Vector3 minPos = this.ScreenPositionToLocalPosition(Vector3.zero);
        Vector3 maxPos = this.ScreenPositionToLocalPosition(new Vector3(Screen.width, Screen.height));

        this.curtianWidth = maxPos.x - minPos.x;
        this.curtainHeight = maxPos.y - minPos.y;

        int trianglesCount = (this.numColumns-1)*(this.numRows-1)*2;
        this.indices = new int[trianglesCount*3];

        for(int vertexIndex = 0; vertexIndex < verticesCount; ++vertexIndex)
        {
            // Fill vertex position data
            int rowIndex = vertexIndex / this.numColumns;
            int colIndex = vertexIndex % this.numColumns;
            float startPositionX = - this.curtianWidth / 2;
            float startPositionY = this.curtainHeight / 2;
            float defaultPositionZ = 0;

            float unitWidth = this.curtianWidth / (this.numColumns - 1);
            float unitHeight = this.curtainHeight / (this.numRows - 1) ;

            Vector2 uv = new Vector2(
                colIndex/(float)(this.numColumns-1),
                (this.numRows - 1 - rowIndex) /(float)(this.numRows-1)
                );
            this.uvs[vertexIndex] = uv;
           
            Vector3 vertexPosition  = new Vector3(
                startPositionX + colIndex * unitWidth,
                startPositionY - rowIndex * unitHeight,
                defaultPositionZ
            );
            this.vertices[vertexIndex] = vertexPosition;
            if(colIndex > 0 && rowIndex < this.numRows-1)
            {
                int triangleIndex = this.calcVertexIndex(rowIndex,colIndex-1,this.numRows-1,this.numColumns-1)*2;
                // first triangle index for this vertex
                int indicesIndex = triangleIndex * 3;
                this.indices[indicesIndex+0] = vertexIndex;
                this.indices[indicesIndex+1] = this.calcVertexIndex(rowIndex+1,colIndex-1,this.numRows,this.numColumns);
                this.indices[indicesIndex+2] = this.calcVertexIndex(rowIndex,colIndex-1,this.numRows,this.numColumns);

                ++triangleIndex;
                indicesIndex = triangleIndex * 3;
                this.indices[indicesIndex+0] = vertexIndex;
                this.indices[indicesIndex+1] = this.calcVertexIndex(rowIndex+1,colIndex,this.numRows,this.numColumns);
                this.indices[indicesIndex+2] = this.calcVertexIndex(rowIndex+1,colIndex-1,this.numRows,this.numColumns);
            }
        }
    }

    public void SetVertex(int vertexIndex, Vector3 position)
    {
        List<Vector3> tempVertices = new List<Vector3>();
        meshFilter.mesh.GetVertices(tempVertices);
        tempVertices[vertexIndex] = position;
        meshFilter.mesh.SetVertices(tempVertices);
    }
    
    public void SetVertex(Dictionary<int,UIVertex> DirtyVertices)
    {
        List<Vector3> tempVertices = new List<Vector3>();
        meshFilter.mesh.GetVertices(tempVertices);
        foreach(var vertex in DirtyVertices)
        {
            tempVertices[vertex.Value.VertexIndex] = this.ScreenPositionToLocalPosition(vertex.Value.GetComponent<RectTransform>().position);
        }
        meshFilter.mesh.SetVertices(tempVertices);

        UIVertexSpawner spawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        Vector3 position = spawnerComp.RulerPosition();
        if(position.z != -1)
        {
            EPCamera.SendMessage("OnUpdateRulerPosition", position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        UIVertex uiVertex = eventData.pointerDrag.GetComponent<UIVertex>();

        Vector3 position = this.ScreenPositionToLocalPosition(Input.mousePosition);
        Vector3 nowPosition = Input.mousePosition;
       
        UIVertexSpawner spawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        
        // 移动所有选中的顶点
        if (spawnerComp.IsDirtyVerticesContainsKey(uiVertex.VertexIndex))
        {
            spawnerComp.MoveDirtyVertices(nowPosition-startPosition);
            this.SetVertex(spawnerComp.dirtyVertices);
        }
        
        startPosition = nowPosition;
    }

    
    public Vector3 ScreenPositionToLocalPosition(Vector3 screenPoint)
    {
        Vector3 position = this.transform.worldToLocalMatrix.MultiplyPoint(EPCamera.ScreenToWorldPoint(screenPoint));
        position.z = 0;
        
        return position;
    }

    private Vector3 startPosition;
    public void OnClicked(GameObject vertex)
    {
        UIVertexSpawner spawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        UIVertex uiVertex = vertex.GetComponent<UIVertex>();
        spawnerComp.rulerVertexIndex = uiVertex.VertexIndex;

        RectTransform rTrans = vertex.GetComponent<RectTransform>();
        startPosition = Input.mousePosition;

        if(!spawnerComp.dirtyVertices.ContainsKey(spawnerComp.rulerVertexIndex))
        {
            spawnerComp.EmptyDirtyVertices();
            spawnerComp.AddDirtyVertex(uiVertex.VertexIndex, uiVertex);
        }
        EPCamera.SendMessage("OnUpdateRulerPosition", rTrans.position);
    }

    public void OnReleased(GameObject vertex)
    {
        //EPCamera.SendMessage("OnClickedEmpty");
    }

    public void Average()
    {
        var uiSpawnerComp = uiSpawner.GetComponent<UIVertexSpawner>();
        if (uiSpawnerComp.dirtyVertices.Count == 0) { return; }

        Vector3[] tempVertices = this.meshFilter.mesh.vertices;
        float minX = 5000;
        for(int index=0;index <= this.numColumns*(this.NumRows-1);index+=this.NumColumns)
        {
            minX = Mathf.Min(minX,tempVertices[index].x);
        }
        float maxX = -5000;
        for(int index=this.numColumns-1;index<((this.NumColumns*this.NumRows));index+=this.NumColumns)
        {
            maxX = Mathf.Max(maxX, tempVertices[index].x);
        }

        float maxY = -5000;
        for (int index = 0; index < this.numColumns; ++index) 
        {
            maxY = Mathf.Max(maxY, tempVertices[index].y);
        }

        float minY = 5000;
        for(int index=(this.numColumns*(this.NumRows-1));index<(this.numColumns*this.NumRows);++index)
        {  
            minY = Mathf.Min(minY, tempVertices[index].y);
        }
        
        this.InitializeVertices();
        Destroy(this.uiSpawner);

        this.curtianWidth = maxX - minX;
        this.curtainHeight = maxY - minY;
        this.Generate();
    }
}
