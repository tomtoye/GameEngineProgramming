using UnityEngine;

public class Loader : MonoBehaviour {

    public GameStateManager stateManager;
    public Updater UpdateController;

    
    void Awake ()
    {
        stateManager = GameStateManager.Create();
        if( stateManager != null)
        {
            //all our project states
            stateManager.RegisterState<ShapeState>("ShapeState");
            stateManager.RegisterState<TerrainState>("TerrainState");
            stateManager.RegisterState<MazePathState>("MazePathState");
        }
        Instantiate(UpdateController);
    }
    //change state gonna linked to canvas buttons, they send corresponding int
    public void ChangeState(int stateName)
    {

        switch(stateName)
        {
            case 0:
                    stateManager.EnterState("ShapeState");
                break;
            case 1:
                stateManager.EnterState("TerrainState");
                break;
            case 2:
                stateManager.EnterState("MazePathState");
                break;
        }

    }





	

}
