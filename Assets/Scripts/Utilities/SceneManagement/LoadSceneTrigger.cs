using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.SceneManagement
{
    public class LoadSceneTrigger: MonoBehaviour
    {
        // General method to load scenes based on build index
        public void LoadScene(int sceneIndex) =>  SceneManager.LoadScene(sceneIndex);
    
    }
}
