using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Script for integrating the preloader scene in the unity editor when pressing the play button.
/// Saves having to switch to preload scene and then pressing play.
/// </summary>
public class PreloaderIntegrationInEditor
{
#if UNITY_EDITOR
    public static int sceneLoadBack = -1;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void UsePreloader()
    {
        Debug.Log("Initialising Preloader Scene");
        //Get the current scene build index.
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == 0)
        {
            Debug.Log("Already at the Preloader Scene");
        }
        else
        {
            //Assign the scene to load back as the current scene.
            Debug.Log("Loading Preloader Scene");
            sceneLoadBack = currentScene;
            //Load the preloader scene, Should be index 0.
            SceneManager.LoadScene(0);
        }
    }
#endif
}
