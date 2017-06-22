//==================================================================================================================
//state for terrain, handles loading all we need in, updates nessecary functions and destroys when we leave
//==================================================================================================================

using UnityEngine;

public class TerrainState : State
{

    GameObject Terrain; //will be our terrain gen prefab

    GameObject TerrainCanvas; // will be terrain canvas and hae our nessecary sliders ect.

    //public GameStateManager stateManager;



    
    public TerrainState(string a_stringName) : base(a_stringName)
    {
        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {

        //set camera position and rotation to view terrain
        Camera.main.transform.position = new Vector3(60f, 38f, -40f);
        Camera.main.transform.rotation = Quaternion.Euler(20f, 0f, 0f);

        //load in terrain
        Terrain = Resources.Load("Prefabs/ProceduralTerrain", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(Terrain);

        //load in terrain canvas
        TerrainCanvas = Resources.Load("Prefabs/TerrainCanvas", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(TerrainCanvas);

        //Move to update function
        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        //dont need to update anything here so just keep checking for when we want to leave
        if (this != GameStateManager.Instance.CurrentState) Process = Leave;

    }

    protected override void Leave(float a_fTimeStep)
    {
        //destroy the canvas and terrain
        if (GameObject.Find("ProceduralTerrain(Clone)")) GameObject.Destroy(GameObject.Find("ProceduralTerrain(Clone)"));

        if (GameObject.Find("TerrainCanvas(Clone)")) GameObject.Destroy(GameObject.Find("TerrainCanvas(Clone)"));
        

        if (this == GameStateManager.Instance.CurrentState) Process = Initialise;
    


    }






}