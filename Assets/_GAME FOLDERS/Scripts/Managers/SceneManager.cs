using UnityEngine;

public class SceneManager: MonoBehaviour
{
    public void SceneController(int scene)
    {
        //Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
