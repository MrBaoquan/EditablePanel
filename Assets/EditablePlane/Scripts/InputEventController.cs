/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 15:13
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEventController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            GameController.Instance.Settings.ToggleWireframe();
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaY = Input.GetAxis("Vertical");
            UIVertexSpawner[] spawners = GameObject.FindObjectsOfType<UIVertexSpawner>();
            foreach (var spawner in spawners)
            {
                spawner.MoveDirtyVertices(new Vector3(deltaX, deltaY, 0));
                spawner.DoChange();
            }
        }
    }
}
