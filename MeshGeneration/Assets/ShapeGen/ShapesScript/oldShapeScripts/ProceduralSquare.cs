using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralSquare : MonoBehaviour
{
   
    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;

    public float width;
    public float height;
    public float depth;

   




    private void OnDrawGizmos()
    {
        if (_vertices != null)
        {
            Gizmos.color = Color.black;
            foreach (Vector3 vec3 in _vertices)
            {
                Gizmos.DrawSphere(vec3, 0.1f);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        int totalVertices = 8;
        _vertices = new Vector3[totalVertices];
        int totalindices = 36;
        _triangles = new int[totalindices];
        //_normals = new Vector3[totalVertices];

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;





        Generate();
    }

    private void Generate()
    {

        //4 bottom points
        //leftfront
        _vertices[0] = new Vector3(0 - (width / 2), 0, 0 - (depth / 2));
        //leftback
        _vertices[1] = new Vector3(0 - (width / 2), 0, (depth / 2));
        //rightback
        _vertices[2] = new Vector3((width / 2), 0, (depth / 2));
        //rightfront
        _vertices[3] = new Vector3((width / 2), 0, 0 - (depth / 2));

        //4 top points
        //leftfront
        _vertices[4] = new Vector3(0 - (width / 2), height, 0 - (depth / 2));
        //leftback
        _vertices[5] = new Vector3(0 - (width / 2), height, (depth / 2));
        //rightback?
        _vertices[6] = new Vector3((width / 2), height, (depth / 2));
        //rightfront
        _vertices[7] = new Vector3((width / 2), height, 0 - (depth / 2));
        

        _mesh.vertices = _vertices;

        //botface1
        _triangles[0] = 1;
        _triangles[1] = 0;
        _triangles[2] = 2;
        //botface2
        _triangles[3] = 0;
        _triangles[4] = 3;
        _triangles[5] = 2;

        //leftface1
        _triangles[6] = 0;
        _triangles[7] = 1;
        _triangles[8] = 4;

        //leftface2
        _triangles[9] = 4;
        _triangles[10] = 1;
        _triangles[11] = 5;

        //rightface1
        _triangles[12] = 2;
        _triangles[13] = 3;
        _triangles[14] = 6;
        //rightface1
        _triangles[15] = 6;
        _triangles[16] = 3;
        _triangles[17] = 7;

        //backface1
        _triangles[18] = 1;
        _triangles[19] = 2;
        _triangles[20] = 5;
        //backface2
        _triangles[21] = 5;
        _triangles[22] = 2;
        _triangles[23] = 6;

        //frontface1
        _triangles[24] = 0;
        _triangles[25] = 4;
        _triangles[26] = 3;
        //frontface2
        _triangles[27] = 3;
        _triangles[28] = 4;
        _triangles[29] = 7;

        //topface1
        _triangles[30] = 4;
        _triangles[31] = 5;
        _triangles[32] = 6;
        //topface2
        _triangles[33] = 4;
        _triangles[34] = 6;
        _triangles[35] = 7;


        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }


    

}











