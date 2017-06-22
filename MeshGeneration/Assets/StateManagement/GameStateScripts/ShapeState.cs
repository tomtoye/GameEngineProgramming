//==================================================================================================================
//state for terrain, handles loading all we need in, updates nessecary functions and destroys when we leave
//==================================================================================================================

using UnityEngine;


public class ShapeState : State
{

    public GameStateManager stateManager;


    public ShapeState(string a_stringName) : base(a_stringName)
    {
        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        //set camera position and rotation
        Camera.main.transform.position = new Vector3(0f, 4.2f, 12.6f);
        Camera.main.transform.rotation = Quaternion.Euler(20, 180, 0f);

        //bring shape prefabs in
        GameObject Sphere = Resources.Load("Prefabs/ProceduralSphere", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(Sphere);

        GameObject StelDodec = Resources.Load("Prefabs/ProceduralDodecahedron", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(StelDodec);

        GameObject Cylinder = Resources.Load("Prefabs/ProceduralCylinder", typeof(GameObject)) as GameObject;
        GameObject.Instantiate(Cylinder);

        //load shape canvas
        GameObject ShapesCanvas = Resources.Load("Prefabs/ShapesCanvas", typeof(GameObject)) as GameObject;
       
        GameObject.Instantiate(ShapesCanvas);

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
        //destroy shapes
        if (GameObject.Find("ProceduralSphere(Clone)")) GameObject.Destroy(GameObject.Find("ProceduralSphere(Clone)"));
        if (GameObject.Find("ProceduralDodecahedron(Clone)")) GameObject.Destroy(GameObject.Find("ProceduralDodecahedron(Clone)"));
        if (GameObject.Find("ProceduralCylinder(Clone)")) GameObject.Destroy(GameObject.Find("ProceduralCylinder(Clone)"));

        //destroy canvas
        if (GameObject.Find("ShapesCanvas(Clone)")) GameObject.Destroy(GameObject.Find("ShapesCanvas(Clone)"));


        if (this == GameStateManager.Instance.CurrentState) Process = Initialise;
    }




    //old way i was going to create objects without using prefab

   // private void CreateSphere()
   // {
   //     Sphere = new GameObject();
   //     Sphere.name = "Sphere";
   //     Sphere.transform.parent = ShapesContainer.transform;
   //     Sphere.transform.position = new Vector3(-5, 0, 0);
   //     Sphere.AddComponent<ProceduralSphere3>();
   //
   //     Renderer SphereRender = Sphere.GetComponent<Renderer>();
   //     SphereRender.material = ShapesMat;
   //     SphereScript = Sphere.GetComponent<ProceduralSphere3>();
   // }
   //
   //
   // private void CreateDodec()
   // {
   //     StelDodec = new GameObject();
   //     StelDodec.name = "StellatedDodecahedron";
   //     StelDodec.transform.parent = ShapesContainer.transform;
   //     StelDodec.AddComponent<ProceduralDodecahedron>();
   //
   //     Renderer renderer = StelDodec.GetComponent<Renderer>();
   //     renderer.material = ShapesMat;
   //     DodecScript = StelDodec.GetComponent<ProceduralDodecahedron>();
   // }
   // private void CreateCylinder()
   // {
   //     Cylinder = new GameObject();
   //     Cylinder.name = "Cylinder";
   //     Cylinder.transform.parent = ShapesContainer.transform;
   //     Cylinder.transform.position = new Vector3(5, 0, 0);
   //     Cylinder.AddComponent<ProceduralCylinder1>();
   //
   //     Renderer renderer = Cylinder.GetComponent<Renderer>();
   //     renderer.material = ShapesMat;
   //     CylinderScript = Cylinder.GetComponent<ProceduralCylinder1>();
   //
   // }





  



}