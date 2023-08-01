using UnityEngine;

public class SceneManager: MonoBehaviour
{
    public void SceneController(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
