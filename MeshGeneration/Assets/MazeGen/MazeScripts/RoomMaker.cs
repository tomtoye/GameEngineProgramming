using UnityEngine;

public class RoomMaker
{
    //pos of room
    public int xPos, yPos;
    //width height of room
    public int roomWidth, roomHeight;
    //where corridor leading to this room came from (north east ect)
    public Direction enteringCorridor;


    //1st room in maze
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        //give random width height to room
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        //set position in middle of mazegrid as is first room
        xPos = Mathf.RoundToInt(columns / 2f - (roomWidth / 2f));
        yPos = Mathf.RoundToInt(rows / 2f - (roomHeight / 2f));
    }


    //Same as above but know we need to draw on the end of a corridor from the previous room/corridor
    public void SetupRoom(IntRange widthRange, IntRange heightRange, int columns, int rows, CorridorMaker corridor)
    {
        //where did corridor come from, that we trying to join onto
        enteringCorridor = corridor.direction;

        //give random width height to room
        roomWidth = widthRange.Random;
        roomHeight = heightRange.Random;

        //see where corridor came from and adjust accordingly
        switch (corridor.direction)
        {
     
            //here for each case we are going to do similar things

            //check we are not going to try draw over the size of the mazegrid with clamp
            //this is both for height/width of room depending on direction but also x or y of where room is placed

            //put room at end of the corridor we joining onto
            //however we can randomize where about they connect a bit
            //eg if going north, the room can connect to corridor with any of its x positions (well not very ends of room)
            

            case Direction.North:
                
                roomHeight = Mathf.Clamp(roomHeight, 1, rows - corridor.EndPositionY);

                yPos = corridor.EndPositionY;

                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);

                break;

            case Direction.East:

                roomWidth = Mathf.Clamp(roomWidth, 1, columns - corridor.EndPositionX);

                xPos = corridor.EndPositionX;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);

                break;

            case Direction.South:

                roomHeight = Mathf.Clamp(roomHeight, 1, corridor.EndPositionY);

                yPos = corridor.EndPositionY - roomHeight + 1;

                xPos = Random.Range(corridor.EndPositionX - roomWidth + 1, corridor.EndPositionX);
                xPos = Mathf.Clamp(xPos, 0, columns - roomWidth);

                break;

            case Direction.West:

                roomWidth = Mathf.Clamp(roomWidth, 1, corridor.EndPositionX);

                xPos = corridor.EndPositionX - roomWidth + 1;

                yPos = Random.Range(corridor.EndPositionY - roomHeight + 1, corridor.EndPositionY);
                yPos = Mathf.Clamp(yPos, 0, rows - roomHeight);

                break;
        }
    }
}