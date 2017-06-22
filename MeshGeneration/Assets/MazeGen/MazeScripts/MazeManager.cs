using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MazeManager : MonoBehaviour
{



    public Vector2 gridWorldSize;


    TileVec2[,] MazeGrid;

    //number of columns rows we will have in our maze (width and height respectivley)
    public int columns = 100, rows = 100;



    //for each of following give two ints for a range of how many or the width/length of each thing we want
    //from there we can use random num from range to generate unique mazes each time

    public int noRoomsMin = 10, noRoomsMax = 20;
    public IntRange numRooms;

    public int roomWidthMin = 4, roomWidthMax = 8;
    public IntRange roomWidth;

    public int roomHeightMin = 4, roomHeightMax = 8;
    public IntRange roomHeight;

    public int corLengthMin = 2, corLengthMax = 10;
    public IntRange corridorLength;

    //the prefab we will use for all the floors and walls
    public GameObject tilePrefab;

    //array of each tile in maze, if true then we want to place a tile at this position
    public bool[,] FloorTiles;

    //TileVector2[,] MazeGrid;


    //array of our rooms and corridors
    private RoomMaker[] rooms;  //array of our rooms and corridors
    private CorridorMaker[] corridors;                             
    private GameObject mazeHolder; //gonna put instatiated tiles in here to clean up hierarchy, makes deleting easier too

   // public bool mazeGenerated = false;


    //look four our gen button on cavas and add listener, also first time set up just gen a maze with preset values
    private void Start()
    {
        Button Gen = GameObject.Find("GenMaze").GetComponent<Button>();
        Gen.onClick.AddListener(Generate);
        Generate();
        
    }

    public void Generate()
    {
        if (GameObject.Find("MazeCanvas(Clone)"))
        {
            //rows and columns of maze grid
            int tempRows = (int)GameObject.Find("MazeRowsSlider").GetComponent<Slider>().value;
            int tempColumns = (int)GameObject.Find("MazeColumnsSlider").GetComponent<Slider>().value;

            if (rows != tempRows) { rows = tempRows; }
            if (columns != tempColumns) { columns = tempColumns; }
            
            //min max number of rooms
            int tempRoomMax = (int)GameObject.Find("MazeRoomMaxSlider").GetComponent<Slider>().value;
            int tempRoomMin = (int)GameObject.Find("MazeRoomMinSlider").GetComponent<Slider>().value;
        
            if (noRoomsMax != tempRoomMax) { noRoomsMax = tempRoomMax; }
            if (noRoomsMin != tempRoomMin) { noRoomsMin = tempRoomMin; }

            //min max of rooms widths
            int tempWidthMax = (int)GameObject.Find("MazeRoomWidthMaxSlider").GetComponent<Slider>().value;
            int tempWidthMin = (int)GameObject.Find("MazeRoomWidthMinSlider").GetComponent<Slider>().value;
        
            if (roomWidthMax != tempWidthMax) { roomWidthMax = tempWidthMax; }
            if (roomWidthMin != tempWidthMin) { roomWidthMin = tempWidthMin; }

            //min max of rooms heights
            int tempHeightMax = (int)GameObject.Find("MazeRoomHeightMaxSlider").GetComponent<Slider>().value;
            int tempHeightMin = (int)GameObject.Find("MazeRoomHeightMinSlider").GetComponent<Slider>().value;
        
            if (roomHeightMax != tempHeightMax) { roomHeightMax = tempHeightMax; }
            if (roomHeightMin != tempHeightMin) { roomHeightMin = tempHeightMin; }


            //min max length of corridors
            int tempCorMax = (int)GameObject.Find("MazeCorMaxSlider").GetComponent<Slider>().value;
            int tempCorMin = (int)GameObject.Find("MazeCorMinSlider").GetComponent<Slider>().value;
            
            if (corLengthMax != tempCorMax) { corLengthMax = tempCorMax; }
            if (corLengthMin != tempCorMin) { corLengthMin = tempCorMin; }


        }
        //set min max number of rooms
        numRooms = new IntRange(noRoomsMin, noRoomsMax);
        //set min max of rooms widths
        roomWidth = new IntRange(roomWidthMin, roomWidthMax);
        //set min max of rooms heights
        roomHeight = new IntRange(roomHeightMin, roomHeightMax);
        //set min max length of corridors
        corridorLength = new IntRange(corLengthMin, corLengthMax);


        //set container for maze tiles
        setMazeHolder();
        

        //these are kinda self explanatory

        CreateRoomsAndCorridors();

        SetTilesValuesForRooms();

        SetTilesValuesForCorridors();


        CreateGrid();

        InstantiateTiles(); //from bool floortile array


    }





    void CreateRoomsAndCorridors()
    {
        //change our tile array to right size
        FloorTiles = new bool[columns, rows];
        //MazeGrid = new TileVector2[columns, rows];
        //create the rooms array with random size between upper and lower bound
        rooms = new RoomMaker[numRooms.Random];

        //one less corridor than there is rooms
        //as we go room , corridoor, room, (we both start and finish with making a room so we want one less corridoor)
        corridors = new CorridorMaker[rooms.Length - 1];

        //create 1st room corridor
        rooms[0] = new RoomMaker();
        corridors[0] = new CorridorMaker();

        //setup the first room (no corridors exist yet)
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        //make first corridor using room above
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        //for each room we gotta make, excluding first (start at in i = 1)
        for (int i = 1; i < rooms.Length; i++)
        {
            //make room
            rooms[i] = new RoomMaker();
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            //make a corridor if we not at end (dont want corridor leading to nothing
            if (i < corridors.Length)
            {
                corridors[i] = new CorridorMaker();
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }
        }

    }

    //check that floortile array is set to true for where we want rooms
    void SetTilesValuesForRooms()
    {
        //for each room
        for (int i = 0; i < rooms.Length; i++)
        {
            RoomMaker currentRoom = rooms[i];
            //go through each tile in room vertically
            for (int y = 0; y < currentRoom.roomHeight; y++)
            {
                int yCoord = currentRoom.yPos + y;

                //go through each tile in room horizontally
                for (int x = 0; x < currentRoom.roomWidth; x++)
                {
                    //set postion to true in floortile array
                    FloorTiles[currentRoom.xPos + x, yCoord] = true;
                    //MazeGrid[currentRoom.xPos + x, yCoord] = new TileVector2(currentRoom.xPos + x, yCoord, true);
                }
            }
        }
    }

    //this will do basically same as above just check each coridor val
    void SetTilesValuesForCorridors()
    {
        //for each corridor
        for (int i = 0; i < corridors.Length; i++)
        {
            CorridorMaker currentCorridor = corridors[i];

                ////start looking from start pos of this coridor
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                
                //check we increment along correct axis to check each corridor position
                //when moving along corridor set positon to true, so we can place floor tile later
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        for (int j = 0; j < currentCorridor.corridorLength; j++)
                        {
                        FloorTiles[xCoord, yCoord] = true;
                        
                        yCoord += 1;
                        }   
                        break;

                    case Direction.East:
                    for (int j = 0; j < currentCorridor.corridorLength; j++)
                    {
                        FloorTiles[xCoord, yCoord] = true;
                        
                        xCoord += 1;
                  
                    }
                    break;
                    case Direction.South:
                    for (int j = 0; j < currentCorridor.corridorLength; j++)
                    {
                        FloorTiles[xCoord, yCoord] = true;
                       
                        yCoord -= 1;
                    }
                    break;
                    case Direction.West:
                    for (int j = 0; j < currentCorridor.corridorLength; j++)
                    {
                        FloorTiles[xCoord, yCoord] = true;
                       
                        xCoord -= 1;   
                    }
                    break;
                }            
        }
    }

    void InstantiateTiles()
    {
        //bunch of quaterions to be used to set tiles where we need them
        //eg if we want a floor tile instatiate a tile with flatRot
        Quaternion flatRot = Quaternion.Euler(90, 0, 0);
        Quaternion northRot = Quaternion.Euler(0, 0, 0);
        Quaternion eastRot = Quaternion.Euler(0, 90, 0);
        Quaternion southRot = Quaternion.Euler(180, 0, 0);
        Quaternion westRot = Quaternion.Euler(0, 270, 0);

        //optimise if statement in loop
        int columnSearch = columns - 1;
        int rowSearch = rows - 1;

        //temp position holder
        Vector3 position = new Vector3(0f, 0f, 0f);

        //when we place a wall, we want it half away from center point so it lines up with edge of tile
        float wallVertPos = 0.5f;


        GameObject tile;

        // Go through all the tiles in array
        for (int y = 0; y < rows; y++)
        {
            
            position.z = y; //it says z = y but its right, just everyone seems to think z and y should be other way to one another :/

            //small bit of optimisation, do here instead of in every loop of x
            int tempPlusy = y + 1;
            int tempMinusy = y - 1;
            
            for (int x = 0; x < columns; x++)
            {
                //if we are at a floor tile
                
                if (FloorTiles[x , y] == true)
                {
                    position.x = x;

                    //check each side (north, east, west, south to see if we should place a wall in that direction

                    //check west side is the best side
                    //if we are on far left of array we need a wall or if we dont have a floortile to west of us we need to put a wall tile
                    //the next few if statements are same but checking each other side, so i wont repeat self
                    if (x == 0 || FloorTiles[x - 1, y] != true)
                    {
                        tile = Instantiate(tilePrefab, new Vector3(position.x - 0.5f, wallVertPos, position.z), westRot) as GameObject;
                        tile.transform.parent = mazeHolder.transform;
                    }
                    //check south
                    if (y == 0 || FloorTiles[x, tempMinusy] != true)
                    {
                        tile = Instantiate(tilePrefab, new Vector3(position.x, wallVertPos, position.z - 0.5f), southRot) as GameObject;
                        tile.transform.parent = mazeHolder.transform;
                    }
                    //check east
                    if (x == columnSearch || FloorTiles[x + 1, y] != true)
                    {
                        tile = Instantiate(tilePrefab, new Vector3(position.x + 0.5f, wallVertPos, position.z), eastRot) as GameObject;
                        tile.transform.parent = mazeHolder.transform;
                    }
                    //check north
                    if (y == rowSearch || FloorTiles[x , tempPlusy] != true)
                    {
                        tile = Instantiate(tilePrefab, new Vector3(position.x, wallVertPos, position.z + 0.5f), northRot) as GameObject;
                        tile.transform.parent = mazeHolder.transform;
                    }

                    //finally place a floor tile on floor now that we checked all of its sides...
                    tile = Instantiate(tilePrefab, position, flatRot) as GameObject;
                    tile.transform.parent = mazeHolder.transform;

                }
      
          
            }
        }
    }




    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(columns, 1, rows));
    //
    //    if (MazeGrid != null)
    //    {
    //        foreach (TileVec2 n in MazeGrid)
    //        {
    //            Gizmos.color = (n.isFloor) ? Color.white : Color.red;
    //            Vector3 worldPos = new Vector3(n.xPos, 0, n.yPos);
    //            Gizmos.DrawCube(worldPos, Vector3.one * 0.5f);
    //        }
    //    }
    //
    //}

    //this will be used for pathfinding
    void CreateGrid()
    {
        MazeGrid = new TileVec2[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //check each pos in maze
                bool floor = FloorTiles[x, y];
                //set grid to position and see if floor tile there
                MazeGrid[x,y] = new TileVec2(floor, x, y);
            }
        }

    }


    //check all tiles around given tile
    public List<TileVec2> GetNeighbours(TileVec2 Tile)
    {
        List<TileVec2> neighbours = new List<TileVec2>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; //dont check self

                int xCheck = Tile.xPos + x;
                int yCheck = Tile.yPos + y;
                //check we not out of bounds of grid
                if (xCheck >= 0 && xCheck < columns && yCheck >= 0 && yCheck < rows)
                {
                    neighbours.Add(MazeGrid[xCheck, yCheck]);
                }

            }

          
        }
        return neighbours;
    }


    void setMazeHolder()
    {
        //if a maze already exists delete it
        if (GameObject.Find("mazeHolder") != null)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(mazeHolder);
               
            }
            else Destroy(mazeHolder);
        }

        //create new mazeHolder
        mazeHolder = new GameObject("mazeHolder");
    }



}