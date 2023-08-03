using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] SceneLoader _sceneLoader;
    private void Awake()
    {
        InitializeQualityDropdown();
    }
    private void InitializeQualityDropdown()
    {
        _qualityDropdown.value = QualitySettings.GetQualityLevel();
        _qualityDropdown.onValueChanged.AddListener(SetQuality);
    }
    public void MuteAllSounds()
    {
        AudioListener.volume = 0;
    }
    public void UnmuteAllSounds()
    {
        AudioListener.volume = 1;
    }
    // Duruma gore ilave edilebilir.
    private void SetAudioListenerVolume(int volumeLevel)
    {
        AudioListener.volume = volumeLevel;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetNormalMode()
    {
        PlayerPrefs.SetInt("GameMode", 0);
        _sceneLoader.LoadSceneWithLoadingScreen(1);
    }

    public void SetTimeMode()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        _sceneLoader.LoadSceneWithLoadingScreen(1);
    }
}
