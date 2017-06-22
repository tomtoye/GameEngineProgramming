using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding1 : MonoBehaviour
{



    public GameObject startTile;
    public GameObject endTile;
    public GameObject pathTile;

    private Quaternion flatTile = Quaternion.Euler(90f, 0f, 0f);

    //position to spawn pathfinding tiles slighty above mazetiles
    private Vector3 aboveMaze = new Vector3(0f, 0.1f, 0f);


    private bool blnStartTile;

    private GameObject pathHolder; //store all pathTiles in here


    private TileVec2 startPos;

    MazeManager MazeGrid;

    void Awake()
    {
        MazeGrid = GetComponent<MazeManager>();

        resetPathHolder();


    }

    // Update is called once per frame
    void Update()
    {

        //place our start tile at location we hit with raycast
        if (Input.GetButtonDown("Fire1")) setStartEndTiles(true);
        //place our end tile at location we hit with raycast
        if (Input.GetButtonDown("Fire2")) setStartEndTiles(false);

    }

    void resetPathHolder()
    {
        //if a pathholder already exists delete it
        if (GameObject.Find("pathHolder") != null)
        {
            if (Application.isEditor) DestroyImmediate(pathHolder);

            else Destroy(pathHolder);
        }
        // Create new  mazeHolder.
        pathHolder = new GameObject("pathHolder");
    }


    void setStartEndTiles(bool start)
    {

        //do a raycast to see where user clicked on screen and if it hit a tile store position and place start/end tile at that positiom
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            //if left click to place start tile
            if (start)
            {
                GameObject tile = Instantiate(startTile, hit.transform.position + aboveMaze, flatTile) as GameObject;
                tile.transform.parent = pathHolder.transform;
                tile.name = "startTile";

                //set our starttile boolean to true so we know we have a start tile in the world
                blnStartTile = true;
                startPos = new TileVec2(true, (int)hit.transform.position.x, (int)hit.transform.position.z);
            }
            //if right click to place end tile
            else
            {
                //if start tile exists, go to place the end tile
                if (blnStartTile)
                {

                    GameObject tile = Instantiate(endTile, hit.transform.position + aboveMaze, flatTile) as GameObject;
                    tile.name = "endTile";
                    tile.transform.parent = pathHolder.transform;
                    //set our endtile boolean to true so we know we have a end tile in the world
                    //blnEndTile = true;


                    TileVec2 endPos = new TileVec2(true, (int)hit.transform.position.x, (int)hit.transform.position.z);




                    //now we have a start/end tile, go find path between them
                    FindPath(startPos, endPos);
                }
            }
        }

    }


    //function called to find path, give start and end position of path to be found
    void FindPath(TileVec2 StartPos, TileVec2 EndPos)
    {

        List<TileVec2> openSet = new List<TileVec2>(); //all possible positions worth searching.
        HashSet<TileVec2> closedSet = new HashSet<TileVec2>(); //closed set of evaluated and discarded positions

        openSet.Add(startPos); //add start pos as first tile in open set

        //whilst we still have values in openset
        while (openSet.Count > 0)
        {
            TileVec2 currentTile = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i]; //current tile is equal to next best candidate in list
                }
            }

            openSet.Remove(currentTile); //remove tile we currently evaluating
            closedSet.Add(currentTile); //move it to closed set

            //if we at end position
            if (currentTile.xPos == EndPos.xPos)
            {
                if (currentTile.yPos == EndPos.yPos)
                {
                    //go draw path to show user
                    RetracePath(startPos, currentTile);
                    return;
                }

            }

            //go look at our neighbours and calculate their costs and add to open list.
            foreach (TileVec2 neighbour in MazeGrid.GetNeighbours(currentTile))
            {
                //unless they out bounds or already in closed list
                if (!neighbour.isFloor || closedSet.Contains(neighbour)) continue;

                int newMovCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);

                if (newMovCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, EndPos);

                    neighbour.parent = currentTile;

                    if (!openSet.Contains(neighbour)) openSet.Add(neighbour);

                }

            }



        }





    }

    //function used to calculate distance between tiles

    //we use a value of 10 from tile to tile vertically and horizontally, then a value of 14 fo diagonal movements due to pythagoras 1^2 + 1^2 = sqrt(2) or 1.4ish
    //but use ints and scale to 10 and 14, as ints cheaper then floats

    int GetDistance(TileVec2 tileA, TileVec2 tileB)
    {
        int xDist = Mathf.Abs(tileA.xPos - tileB.xPos);
        int yDist = Mathf.Abs(tileA.yPos - tileB.yPos);

        if (xDist > yDist) return 14 * yDist + 10 * (xDist - yDist);
        return 14 * xDist + 10 * (yDist - xDist);


    }

    //retrace path
    //keep looking at next tiles parent until we back at start tile
    //along the way instaniate a tile to show user calculated path
    void RetracePath(TileVec2 startNode, TileVec2 currentTile)
    {
        resetPathHolder();
        while (currentTile != startNode)
        {
            GameObject tile = Instantiate(pathTile, new Vector3(currentTile.xPos, 0.05f, currentTile.yPos), flatTile) as GameObject;
            tile.transform.parent = pathHolder.transform;

            currentTile = currentTile.parent;
        }
    }





}
