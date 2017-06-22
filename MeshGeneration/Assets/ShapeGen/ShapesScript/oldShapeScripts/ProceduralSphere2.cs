using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]



public class ProceduralSphere2 : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;

    //heightofPrism/Cylinder
    public float height;
    //radiusofPrism/Cylinder
    public float width;
    //heightofPrism/Cylinder
    public float depth;
   
    //no of edges on each end face of prism, e.g if =5 then we have pentahedron
    public int noOfHorizontalEdges = 10;
    public int noOfVerticalEdges = 10;

    public int nbLat = 10;
    public int nbLong = 10;


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

        //BottomFaces();
        //topFaces();

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



        int j = 0;
        int i;
        for (i = 0; i < noOfHorizontalEdges; i++)
        {
            _triangles[j++] = i + 2;
            if (i == noOfHorizontalEdges - 1) _triangles[j-1] = 1;
            _triangles[j++] = i + 1;
           
            _triangles[j++] = 0;
        }


        ////Middle
       //for (i = 0; i < noOfVerticalEdges - 1; i++)
       //{
       //    for (int k = 0; k < noOfHorizontalEdges; k++)
       //    {
       //        int current = k + i * (noOfHorizontalEdges + 1) + 1;
       //        int next = current + noOfHorizontalEdges + 1;
       //
       //        _triangles[j++] = current;
       //        _triangles[j++] = current + 1;
       //        _triangles[j++] = next + 1;
       //
       //        _triangles[j++] = current;
       //        _triangles[j++] = next + 1;
       //        _triangles[j++] = next;
       //    }
       //}
        //
        //Bottom Cap
        for (i = 0; i < noOfHorizontalEdges; i++)
        {

            Debug.Log(_vertices.Length - (noOfHorizontalEdges + 1));
            _triangles[j++] = _vertices.Length - 1;

            _triangles[j++] = _vertices.Length - (i + 2) - 1;
            if (i == noOfHorizontalEdges - 1) _triangles[j - 1] = _vertices.Length - 2;

            _triangles[j++] = _vertices.Length - (i + 1) - 1;
    
            

        }








        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

    }

    //allverts placed correctly safteypig
    private void CreateVertices()
    {
        //botcentre point
        _vertices[(noOfHorizontalEdges * noOfVerticalEdges) + 1] = new Vector3(0, -height, 0);
        
        //topcentre point (last in array)
        _vertices[0] = new Vector3(0, height, 0);

        float x, y, z;
        int vert = 0;

        float fPI = Mathf.PI;
        float f2PI = 2 * Mathf.PI;
        for (int i = 0; i < noOfVerticalEdges; i++)
        {
           
            //+1's because we have already set up top and bottom verts
            float theta = fPI * (i +1 ) / (noOfVerticalEdges + 1);
            //y is only going to change each time we move up a row on sphere so can do in outer loop
            y = height * (Mathf.Cos(theta));

            for (int j = 0; j < noOfHorizontalEdges; j++)
            {
                vert++;
                //find next x,z coord along
                //having a seperate hieght and radius allows us to have more varied shapes e.g rugby ball style spheres
                float phi = f2PI * j / noOfHorizontalEdges;

                x = width * (Mathf.Sin(theta) * Mathf.Cos(phi));
                z = depth * (Mathf.Sin(theta) * Mathf.Sin(phi));

                //place vert
                _vertices[vert] = new Vector3(x, y, z);
        
            }
        }
    }

    private void BottomFaces()
    {
        int j = 0;
        int i = 0;
        for (i = 0; i < noOfHorizontalEdges; i++)
        {
           

            _triangles[j] = i + 1;
            _triangles[j + 1] = 0;
            _triangles[j + 2] = i + 2;
            if (i == noOfHorizontalEdges - 1) _triangles[j + 2] = 1;


            for (int k = 0; k < noOfHorizontalEdges; i++)
            {
                //j = j + 3;

                _triangles[j] = i + 1;
                _triangles[j + 1] = 0;
                _triangles[j + 2] = i + 2;
                if (i == noOfHorizontalEdges - 1) _triangles[j + 2] = 1;
            }

             j = j + 3;
        }



        //for (i = ; i < noOfHorizontalEdges; i++)
        //{
        //    j = j + 3;
        //
        //    _triangles[j] = i + 1;
        //    _triangles[j + 1] = 0;
        //    _triangles[j + 2] = i + 2;
        //    if (i == noOfHorizontalEdges - 1) _triangles[j + 2] = 1;
        //}




    }

    private void topFaces()
    {
        int basei = (noOfVerticalEdges * noOfHorizontalEdges) - (noOfHorizontalEdges - 1);
        int basej = (-noOfHorizontalEdges * 3) + (noOfHorizontalEdges * noOfVerticalEdges * 6) - noOfVerticalEdges;

        int j = basej;
        for (int i = 0; i < noOfHorizontalEdges; i++)
        {
            j = j +3;
            int tempi = i + basei;
            Debug.Log(tempi);
            //Debug.Log(j);


            _triangles[j + 0] = tempi + 0;
            _triangles[j + 1] = (noOfHorizontalEdges * noOfVerticalEdges) + 1;
            _triangles[j + 2] = tempi + +1;
            //if (tempi == (noOfHorizontalEdges * noOfVerticalEdges)) _triangles[j + 0] = tempi; //- noOfHorizontalEdges + 1;
        }





        //Debug.Log("1st " + (i + 1) + " 2nd " + (i + 2));
        //
        //
        //Debug.Log("j value " + j); //j is fine
        //
        //
        ////FIRST VERT
        //_triangles[j] = i + 1;
        //
        //
        ////SECOND VERT
        //_triangles[j + 1] = (noOfHorizontalEdges * noOfVerticalEdges) + 1;
        //
        //
        ////THIRD VERT
        //_triangles[j + 2] = i + 2;
        ////if (i == (noOfHorizontalEdges * noOfVerticalEdges) - 1) _triangles[j + 2] = 7;
        //
        //
        ////Debug.Log("second vert" + (i + 2));
        ////Debug.Log(i);

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