using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class Procedural4SidedPyramid : MonoBehaviour
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
        int totalVertices = 5;
        _vertices = new Vector3[totalVertices];
        int totalindices = 18;
        _triangles = new int[totalindices];
        //_normals = new Vector3[totalVertices];

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;





        Generate();
    }

    private void Generate()
    {

        //4 bottom squares
        _vertices[0] = new Vector3(0 - (width / 2), 0, 0 - (depth / 2));
        _vertices[1] = new Vector3(0 - (width / 2), 0, (depth / 2));
        _vertices[2] = new Vector3((width / 2), 0, (depth / 2));
        _vertices[3] = new Vector3((width / 2), 0, 0 - (depth / 2));

        //top point
        _vertices[4] = new Vector3(0, height, 0);

        _mesh.vertices = _vertices;

        //botface
        _triangles[0] = 1;
        _triangles[1] = 0;
        _triangles[2] = 2;
        //botface2
        _triangles[3] = 0;
        _triangles[4] = 3;
        _triangles[5] = 2;
        //frontface
        _triangles[6] = 0;
        _triangles[7] = 4;
        _triangles[8] = 3;
        //leftface
        _triangles[9] = 1;
        _triangles[10] = 4;
        _triangles[11] = 0;

        //rightface
        _triangles[12] = 3;
        _triangles[13] = 4;
        _triangles[14] = 2;

        //backface
        _triangles[15] = 2;
        _triangles[16] = 4;
        _triangles[17] = 1;


        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }




}
