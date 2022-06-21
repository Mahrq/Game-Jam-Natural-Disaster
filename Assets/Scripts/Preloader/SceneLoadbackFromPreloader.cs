using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Loads the previous scene when the play button in editor was pressed.
/// </summary>
public class SceneLoadbackFromPreloader : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Default load will not load the scene back to when the play button was pressed.")]
    private bool _defaultLoad = true;

    private void Start()
    {
#if UNITY_EDITOR
        if (_defaultLoad)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            if (PreloaderIntegrationInEditor.sceneLoadBack > 0)
            {
                Debug.Log($"Preloader successfully loaded, Returning to Scene: {PreloaderIntegrationInEditor.sceneLoadBack}");
                SceneManager.LoadScene(PreloaderIntegrationInEditor.sceneLoadBack);
            }
        }

#else   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
#endif
    }
}
