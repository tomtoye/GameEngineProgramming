//==================================================================================================================
//state for maze/pathfinding, handles loading all we need in, updates nessecary functions and destroys when we leave
//==================================================================================================================

using UnityEngine;


public class MazePathState : State
{

    public MazePathState(string a_stringName) : base(a_stringName)
    {
        
        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {

        //set camera position and rotation
        Camera.main.transform.position = new Vector3(50f, 100f, 50f);
        Camera.main.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        //also because we want perfect top down perspective switch to orthographic
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 60;
        //loading maze manager (hlods relevant scripts)
        GameObject MazePath = Resources.Load("Prefabs/MazeManager", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(MazePath);
        //bring in correct maze/path canvas
        GameObject MazeCanvas = Resources.Load("Prefabs/MazeCanvas", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(MazeCanvas);


        //done initialsing go to update
        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        //check for when user want to leave
        if (this != GameStateManager.Instance.CurrentState) Process = Leave;
    }

    protected override void Leave(float a_fTimeStep)
    {
        //because this is only scene that uses ortho, set back on exit
        Camera.main.orthographic = false;

        //destroy our holders
        if (GameObject.Find("mazeHolder")) GameObject.Destroy(GameObject.Find("mazeHolder"));
        if (GameObject.Find("pathHolder")) GameObject.Destroy(GameObject.Find("pathHolder"));
        //destroy canvas
        if (GameObject.Find("MazeCanvas(Clone)")) GameObject.Destroy(GameObject.Find("MazeCanvas(Clone)"));
        //destroy our prefab
        if (GameObject.Find("MazeManager(Clone)")) GameObject.Destroy(GameObject.Find("MazeManager(Clone)"));


        if (this == GameStateManager.Instance.CurrentState) Process = Initialise;



    }

}