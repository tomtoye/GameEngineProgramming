using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralCylinder : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;

    
    public float height;
    public float radius;
    public int noOfEdgesOnFace = 10;
    


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
        int totalVertices = (noOfEdgesOnFace*2) + 2;
        _vertices = new Vector3[totalVertices];
        int totalindices = (noOfEdgesOnFace) * 12;
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
        //topcentre point
        _vertices[1] = new Vector3(0, height, 0);



        //edge of circle coords
        //x = xOrigin + r* cos(theta)
        //y = yOrigin + r* sin(theta)



        float x;
        float y;
        float avgRadian = (2*Mathf.PI) / noOfEdgesOnFace;

        for (int i = 0; i < noOfEdgesOnFace; i++)
        {

            x = radius * Mathf.Cos((avgRadian) * i);
            y = radius * Mathf.Sin((avgRadian) * i);

            //bottompoint at x,y
            _vertices[i + 2] = new Vector3(x, 0, y);
            //toppoint at x,y
            _vertices[i + 2 + noOfEdgesOnFace] = new Vector3(x, height, y);

           
        }

        _mesh.vertices = _vertices;


        //everythingfinetillhere

        
        int j = 0;
        //bottom cylinder
        for (int i = 0; i < noOfEdgesOnFace; i++)
        {
            j = i * 3;

            _triangles[j] = i + 3;
            _triangles[j + 1] = 0;
            _triangles[j + 2] = i + 2;
            if (i == noOfEdgesOnFace - 1)
            {
                _triangles[j] = 2;
            }

        }



        //top cylinder
        for (int i = noOfEdgesOnFace; i < (noOfEdgesOnFace*2); i++)
        {
            j = i * 3;
        
        
            if (i == noOfEdgesOnFace)
            {
                 _triangles[j] = (noOfEdgesOnFace * 2) + 1;    
            }
            else
            {
                _triangles[j] = i + 1;
            }

            _triangles[j + 1] = 1;

            if (i == (noOfEdgesOnFace * 2) - 1)
            {
                _triangles[j + 2] = (noOfEdgesOnFace * 2) + 1;
            }
            else
            {
            _triangles[j+ 2] = i + 2;
            }


  
        
        }


        //bodyofcylinder pass1

        for (int i = 0; i < noOfEdgesOnFace; i++)
        {
            j = (noOfEdgesOnFace* 6) + (i * 3);
        
            int temp = ((noOfEdgesOnFace * 2) + (i - (noOfEdgesOnFace - 2)));
            //Debug.Log(temp);


            _triangles[j] = i + 2;
            _triangles[j + 1] = temp;
            _triangles[j + 2] = i + 3;
            if (i == noOfEdgesOnFace - 1)
            {
                _triangles[j + 2] = 2;
            }

        }


        //bodyofcylinder pass2

        for (int i = 0; i < noOfEdgesOnFace; i++)
        {
            j = (noOfEdgesOnFace * 9) + (i * 3);

            int temp = ((noOfEdgesOnFace * 2) + (i - (noOfEdgesOnFace - 2)));

            if (i + 3 == (noOfEdgesOnFace + 2))
            {
                _triangles[j] = 2;
            }
            else
            {
                _triangles[j] = i + 3;
            }



            _triangles[j + 1] = temp;

            if (temp == (noOfEdgesOnFace * 2) + 1)
            {
                _triangles[j + 2] = noOfEdgesOnFace + 2;
            }
            else
            {
                _triangles[j + 2] = temp + 1;
            }






        }

        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }
}






