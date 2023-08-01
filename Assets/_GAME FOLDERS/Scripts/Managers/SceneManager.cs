using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [Header("LOADING CONTROLS")]
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Slider _loadingSlider;
    public void SceneController(int scene)
    {

        StartCoroutine(LoadingController(scene));
    }

    IEnumerator LoadingController(int scene)
    {
        _loadingPanel.SetActive(true);
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        while (!operation.isDone)
        {
            float value = Mathf.Clamp01(operation.progress / .9f);
            _loadingSlider.value = value;
            yield return null;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
