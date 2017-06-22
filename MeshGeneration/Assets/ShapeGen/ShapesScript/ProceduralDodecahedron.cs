using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; //need this for list
using UnityEngine.UI;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralDodecahedron : MonoBehaviour
{


    private Vector3[] _vertices; //array to hold all the verts of our shape


    private int[] _triangles; //array to hold all the tris of our shape

    Mesh _mesh; //the mesh we gonna create

    
    public float dodecScale = 1f;   //dodecahedron vert scale multiplier, this will make the base shape bigger or smaller
    public float isoStellation = 1f;    //isosahedron vert scale multiplier, this will make the shapes pointy star bits bigger or smaller, this can even go under 1 to make the points go inward

    //these are just to make code look clearer, less magic numbers
    private int isosStartPoint = 20; //this is where we start storing verts of the isosahedron points in the vert array
    private int totalVerts = 32; //the end of our vert array

    Material Shapes;
    // Use this for initialization
    void Start()
    {
        Generate();
    }

    void Update()
    {
        if (GameObject.Find("ShapesCanvas(Clone)"))
        {
            float tempSize = GameObject.Find("dodecSizeSlider").GetComponent<Slider>().value;
            float tempStell = GameObject.Find("dodecStellateSlider").GetComponent<Slider>().value;

            if (dodecScale != tempSize) { dodecScale = tempSize; Generate(); }
            if (isoStellation != tempStell) { isoStellation = tempStell; Generate(); }
        }
    }




    public void Generate()
    {
        _vertices = new Vector3[totalVerts]; //set array to correct length //is 32 as dodecahedron has 20 points and isosohedron has 12 and a stellated dodecahedron is both combined kinda...

        int totalindices = 360; //3 * 5 * 12 * 2 = 360 || we need 120 triangles in total because a stellated dodecahedron has 60 faces but for our object we need to draw both internal and external faces
                                //this is because determining which faces are drawing outwards is more effort then what its worth so we will have both, anyway for each tri we need 3 ints in the array so 3*120 = 360
        _triangles = new int[totalindices]; //set array to correct length


        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;

    
        

        //MapEachVert
        CreateVertices(); //here we calculate where each vert should be placed
        _mesh.vertices = _vertices;

        DrawTriangles();//here we draw each tri between our verts
        
        ManipulateVerts();//here we manipulate either isos or dodec verts in our array by our scale factors determined by user
        
        //finally we give our mesh all the verts and the tris we calculated
        _mesh.vertices = _vertices;
        
        _mesh.triangles = _triangles;
        //_mesh.RecalculateNormals();
    }



    private void DrawTriangles()
    {

        //from pervious testing we know that 0,8,20 is a triangle we draw, where 0 and 8 are neighbours and 20 is a Isosahedron point
        //so here we can calculate the neighbour length value and iso to child length values (this is where our isosohedron has 5 kids, the pentagon nearest to it)
        float childDist = Vector3.Distance(_vertices[20], _vertices[0]);
        float neighbourDist = Vector3.Distance(_vertices[0], _vertices[8]);


        //counter for all points to be added to _triangle array
        int currentTriangle = 0;

        //for each Isosahedron vert
        for (int i = isosStartPoint; i < totalVerts; i++)
        {
            List<int> isoKids = new List<int>(5); //list all the dodec verts we want to draw triangles too (the "pentagon" of verts that we are nearest to)
            //look through each dodec vert
            for (int j = 0; j < isosStartPoint; j++)
            {
                //calculate distance from the isos vert to the dodec vert we are looking at
                float dist = Vector3.Distance(_vertices[i], _vertices[j]);
                
                //if the dodec vert is close enough
                if (dist == childDist)
                {
                    //add the vert to our list
                    //well its an int and an int list but you get the idea...
                    //the vert that is in the array position j...
                    isoKids.Add(j);
                }
            }
            

            //for each of the isos kids
            foreach (int _vert in isoKids)
            {
                //look at all other dodec verts 
                for (int k = 0; k < isosStartPoint; k++)
                {
                    //calculate the distance between this dodec vert and the other dodec vert we are looking at
                    float dist = Vector3.Distance(_vertices[_vert], _vertices[k]);
                    //check to see if they are our neighbour
                    if (dist == neighbourDist)
                    {
                        //if so we should check to see that our neighbour is also a child of the current isos vert we are looking at\
                        if (isoKids.Contains(k))
                        {
                            //if all that is true then lets draw a tri between it, ourself and the isos vert
                            _triangles[currentTriangle++] = _vert;
                            _triangles[currentTriangle++] = k;
                            _triangles[currentTriangle++] = i;
                        }
                    }
                }
            }
        }
    }

    //==================================================================================
    //this function is to manipulate all the verts as described above
    //ie do we want big/small shape, big spikes (stellation value) or small or even have spikes go inwards
    private void ManipulateVerts()
    {
        //used to scale only the dodec verts
        Vector3 DoDecScaleVect = new Vector3(dodecScale, dodecScale, dodecScale);

        //used to scale only the isos verts
        Vector3 IsoScaleVect = new Vector3(isoStellation, isoStellation, isoStellation);

        //for each dodec vert apply scale factor
        for (int i = 0; i < isosStartPoint; i++)
        {
            _vertices[i].Scale(DoDecScaleVect);
        }
        //for each isos vert apply scale factor
        for (int i = isosStartPoint; i < totalVerts; i++)
        {
            _vertices[i].Scale(IsoScaleVect);

        }
    }
    //==================================================================================




    //=================================================================================================================
    //Map verts
    private void CreateVertices()
    {
        //https://en.wikipedia.org/wiki/Regular_icosahedron#Cartesian_coordinates
        //that link contains the cartesian coordinates icosahedron which we are using below along with the golden values or h value of both dodec and isoahedron

        //for dodecahedron use cartesian coordinates in below link, but using h value in above link, so they line up correctly and scale together
        //https://en.wikipedia.org/wiki/Regular_dodecahedron#Cartesian_coordinates

        //there may be a way to put all this into a loop but im not smart enough :/

        //======================================================================
        //DODEC verts
        float goldenDecRatio = ((Mathf.Sqrt(5) - 1) / 2f);

        _vertices[0] = new Vector3(1, 1, 1);

        _vertices[1] = new Vector3(-1, -1, -1);

        _vertices[2] = new Vector3(1, -1, -1);

        _vertices[3] = new Vector3(1, 1, -1);


        _vertices[4] = new Vector3(-1, 1, 1);

        _vertices[5] = new Vector3(-1, -1, 1);

        _vertices[6] = new Vector3(-1, 1, -1);

        _vertices[7] = new Vector3(1, -1, 1);


        _vertices[8] = new Vector3(0, (1f / goldenDecRatio), (goldenDecRatio));

        _vertices[9] = new Vector3(0, -(1f / goldenDecRatio), (goldenDecRatio));

        _vertices[10] = new Vector3(0, -(1f / goldenDecRatio), -(goldenDecRatio));

        _vertices[11] = new Vector3(0, (1f / goldenDecRatio), -(goldenDecRatio));


        _vertices[12] = new Vector3((1f / goldenDecRatio), (goldenDecRatio), 0);

        _vertices[13] = new Vector3(-(1f / goldenDecRatio), -(goldenDecRatio), 0);

        _vertices[14] = new Vector3(-(1f / goldenDecRatio), (goldenDecRatio), 0);

        _vertices[15] = new Vector3((1f / goldenDecRatio), -(goldenDecRatio), 0);


        _vertices[16] = new Vector3((goldenDecRatio), 0, (1f / goldenDecRatio));

        _vertices[17] = new Vector3(-(goldenDecRatio), 0, -(1f / goldenDecRatio));

        _vertices[18] = new Vector3(-(goldenDecRatio), 0, (1f / goldenDecRatio));

        _vertices[19] = new Vector3((goldenDecRatio), 0, -(1f / goldenDecRatio));
        //=========================================================================


        
        //=========================================================================
        //ISOS verts
        float goldenIcoRatio = ((1f + Mathf.Sqrt(5)) / 2f);

        _vertices[20] = new Vector3(0, 1, goldenIcoRatio);

        _vertices[21] = new Vector3(0, 1, -goldenIcoRatio);

        _vertices[22] = new Vector3(0, -1, -goldenIcoRatio);

        _vertices[23] = new Vector3(0, -1, goldenIcoRatio);



        _vertices[24] = new Vector3(1, goldenIcoRatio, 0);

        _vertices[25] = new Vector3(1, -goldenIcoRatio, 0);

        _vertices[26] = new Vector3(-1, -goldenIcoRatio, 0);

        _vertices[27] = new Vector3(-1, goldenIcoRatio, 0);




        _vertices[28] = new Vector3(goldenIcoRatio, 0, 1);

        _vertices[29] = new Vector3(-goldenIcoRatio, 0, 1);

        _vertices[30] = new Vector3(goldenIcoRatio, 0, -1);

        _vertices[31] = new Vector3(-goldenIcoRatio, 0, -1);
        //=========================================================================


    }
    //=============================================================================


    //private void OnDrawGizmos()
    //{
    //    if (_vertices != null)
    //    {
    //        Gizmos.color = Color.black;
    //        for (int i = 0; i < 20; i++)
    //        {
    //            Gizmos.DrawSphere(_vertices[i], 0.1f);
    //        }
    //        Gizmos.color = Color.blue;
    //        for (int i = 20; i < 32; i++)
    //        {
    //            Gizmos.DrawSphere(_vertices[i], 0.1f);
    //        }
    //
    //    }
    //}





}