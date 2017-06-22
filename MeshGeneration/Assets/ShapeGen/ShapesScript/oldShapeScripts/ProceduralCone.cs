using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralCone : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;


    public float height;
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
        int totalVertices = (noOfPointsAroundCircumference * 2) + 2;
        _vertices = new Vector3[totalVertices];
        int totalindices = (noOfPointsAroundCircumference) * 12;
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
        //x = x0 + r* cos(theta)
        //y = y0 + r* sin(theta)



        float x;
        float y;
        float avgDegree = 360 / noOfPointsAroundCircumference;
        //Debug.Log(avgDegree);

        for (int i = 0; i < noOfPointsAroundCircumference; i++)
        {
            x = radius * Mathf.Cos((avgDegree * (Mathf.PI / 180)) * i);
            y = radius * Mathf.Sin((avgDegree * (Mathf.PI / 180)) * i);

            //Debug.Log((avgDegree * (Mathf.PI / 180)) * i);
            //Debug.Log(y);

            _vertices[i + 2] = new Vector3(x, 0, y);
            _vertices[i + 2 + noOfPointsAroundCircumference] = new Vector3(x, height, y);


        }

        _mesh.vertices = _vertices;


        //everythingfinetillhere

        
        int j = 0;
        //bottom cylinder
        for (int i = 0; i < noOfPointsAroundCircumference; i++)
        {
            j = i * 3;

            _triangles[j] = i + 3;
            _triangles[j + 1] = 0;
            _triangles[j + 2] = i + 2;
            if (i == noOfPointsAroundCircumference - 1)
            {
                _triangles[j] = 2;
            }


            //k = (6 * noOfPointsAroundCircumference) + (i*3);
            //Debug.Log(k);
            //Debug.Log(i);
            //_triangles[k] = i + 2;
            //_triangles[k + 1] =  ((-1 * noOfPointsAroundCircumference) +2+i);
            //
            ////_triangles[k + 2] = i + 3;
            //if (i == noOfPointsAroundCircumference + 1)
            //{
            //    _triangles[k + 2] = 2;
            //}
            //else
            //{
            //    _triangles[k + 2] = i + 3;
            //}








        }



        //top cylinder
        for (int i = noOfPointsAroundCircumference; i < (noOfPointsAroundCircumference * 2); i++)
        {
            j = i * 3;


            if (i == noOfPointsAroundCircumference)
            {
                _triangles[j] = (noOfPointsAroundCircumference * 2) + 1;
            }
            else
            {
                _triangles[j] = i + 1;
            }

            _triangles[j + 1] = 1;

            if (i == (noOfPointsAroundCircumference * 2) - 1)
            {
                _triangles[j + 2] = (noOfPointsAroundCircumference * 2) + 1;
            }
            else
            {
                _triangles[j + 2] = i + 2;
            }

            //int temp = j + 1;
            //int temp2 = j + 2;
            //Debug.Log("j= " + j + "i= " + (i + 2));
            //Debug.Log("j= " + temp + "i= 0");
            //Debug.Log("j= " + temp2 + "i= " + (i + 3));


        }


        //bodyofcylinder pass1

        //k = (6 * noOfPointsAroundCircumference) + (i*3);
        //Debug.Log(k);
        //Debug.Log(i);
        //_triangles[k] = i + 2;
        //_triangles[k + 1] =  ((-1 * noOfPointsAroundCircumference) +2+i);
        //
        ////_triangles[k + 2] = i + 3;
        //if (i == noOfPointsAroundCircumference + 1)
        //{
        //    _triangles[k + 2] = 2;
        //}
        //else
        //{
        //    _triangles[k + 2] = i + 3;
        //}

        for (int i = 0; i < noOfPointsAroundCircumference; i++)
        {
            j = (noOfPointsAroundCircumference * 6) + (i * 3);

            _triangles[j] = i + 2;
            _triangles[j + 1] = 1;
            _triangles[j + 2] = i + 3;
            if (i == noOfPointsAroundCircumference - 1)
            {
                _triangles[j + 2] = 2;
            }




            //Debug.Log(_triangles[j]);

        }


        //_triangles[36] = 2;
        //_triangles[37] = ((noOfPointsAroundCircumference*2) -4);
        //_triangles[38] = 3;
        //
        //
        //_triangles[39] = 3;
        //_triangles[40] = ((noOfPointsAroundCircumference * 2) - 3);
        //_triangles[41] = 4;
        //
        //_triangles[42] = 4;
        //_triangles[43] = ((noOfPointsAroundCircumference * 2) - 2);
        //_triangles[44] = 5;
        //
        //_triangles[45] = 5;
        //_triangles[46] = ((noOfPointsAroundCircumference * 2) - 1);
        //_triangles[47] = 6;
        //
        //_triangles[48] = 6;
        //_triangles[49] = ((noOfPointsAroundCircumference * 2));
        //_triangles[50] = 7;
        //
        //_triangles[51] = 7;
        //_triangles[52] = ((noOfPointsAroundCircumference * 2) + 1);
        //_triangles[53] = 2;






        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();
    }
}


