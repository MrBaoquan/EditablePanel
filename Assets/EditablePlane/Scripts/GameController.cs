/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 14:37
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject editableMesh;
    public Camera rendererCamera;
    public UIVertexSpawner uiVertexSpawner;
    public EPSettings Settings;
    public UIEventController uiEventController;

    private static GameController instance;
    public static GameController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ReGeneratePlane();
    }

    public void SetMeshSize(int rows,int columns)
    {
        EditablePanelMesh editablePM = editableMesh.GetComponent<EditablePanelMesh>();
        editablePM.NumRows = rows;
        editablePM.NumColumns = columns;
    }

    public void ReGeneratePlane()
    {
        //this.SpawnUIVertices();
    }

}
