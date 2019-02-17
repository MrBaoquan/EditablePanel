using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class EditablePlaneMesh : MonoBehaviour
{

    private Vector3[] vertices;
    private int[] indices;

    private int numColumns = 5;
    private int numRows = 5;

    private float curtianWidth = 8;   // unit:meter
    private float curtainHeight = 6;

    // Start is called before the first frame update
    private void Start()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        this.InitializeVertices();

        Mesh curtainMesh = new Mesh();
        curtainMesh.vertices = this.vertices;
        curtainMesh.triangles = this.indices;
        meshFilter.mesh = curtainMesh;

        
    }

    private int calcVertexIndex(int rowIndex, int colIndex, int InNumRows, int InNumColumns)
    {
        return rowIndex*InNumColumns + colIndex;
    }

    private void InitializeVertices()
    {
        int verticesCount = this.numColumns * this.numRows;
        this.vertices = new Vector3[verticesCount];

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

            Vector3 vertexPosition  = new Vector3(
                startPositionX + colIndex * unitWidth,
                startPositionY - rowIndex * unitHeight,
                defaultPositionZ
            );
            vertices[vertexIndex] = vertexPosition;

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

                Debug.Log(triangleIndex);
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
