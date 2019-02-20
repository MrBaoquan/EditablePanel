/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 15:13
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventController : MonoBehaviour
{
    public float moveMeshSpeed = 0.1f;
    public float scaleMeshFactor = 0.05f;
    public float moveVertexSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaY = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameController.Instance.Settings.ToggleWireframe();
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();

            if (Input.GetKeyDown(KeyCode.S))
            {    
                foreach (var mesh in meshes)
                {
                    mesh.SaveMeshData();
                }
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                foreach (var mesh in meshes)
                {
                    mesh.Average();
                }
            }

            foreach(var mesh in meshes )
            {
                mesh.AddWorldPosition(new Vector3(deltaX, deltaY, 0)*moveMeshSpeed);
            }
        }
        else
        {
            UIVertexSpawner[] spawners = GameObject.FindObjectsOfType<UIVertexSpawner>();
            foreach (var spawner in spawners)
            {
                spawner.MoveDirtyVertices(new Vector3(deltaX, deltaY, 0)*moveVertexSpeed);
                spawner.DoChange();
            }
        }

       
        if(Input.GetKey(KeyCode.W))
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();
                foreach (var mesh in meshes)
                {
                    mesh.AddLocalScale(Vector3.right * scaleMeshFactor);
                }
            }else if(Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();
                foreach (var mesh in meshes)
                {
                    mesh.AddLocalScale(Vector3.right * -scaleMeshFactor);
                }
            }
        }

        if (Input.GetKey(KeyCode.H))
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();
                foreach (var mesh in meshes)
                {
                    mesh.AddLocalScale(Vector3.up * scaleMeshFactor);
                }
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();
                foreach (var mesh in meshes)
                {
                    mesh.AddLocalScale(Vector3.up * -scaleMeshFactor);
                }
            }
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            EditablePanelMesh[] meshes = GameObject.FindObjectsOfType<EditablePanelMesh>();
            foreach (var mesh in meshes)
            {
                mesh.SaveMeshData();
            }
        }
#endif
    }
}
