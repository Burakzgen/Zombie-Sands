using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    private void Awake()
    {
        _qualityDropdown.value = QualitySettings.GetQualityLevel();
        _qualityDropdown.onValueChanged.AddListener(SetQuality);
    }
    public void MuteSounds()
    {
        AudioListener.volume = 0;
    }
    public void OnSounds()
    {
        AudioListener.volume = 1;
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

}
