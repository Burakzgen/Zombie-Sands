using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _qualityDropdown;
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
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    private void SetAudioListenerVolume(int volumeLevel)
    {
        AudioListener.volume = volumeLevel;
    }
}
