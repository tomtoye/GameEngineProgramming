using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]



public class ProceduralSphere : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;

    //heightofPrism/Cylinder
    public float height;
    //radiusofPrism/Cylinder
    public float radius;
    //no of edges on each end face of prism, e.g if =5 then we have pentahedron
    public int noOfHorizontalEdges = 10;
    public int noOfVerticalEdges = 10;


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
        //verts for each one on end face then an extra 2 for centre points on each face 
        int totalVertices = (noOfHorizontalEdges * noOfVerticalEdges) + 2;
        _vertices = new Vector3[totalVertices];


        //need 3 for each tri we draw, we draw noOfEdgeFace*4*3
        // ^ each set of 3 for every horizontal set x2 to make square (we pair up top and bottom of spheres as they = same as horizontal set together) * no of vert sections we have
        int totalindices = noOfVerticalEdges * noOfHorizontalEdges * 6;
        _triangles = new int[totalindices];

        //_normals = new Vector3[totalVertices];

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;

        Generate();
    }


    private void Generate()
    {

        //MapEachVert
        CreateVertices();
        _mesh.vertices = _vertices;


        //MapTri's

        for (int i = 0; i < noOfHorizontalEdges; i++)
        {
            int j = i * 3;
            
            //int test = (((noOfHorizontalEdges * noOfVerticalEdges) - 2) +i);
            //Debug.Log(test);


            //int basej = ((((noOfVerticalEdges - 1) * 3 * noOfHorizontalEdges) * 2) + noOfHorizontalEdges * 2) + 3;
            //int basei = (noOfHorizontalEdges * noOfHorizontalEdges) - 3;

            BottomFaces(i, j); //tested works
            //topFaces(i + basei, j + basej);

            // Debug.Log(i + basei);



            //BodyOfCylinderPass1(i, j, temp);
            //BodyOfCylinderPass2(i, j, temp);


            //_triangles[45] = 7;
            //_triangles[46] = 10;
            //_triangles[47] = 8;
            //
            //_triangles[48] = 8;
            //_triangles[49] = 10;
            //_triangles[50] = 9;
            //
            //_triangles[51] = 9;
            //_triangles[52] = 10;
            //_triangles[53] = 7;

            




        }

        int basei = (noOfHorizontalEdges * noOfVerticalEdges) - noOfHorizontalEdges + 1;

        int basej = (noOfHorizontalEdges * (noOfVerticalEdges - 1)) + 1;


        for (int i = 0; i < noOfHorizontalEdges; i++)
        {
            //int basei = 
            int tempi = i + basei;
            //int j = i * 3;
            //int tempj = (noOfHorizontalEdges*(noOfVerticalEdges-1)) + 1;

            _triangles[basej + i + 0] = tempi;

            _triangles[basej + i + 1] = (noOfHorizontalEdges * noOfVerticalEdges) + 1;

            _triangles[basej + i + 2] = tempi;
        }


            _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

    }

    private void CreateVertices()
    {
        _vertices[0] = new Vector3(0, 0, 0);
        //topcentre point

        //-1 but included in what should be +2
        _vertices[(noOfHorizontalEdges * noOfVerticalEdges) + 1] = new Vector3(0, height, 0);



        //how to find each vert of circle/prism coordinates
        //x = xOrigin + r* cos(theta)
        //y = yOrigin + r* sin(theta)

        float x;
        float y;
        float z;
        //use just rads, is cleaner and dont break stuff :/

        //find gap in rads between each vert
        float avgHorRad = (2 * Mathf.PI) / noOfHorizontalEdges;
        //float avgVertRad = (2 * Mathf.PI) / noOfVerticalEdges;
        //for no of verts we want per face
        int k = 0;
        float heightDiffBetweenVerticalLayers = height / (noOfVerticalEdges + 1);
        for (int i = 0; i < noOfVerticalEdges; i++)
        {
            z = (i + 1) * heightDiffBetweenVerticalLayers;
            for (int j = 0; j < noOfHorizontalEdges; j++)
            {
                k++;
                //find next x,y coord along using circle theorem and our avgHorRad
                x = radius * Mathf.Cos((avgHorRad) * j);
                y = radius * Mathf.Sin((avgHorRad) * j);

      
                //place a vert on bottom face at x ,bottomofprims/circle , y
                _vertices[k] = new Vector3(x, z, y);
                
            }
        }


    }

    private void BottomFaces(int i, int j)
    {
        _triangles[j] = i + 2;
        _triangles[j + 1] = 0;
        _triangles[j + 2] = i + 1;
        if (i == noOfHorizontalEdges - 1) _triangles[j] = 1;
    }

    private void topFaces(int i, int j)
    {

        Debug.Log("1st " + (i+1) + " 2nd " + (i+2));


        Debug.Log("j value " + j); //j is fine


        //FIRST VERT
        _triangles[j] = i + 1;


        //SECOND VERT
        _triangles[j + 1] = (noOfHorizontalEdges * noOfVerticalEdges) + 1;


        //THIRD VERT
        _triangles[j + 2] = i + 2;
        //if (i == (noOfHorizontalEdges * noOfVerticalEdges) - 1) _triangles[j + 2] = 7;


        //Debug.Log("second vert" + (i + 2));
        //Debug.Log(i);

    }
    private void BodyOfCylinderPass1(int i, int j, int temp)
    {

        int k = (noOfHorizontalEdges * 6) + j;

        _triangles[k] = i + 2;
        _triangles[k + 1] = temp;
        _triangles[k + 2] = i + 3;
        if (i == noOfHorizontalEdges - 1) _triangles[k + 2] = 2;



    }
    private void BodyOfCylinderPass2(int i, int j, int temp)
    {
        int k = (noOfHorizontalEdges * 9) + j;

        _triangles[k] = i + 3;
        if (i + 3 == (noOfHorizontalEdges + 2)) _triangles[k] = 2;

        _triangles[k + 1] = temp;



        _triangles[k + 2] = temp + 1;
        if (temp == (noOfHorizontalEdges * 2) + 1) _triangles[k + 2] = noOfHorizontalEdges + 2;

    }


}