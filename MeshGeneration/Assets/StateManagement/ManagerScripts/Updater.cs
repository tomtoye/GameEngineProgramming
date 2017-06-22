using UnityEngine;

public class Updater : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //if we in a state, run that states update function
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.Update();
        }
    }

    //quick function set up to allow user to quit demo scenes
    public void QuitProject()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }




}
