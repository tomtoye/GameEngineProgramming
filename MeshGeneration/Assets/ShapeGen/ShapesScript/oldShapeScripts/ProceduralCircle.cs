using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralCircle : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;



    public float radius;
    public int noOfPointsAroundCircumference = 8;



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

        int totalVertices = noOfPointsAroundCircumference + 1;
        _vertices = new Vector3[totalVertices];
        int totalindices = (noOfPointsAroundCircumference) * 3;
        _triangles = new int[totalindices];
        //_normals = new Vector3[totalVertices];

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;





        Generate();
    }

    private void Generate()
    {
        //bottomcentre point
        _vertices[0] = new Vector3(0, 0, 0);

        float x;
        float y;
        float avgDegree = 360 / noOfPointsAroundCircumference + 1;
        //Debug.Log(noOfPointsAroundCircumference);

        for (int i = 0; i < noOfPointsAroundCircumference; i++)
        {
            x = radius * Mathf.Cos((avgDegree * (Mathf.PI / 180)) * i);
            y = radius * Mathf.Sin((avgDegree * (Mathf.PI / 180)) * i);

            _vertices[i + 1] = new Vector3(x, 0, y);


        }

        _mesh.vertices = _vertices;


        //everythingfinetillhere


        int j = 0;

        for (int i = 0; i < noOfPointsAroundCircumference; i++)
        {

            j = i * 3;

            _triangles[j] = i + 1;
            _triangles[j + 1] = 0;
            _triangles[j + 2] = i + 2;
            if (i == noOfPointsAroundCircumference - 1)
            {
                _triangles[j + 2] = 1;
            }

        }


        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }




}

