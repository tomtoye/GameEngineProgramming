using UnityEngine;

//what direction we draw corridor
public enum Direction
{
    North, East, South, West,
}

public class CorridorMaker
{
    //start x,y coor
    public int startXPos, startYPos;
    //length of corridoor
    public int corridorLength; 
    //what way to draw           
    public Direction direction;


    //where it gonna end X
    public int EndPositionX
    {
        get
        {
            if (direction == Direction.North || direction == Direction.South) return startXPos; //north south dont matter for x

            if (direction == Direction.East) return startXPos + corridorLength - 1; //east
            else return startXPos - corridorLength + 1; //west

            
        }
    }

    //where it gonna end Y
    public int EndPositionY
    {
        get
        {
            if (direction == Direction.East || direction == Direction.West) return startYPos; //east west dont matter for Y

            if (direction == Direction.North) return startYPos + corridorLength - 1; //north
            else return startYPos - corridorLength + 1; // south  
        }
    }


    public void SetupCorridor(RoomMaker room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
    {
        //choose rand direction to draw coridor upon
        direction = (Direction)Random.Range(0, 4);

        //get rand length
        corridorLength = length.Random;

        //length max
        int maxLength = length.m_Max -1;

        switch (direction)
        {

            //below we will look at what direction we want to head then
            //we choose a position on side (north, east ect) of wall that is random to spawn corridor from
            //so if case north choose random spot from top wall to draw from
            //now set our max length so we dont reach edge of board when drawing it

            case Direction.North:
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - 1);
                startYPos = room.yPos + room.roomHeight;
                maxLength = rows - startYPos - roomHeight.m_Min;
                break;
            case Direction.East:
                startXPos = room.xPos + room.roomWidth;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight - 1);
                maxLength = columns - startXPos - roomWidth.m_Min;
                break;
            case Direction.South:
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth);
                startYPos = room.yPos;
                maxLength = startYPos - roomHeight.m_Min;
                break;
            case Direction.West:
                startXPos = room.xPos;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight);
                maxLength = startXPos - roomWidth.m_Min;
                break;
        }

        //make sure our length is not over our max possible length
        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);
    }
}