using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _textsToChange;
    [SerializeField] private string[] _english;
    [SerializeField] private string[] _turkish;
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private Button _enButton, trButton;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Language") == 1)
            ChangeLanguageToTurkish();
        else
            ChangeLanguageToEnglish();
    }
    private void OnEnable()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            _enButton.onClick.AddListener(ChangeLanguageToEnglish);
            trButton.onClick.AddListener(ChangeLanguageToTurkish);
        }
    }
    public void ChangeLanguageToTurkish()
    {
        for (int i = 0; i < _textsToChange.Length; i++)
        {
            _textsToChange[i].text = _turkish[i];
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            ChangeLanguageDropdown("DÜSÜK", "ORTA", "YUKSEK");

            _enButton.gameObject.SetActive(true);
            trButton.gameObject.SetActive(false);
        }
        PlayerPrefs.SetInt("Language", 1);
    }

    public void ChangeLanguageToEnglish()
    {
        for (int i = 0; i < _textsToChange.Length; i++)
        {
            _textsToChange[i].text = _english[i];
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
        {
            ChangeLanguageDropdown("LOW", "MEDIUM", "HIGH");

            _enButton.gameObject.SetActive(false);
            trButton.gameObject.SetActive(true);
        }
        PlayerPrefs.SetInt("Language", 0);
    }
    private void ChangeLanguageDropdown(string text1, string text2, string text3)
    {
        _dropdown.options[0].text = text1;
        _dropdown.options[1].text = text2;
        _dropdown.options[2].text = text3;

        _dropdown.RefreshShownValue();
    }
}
