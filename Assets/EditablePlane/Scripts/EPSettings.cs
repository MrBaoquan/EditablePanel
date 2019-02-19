/** Copyright (c) 2019 mrma617@gmail.com
 *  Author: MrBaoquan
 *  CreateTime: 2019-2-18 8:52
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPSettings : MonoBehaviour
{
    public Material lineMat;
    private bool bRuler = true;
    public bool Ruler
    {
        set { this.bRuler = value; }
        get { return this.bRuler; }
    }

    private bool wireframe;

    private Vector3 rulerPosition;

    public void ToggleWireframe()
    {
        this.wireframe = !this.wireframe;
    }

    private void OnPreRender()
    {
        GL.wireframe = this.wireframe;
    }



    private void OnClickedEmpty()
    {
        this.bRuler = false;
    }

    private void OnUpdateRulerPosition(Vector3 InPosition)
    {
        this.bRuler = true;
        Vector3 position = new Vector3(InPosition.x / Screen.width, InPosition.y / Screen.height, 1);
        this.rulerPosition = position;
    }

    private void OnPostRender()
    {
        GL.wireframe = this.wireframe;
       
        if(bRuler)
        {
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.LoadOrtho();

            Vector3 mousePosition = this.rulerPosition;
            mousePosition.z = 1;

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(mousePosition);
            GL.Vertex(new Vector3(mousePosition.x, 0, 1));
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(mousePosition);
            GL.Vertex(new Vector3(Screen.width,mousePosition.y,1));
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(new Color(0.5f,0.0f,0.0f,0.5f));
            GL.Vertex(mousePosition);
            GL.Vertex(new Vector3(0, mousePosition.y, 1));
            GL.End();

            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex(mousePosition);
            GL.Vertex(new Vector3(mousePosition.x, Screen.height, 1));
            GL.End();

            GL.PopMatrix();
        }
        
    }
}
