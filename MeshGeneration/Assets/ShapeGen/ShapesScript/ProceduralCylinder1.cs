using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]



public class ProceduralCylinder1 : MonoBehaviour
{


    private Vector3[] _vertices;
    //private Vector3[] _normals;

    private int[] _triangles;

    Mesh _mesh;

    //heightofPrism/Cylinder
    public float height = 1;
    //radiusofPrism/Cylinder

    public float radius = 1;

    //no of edges on each end face of prism, e.g if =5 then we have pentahedron
    public int noOfEdgesOnFace = 10;

    private int currentVert = 0;


    // Use this for initialization
    void Start()
    {
        Generate();
    }

    //quick update function added to show off shape in scene
    void Update()
    {
        if (GameObject.Find("ShapesCanvas(Clone)"))
        {
            float tempheight = GameObject.Find("cylinderHeightSlider").GetComponent<Slider>().value;
            float tempradius = GameObject.Find("cylinderRadiusSlider").GetComponent<Slider>().value;

            int tempLoops = (int)GameObject.Find("cylinderLoopsSlider").GetComponent<Slider>().value;

            if (height != tempheight) { height = tempheight; Generate(); }
            if (radius != tempradius) { radius = tempradius; Generate(); }
     
            if (noOfEdgesOnFace != tempLoops) noOfEdgesOnFace = tempLoops; Generate();
        }


    }



    public void Generate()
    {
        //verts for each one on end face then an extra 2 for centre points on each face 
        int totalVertices = (noOfEdgesOnFace * 2) + 2;
        _vertices = new Vector3[totalVertices];


        //need 3 for each tri we draw, we draw noOfEdgeFace*4*3
        // ^ each set of 3 for top face, bottom face, then 2 sets for column (we need to makes squares here)
        //so 4*3*no of columns
        int totalindices = (noOfEdgesOnFace) * 4 * 3;
        _triangles = new int[totalindices];

        //_normals = new Vector3[totalVertices];

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;

        //MapEachVert
        CreateVertices();
  
        currentVert = 0;


        //MapTri's
        for (int i = 0; i < noOfEdgesOnFace; i++)
        {
            int iTopLoop = i + noOfEdgesOnFace + 2;

            BottomFaces(i); //draws faces on bottom cap
            topFaces(i + noOfEdgesOnFace); //draws faces on top cap
            BodyOfCylinderPass1(i, iTopLoop); //does one loop through body of cylinder
            BodyOfCylinderPass2(i, iTopLoop); // does another so we get quads along body
        }

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

    }

    private void CreateVertices()
    {
        _vertices[0] = new Vector3(0, -height, 0);
        //topcentre point
        _vertices[1] = new Vector3(0, height, 0);



        //how to find each vert of circle/prism coordinates
        //x = xOrigin + r* cos(theta)
        //y = yOrigin + r* sin(theta)

        float x, y;
        //use just rads, is cleaner and dont break stuff :/

        //find gap in rads between each vert
        float pi2 = 2 * Mathf.PI;
        float avgRad = pi2 / noOfEdgesOnFace;

        //for no of verts we want per face
        for (int i = 0; i < noOfEdgesOnFace; i++)
        {
            //find next x,y coord along using circle theorem and our avgRad
            x = radius * Mathf.Cos((avgRad) * i);
            y = radius * Mathf.Sin((avgRad) * i);

            //place a vert on bottom face at x ,bottomofprims/circle , y
            _vertices[i + 2] = new Vector3(x, -height, y);
            //place a vert on top face at at x ,topofprims/circle , y
            _vertices[i + 2 + noOfEdgesOnFace] = new Vector3(x, height, y);
        }
    }

    //draw a triangle on bottomface between verts i + 3 and i + 2, and centre point
    private void BottomFaces(int i)
    {
        _triangles[currentVert++] = i + 3;
        if (i == noOfEdgesOnFace - 1) _triangles[currentVert - 1] = 2; //edge case to draw back to original vert in loop
        _triangles[currentVert++] = 0; //centre bottom vert
        _triangles[currentVert++] = i + 2;
       
    }

    //draw a triangle on topface between verts i + 1 and i + 2, and centre point
    private void topFaces(int i)
    {
        _triangles[currentVert++] = i + 1;
        if (i == noOfEdgesOnFace) _triangles[currentVert-1] = (noOfEdgesOnFace * 2) + 1; //edge case to draw back to original vert in loop

        _triangles[currentVert++] = 1; //centre top vert
        _triangles[currentVert++] = i + 2;
        if (i == (noOfEdgesOnFace * 2) - 1) _triangles[currentVert-1] = (noOfEdgesOnFace * 2) + 1; //edge case to draw back to original vert in loop

    }

    //draws tris on body of cylinder that has two point on bottom loop of verts and one on top loop
    private void BodyOfCylinderPass1(int i, int iTopLoop)
    {
        _triangles[currentVert++] = i + 2;
        _triangles[currentVert++] = iTopLoop;
        _triangles[currentVert++] = i + 3;
        if (i == noOfEdgesOnFace - 1) _triangles[currentVert-1] = 2; //edge case to draw back to original vert in loop



    }
    //draws tris on body of cylinder that has two point on top loop of verts and one on bottom
    private void BodyOfCylinderPass2(int i, int iTopLoop)
    {
        _triangles[currentVert++] = i + 3;
        if (i + 3 == (noOfEdgesOnFace + 2)) _triangles[currentVert-1] = 2; //edge case to draw back to original bottom vert in loop
        _triangles[currentVert++] = iTopLoop;
        _triangles[currentVert++] = iTopLoop + 1;
        if (iTopLoop == (noOfEdgesOnFace * 2) + 1) _triangles[currentVert-1] = noOfEdgesOnFace + 2; //edge case to draw back to original top vert in loop

    }


    //private void OnDrawGizmos()
    //{
    //    if (_vertices != null)
    //    {
    //        Gizmos.color = Color.black;
    //        foreach (Vector3 vec3 in _vertices)
    //        {
    //            Gizmos.DrawSphere(vec3, 0.1f);
    //        }
    //    }
    //}

}