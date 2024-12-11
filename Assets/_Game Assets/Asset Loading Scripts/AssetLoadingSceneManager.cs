using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetLoadingSceneManager : MonoBehaviour
{
    void Start()
    {
        LoadAssets();
    }

    private async void LoadAssets()
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync("Main Menu");
        if (scene != null)
        {
            scene.allowSceneActivation = false;

            await Task.Delay(1500);
            
            while (!scene.isDone)
            {
                Debug.Log(scene.progress);
                if (scene.progress >= 0.9f)
                {
                    break;
                }
                await Task.Yield();
            }
            
            scene.allowSceneActivation = true;
        }
    }
}
