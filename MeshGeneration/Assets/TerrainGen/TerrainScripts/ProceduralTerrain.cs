//===============================================================
//This script is used to create a terrain mesh using perlin noise
//
//with this script we can choose...
//
//the size of the terrain width/height
//the columns/rows of how many verts our mesh has
//the lower and higher bounding heights
//noise scale factor
//octaves - not included in demo scene, quite tricky to get right with terrain height bounds
//lacunarity - affects frequency in each octave
//persistence - affects amplitude in each frequency
//at what heights we can spawn "mountains" and how much there height will scale
//the colour and varying heights of which parts of the terrain will spawn
//the seed value of where the perlin noise value is generated from
//===============================================================



using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralTerrain : MonoBehaviour
{
    //======================
    //grid variables
    public int rows = 100, columns = 100; //how many rows/columns we want in terrain map
    public float widthHeightMap, heightHeightMap; //the width and height (i guess length would be better word :/) of the map
    private Vector3[] _vertices; //array for each vert in grid
    public int iTotalVertices; //total verts that grid contains
    //======================


    //======================
    //perlin variables
    public float scaleNoise = 0.5f; //used to scale level of detail we want from noise array
    public int seedX = 1, seedY =1; //seed numbers to generate new map using new perlin noise array
    public int octaves = 1; //how many times we run through perlinnoise generator for more unique maps that dont look like just plain rolling hills
    public float lacunarity = 1, persistense = 1; //used as part of octaves (for each octave we run through we dont want the same values being made so lacunarity and persistense manipulate the frequencys and amplitudes for each octave)
    //======================


    //======================
    //mountain variables
    public float mountainStartingHeight = 8f; //at what point we want mountain like terrain appearing
    public float mountainTerrainMultiplier = 2f; //how drastically we want mountains height to increase comapared to rest of map
    //======================


    //======================
    //height variables
    public float minTerrainHeight = 0f;
    public float maxTerrainHeight = 10f;
    //======================


    //======================
    //colour struct and array of struct for each color/height
    [System.Serializable]
    public struct TerrainColours
    {
        public float zVal;
        public Color colour;
    }
    //this is an array of the above struct so we can implement each height colour in editor with help of system.serializable
    public TerrainColours[] terrainColourArray;
    //======================


    //======================
    // Use this for initialization
    void Start()
    {
        Generate(); 
    }
    //======================


    void Update()
    {
        if (GameObject.Find("TerrainCanvas(Clone)"))
        {
            int tempRows = (int)GameObject.Find("terrainRowsSlider").GetComponent<Slider>().value;
            int tempColumns = (int)GameObject.Find("terrainColumnsSlider").GetComponent<Slider>().value;
            
            if (rows != tempRows) { rows = tempRows; Generate(); }
            if (columns != tempColumns) { columns = tempColumns; Generate(); }
            
            
            float tempWidth = GameObject.Find("terrainWidthSlider").GetComponent<Slider>().value;
            float tempHeight = GameObject.Find("terrainHeightSlider").GetComponent<Slider>().value;
            
            if (widthHeightMap != tempWidth) { widthHeightMap = tempWidth; Generate(); }
            if (heightHeightMap != tempHeight) { heightHeightMap = tempHeight; Generate(); }
            
            
            float tempNoise = (int)GameObject.Find("terrainNoiseSlider").GetComponent<Slider>().value;
            if (scaleNoise != tempNoise) { scaleNoise = tempNoise; Generate(); }

            int tempSeedX = (int)GameObject.Find("terrainSeedXSlider").GetComponent<Slider>().value;
            int tempSeedY = (int)GameObject.Find("terrainSeedYSlider").GetComponent<Slider>().value;

            if (seedX != tempSeedX) { seedX = tempSeedX; Generate(); }
            if (seedY != tempSeedY) { seedY = tempSeedY; Generate(); }



            float tempLac = GameObject.Find("terrainLacunaritySlider").GetComponent<Slider>().value;
            float tempPer = GameObject.Find("terrainPersistenceSlider").GetComponent<Slider>().value;

            if (lacunarity != tempLac) { lacunarity = tempLac; Generate(); }
            if (persistense != tempPer) { persistense = tempPer; Generate(); }

            float tempMountStart = GameObject.Find("terrainMountainStartSlider").GetComponent<Slider>().value;
            if (mountainStartingHeight != tempMountStart) { mountainStartingHeight = tempMountStart; Generate(); }


            float tempMountScale = GameObject.Find("terrainMountainScaleSlider").GetComponent<Slider>().value;
            if (mountainTerrainMultiplier != tempMountScale) { mountainTerrainMultiplier = tempMountScale; Generate(); }



            float tempMin = GameObject.Find("terrainMinHeightSlider").GetComponent<Slider>().value;
            float tempMax = GameObject.Find("terrainMaxHeightSlider").GetComponent<Slider>().value;

            if (minTerrainHeight != tempMin) { minTerrainHeight = tempMin; Generate(); }
            if (maxTerrainHeight != tempMax) { maxTerrainHeight = tempMax; Generate(); }

        }


    }




    //======================
    //colour struct and array of struct for each color/height
    public void Generate()
    {
        //set total verts to correct amount
        iTotalVertices = (rows) * (columns);
        _vertices = new Vector3[iTotalVertices];

        //get a 2d array of perlin noise, using values passed

        float[,] perlinNoiseMapArray = GenerateNoise(columns, rows, octaves, persistense, lacunarity, scaleNoise, seedX, seedY);

        //get acces to our meshfilter on this object
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        Mesh _mesh = new Mesh();
        _meshFilter.mesh = _mesh;

        //reintialise colour array
        Color[] _colours = new Color[(rows) * (columns)];


        //position in array
        int x = 0, y = 0;
        //for each vert
        for (int i = 0; i < _vertices.Length; ++i)
        {
            //find correlating height in noise array   
            float z = perlinNoiseMapArray[(x) , (y)] * maxTerrainHeight;

            //if height is above value where we want mountains to appear
            if (z > mountainStartingHeight)
            {
                //multiply the height by our mountain modifier
                z = z * mountainTerrainMultiplier;
            }
            //set the vert to position in grid and at set height
            _vertices[i].Set(x * widthHeightMap, z, y * heightHeightMap);
            
            //increment x
            ++x;

            //if we at end of column, set x back to 0 and increment y
            if (x % (columns) == 0)
               {
                   x = 0;
                   ++y;
               }

            //========================
            //colour setting
            //========================
            //for each colour and height we have in terrain colour array
            for ( int j = 0; j < terrainColourArray.Length; j++)
            {
                //if height at current point is <= height in colour array
                if (z <= terrainColourArray[j].zVal)
                {
                    //then draw colour here at this point to that in array
                    _colours[i] = terrainColourArray[j].colour;
                    break;
                }
            }
        }

        _mesh.vertices = _vertices;
        int numIndicies = columns * rows * 6;
        int[] triangles = new int[numIndicies];
        drawTriangles(triangles);
        _mesh.triangles = triangles;
        _mesh.colors = _colours;
        _mesh.RecalculateNormals();
    }
    //======================


    //===========================
    //here we draw triangles, same as our simple grid, except our vertices have being given diff height by terrain gen
    public void drawTriangles(int[] triangles)
    {
        int k = 0;
        //one less cus we drawing triangles imbetween each of the verts
        int temprows = rows -1;
        int tempcolumns = columns -1;

        for (int i = 0; i < temprows; ++i)
        {
            for (int j = 0; j < tempcolumns; ++j)
            {
                int tempi = j + columns * i;
                int tempinextloop = j + columns * (i + 1);

                triangles[k++] = tempi;
                triangles[k++] = tempinextloop;
                triangles[k++] = tempi + 1;

                triangles[k++] = tempinextloop;
                triangles[k++] = tempinextloop + 1;
                triangles[k++] = tempi + 1;
            }

        }
    }
    //===========================


    //===========================
    //Perlinnoise array generator
    public float[,] GenerateNoise(int _columns, int _rows, int octaves, float persistense, float lacunarity, float scale, int seedX, int seedY)
    {
        //dont allow our scale to be negative
        if (scale <= 0) scale = .0001f;
       
        //array to hold the value generated at each position
        float[,] perlinNoiseMapArray = new float[_columns, _rows];

        //for each row (y value)
        for (int y = 0; y < _rows; ++y)
        {
            //for each column (x value)
            for (int x = 0; x < _columns; ++x)
            {

                float amplitude = 1; //this will be affected by persistense and affect height values
                float frequency = 1; //this will be affected by lacunarity and affect x values
                float currentHeight = 0;


                for (int i = 0; i < octaves; ++i) //3 is good amount of octaves from testing
                {
                    //here we create perlin noise for position x,y
                    //seed is for different maps (seperate x and y so we can scroll in just one direction if desired (also if rand was implemented, doing to both will increase possible variations)
                    //scale is for scale... you wane a big or small perlin noise map?
                    //frequency helps increase amount of small detail we see, not just rolling hills
                    float perlinNum = Mathf.PerlinNoise(((x + seedX) / scale) * frequency, ((y + seedY) / scale) * frequency);

                    //our height will be result of how many octaves we go through and amplitude value
                    currentHeight = currentHeight + (perlinNum * amplitude);

                    //increase amplitude and lacunarity for next pass through of octaves
                    amplitude = amplitude * persistense;
                    frequency = frequency * lacunarity;

                }
                //here we make sure our height value is between our max and min height and store in array
               perlinNoiseMapArray[x, y] = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, currentHeight);
            }
        }
        return perlinNoiseMapArray;
    }
    //===========================

}