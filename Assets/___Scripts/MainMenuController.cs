using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game";

    [Header("Settings UI")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    void Start()
    {
        LoadSettings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenSettings()
    {
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    // --- 1. КНОПКА "ВЫЙТИ/НАЗАД" (просто закрывает меню) ---
    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    // --- 2. КНОПКА "СОХРАНИТЬ" (только сохраняет, окно не закрывает) ---
    public void SaveSettings()
    {
        if (_musicSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", _musicSlider.value);

        if (_sfxSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", _sfxSlider.value);

        PlayerPrefs.Save();
        Debug.Log("Настройки сохранены!");

        // Здесь можно добавить эффект (например, мигание кнопки или звук), 
        // чтобы игрок понял, что нажатие сработало.
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVolume");
            if (_musicSlider != null) _musicSlider.value = musicVol;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume");
            if (_sfxSlider != null) _sfxSlider.value = sfxVol;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}