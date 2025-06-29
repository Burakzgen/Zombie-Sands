using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("LOADING CONTROLS")]
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private Slider _loadingSlider;
    public void LoadSceneWithLoadingScreen(int sceneIndex)
    {
        StartCoroutine(StartAsyncSceneLoad(sceneIndex));
    }
    IEnumerator StartAsyncSceneLoad(int sceneIndex)
    {
        _loadingPanel.SetActive(true);
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex);
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
