using UnityEngine;
using System.Collections;

//simple class uswed to hold x and y pos of each tiles in array and in world (as each tile is perfectly 1 by 1 we dont need a vector3 for each tile position, just used this as kind of intVector2



public class TileVec2
{
    public bool isFloor; //is this tile pos a floor tile?
    public int xPos;
    public int yPos;

    public int gCost; //used for A* pathfinding
    public int hCost; //used for A* pathfinding

    public TileVec2 parent; //used to trace back tiles to find path

    public TileVec2(bool _isFloor, int _x, int _y)
    {
        isFloor = _isFloor;
        xPos = _x;
        yPos = _y;

    }

    //get fcost (we dont need to set as it is calculated by the sum of other costs)
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }




}
