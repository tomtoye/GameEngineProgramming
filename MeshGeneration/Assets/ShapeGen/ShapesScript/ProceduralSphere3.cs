using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]


public class ProceduralSphere3 : MonoBehaviour
{


    private Vector3[] _vertices; //holds all our verts of sphere

    private int[] _triangles; //holds all the tri data

    Mesh _mesh; //acces to our mesh on our gameobject


    //these 3 values could be melded into our variable called radius, but like this we can generate more varied shapes
    public float height = 1; //heightofsphere
    public float width = 1; //widthofsphere
    public float depth = 1;  //depthofsphere

    //no of loops vertically and horizontally we want on sphere, aka latitudinal and longitudinal lines
    //again these two values are seperate for more control/variation
    public int yLoops = 10;
    public int xLoops = 10;

    int currentVert = 0; //counter for each vert we draw in shape


    public Canvas ShapeCanvas;


    // Use this for initialization
    void Start()
    {
        Generate();
    }

    //quick update function added to show off shape in scene
    void Update()
    {
        //here im just checking to see if any of the slider values have changed to update the mesh
        if (GameObject.Find("ShapesCanvas(Clone)"))
        {
            float tempheight = GameObject.Find("sphereHeightSlider").GetComponent<Slider>().value;
            float tempwidth = GameObject.Find("sphereWidthSlider").GetComponent<Slider>().value;
            float tempdepth = GameObject.Find("sphereDepthSlider").GetComponent<Slider>().value;
            int tempxLoops = (int)GameObject.Find("sphereXLoopsSlider").GetComponent<Slider>().value;
            int tempyLoops = (int)GameObject.Find("sphereYLoopsSlider").GetComponent<Slider>().value;

            if (height != tempheight) { height = tempheight; Generate(); }
            else if (width != tempwidth) { width = tempwidth;     Generate(); }
            else if (depth != tempdepth) {depth = tempdepth;      Generate(); }
            else if (xLoops != tempxLoops){xLoops = tempxLoops;   Generate(); }
            else if (yLoops != tempyLoops)  yLoops = tempyLoops;  Generate(); }

           
    }


    public void Generate()
    {

        
        currentVert = 0; //reset to 0 for if we recreate mesh

        //verts for each one on end face then an extra 2 for centre points on each end of sphere (top and bottom) 
        int totalVertices = (yLoops * xLoops) + 2;
        _vertices = new Vector3[totalVertices]; //set our vert array to correct amount

        //we need 3 ints in our triangles array for each tri we wanna draw
        //each int relates to a different vert, tri is drawn between each vert


        //we need (horizontal loop x vertical loop) x2 to make a square out of 2 tri's
        //then x3 as each tri needs 3 ints as explained above
        //the top and bottom loops will be drawing to a single point
        //so we just need triangles not squares, the above value accounts for that
        int totalindices = yLoops * xLoops * 2 * 3; 
        


        _triangles = new int[totalindices]; //set our tri array to correct amount

        MeshFilter _meshfilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _meshfilter.mesh = _mesh;

        CreateVertices(); //MapEachVert

       

        //cap tris (top and bottom of cylinder)
        CapTris();
        //Middle tris (or quads i guess, since we draw t2 tris together? middle part of sphere)
        MiddleLoopTris();


        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.RecalculateNormals();

    }

    //where verts are arranged
    private void CreateVertices()
    {
        //do top and bottom point first
        _vertices[(yLoops * xLoops) + 1] = new Vector3(0, -height, 0); //botcentre point (last in array)
     
        _vertices[0] = new Vector3(0, height, 0); //topcentre point

        //each vector3 variable
        float x, y, z;

        int vert = 0;
        float fPI = Mathf.PI; // PI (360 degrees)
        float f2PI = 2 * fPI; // Pi/2 (180 degrees)

        for (int i = 0; i < yLoops; i++)
        {

            //+1's because we have already set up top and bottom verts
            float theta = fPI * (i + 1) / (yLoops + 1);

            //y is only going to change each time we move up a row on sphere so can do in outer loop
            y = height * (Mathf.Cos(theta));

            for (int j = 0; j < xLoops; j++)
            {
                vert++;
                //find next x,z coord along
                //having a seperate height and radius allows us to have more varied shapes 
                //e.g rugby ball style spheres
                float phi = f2PI * j / xLoops;

                x = width * (Mathf.Sin(theta) * Mathf.Cos(phi));
                z = depth * (Mathf.Sin(theta) * Mathf.Sin(phi));

                //place vert
                _vertices[vert] = new Vector3(x, y, z);

            }
        }
    }

    private void MiddleLoopTris()
    {
        for (int i = 0; i < yLoops - 1; i++)
        {
            //It just works - Todd Howard (2015).
            //I expected to have some edge cases here for when we reached the end of the loop
            //but it just works
            //you will see that one of the sphere segments (like an orange segment) 
            //Has the quads draw with diagonal lines the opposite way.
            //This was where i expected it to mess up (draw facing inwards ect
            //but turned out fine ) of course for those who are picky may want to change this
            //But it will not have any real impact.

            for (int k = 0; k < xLoops; k++)
            {
                int tempi = k + i * (xLoops) + 1;
                int tempinextloop = tempi + xLoops - 1;

                //need 2 tris to make square in ring

                //triangles with 2 verts left and one up top right
                _triangles[currentVert++] = tempi;
                _triangles[currentVert++] = tempi + 1;
                _triangles[currentVert++] = tempinextloop + 1;
                //traingles with 2 verts right and one left bottom
                _triangles[currentVert++] = tempi;
                _triangles[currentVert++] = tempinextloop + 1;
                _triangles[currentVert++] = tempinextloop;
            }

        }
    }

    private void CapTris()
    {
        for (int i = 0; i < xLoops; i++)
        {
            //top cap
            _triangles[currentVert++] = i + 2;

            //edge case for when we reach end of loop.
            //we want to draw back to first vert we drew to in loop
            if (i == xLoops - 1) _triangles[currentVert - 1] = 1;

            _triangles[currentVert++] = i + 1;
            _triangles[currentVert++] = 0; //top center vert

            //Bottom Cap

            //bottom centre vert
            _triangles[currentVert++] = _vertices.Length - 1;
            _triangles[currentVert++] = _vertices.Length - (i + 2) - 1;

            //edge case for when we reach end of loop
            //we want to draw back to first vert in bot cap
            if (i == xLoops - 1) _triangles[currentVert - 1] = _vertices.Length - 2; 

            _triangles[currentVert++] = _vertices.Length - (i + 1) - 1;

        }
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