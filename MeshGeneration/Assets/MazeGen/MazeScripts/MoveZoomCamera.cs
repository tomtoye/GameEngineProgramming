
//movement script for maze, using arrow keys to move and scroll to zoom

using UnityEngine;

public class MoveZoomCamera : MonoBehaviour {

    float camScroll = 1f;
    public int scrollMultiplier = 20;
    public int camMovement = 1;


    Vector3 newCamPosition;
    
    // Update is called once per frame
    void Update()
    {
        camScroll = Input.GetAxis("Mouse ScrollWheel");


        newCamPosition = Camera.main.transform.position;
        
        if (Input.GetKey("left"))
        {
            newCamPosition.x -= camMovement;
        }
        
        if (Input.GetKey("right"))
        {
            newCamPosition.x += camMovement;
        }
        
        if (Input.GetKey("up"))
        {
            newCamPosition.z += camMovement;
        }
        
        if (Input.GetKey("down"))
        {
            newCamPosition.z -= camMovement;
        }

        Camera.main.orthographicSize -= camScroll * scrollMultiplier;

        if (Camera.main.orthographicSize < 1f)
        {
            Camera.main.orthographicSize = 1f;
        }

        Camera.main.transform.position = newCamPosition;



    }



}

