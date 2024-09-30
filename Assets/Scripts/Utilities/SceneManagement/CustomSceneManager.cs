using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities.Mono.Singletons;

namespace Utilities.SceneManagement
{
    public class CustomSceneManager : MonoSingletonPersistent<CustomSceneManager>
    {
        //private bool _pipelineSwitched;
        [SerializeField] private int defaultSceneIndex;
        private void Update()
        {
            // // Check if the user is on a non-main scene and presses the Escape key
            if (SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape))
            {
                // Load the main scene (assuming the main scene is at build index 0)
                LoadScene(defaultSceneIndex);
            }
        }


        // General method to load scenes based on build index
        public void LoadScene(int buildIndex) =>StartCoroutine(LoadYourAsyncScene(buildIndex));
        
        private static IEnumerator LoadYourAsyncScene(int buildIndex)
        {
            // Begin to load the scene
            var asyncOperation = SceneManager.LoadSceneAsync(buildIndex);

            // Optionally, you can prevent the scene from activating immediately by setting this to false
            if (asyncOperation == null) yield break;
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        
    }
}
