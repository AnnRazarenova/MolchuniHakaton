using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Нужно для слайдеров

public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("Settings UI")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;

    void Start()
    {
        // При старте игры загружаем сохраненные настройки,
        // чтобы ползунки стояли там, где их оставили
        LoadSettings();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (PlayerMovement.InputBlocked) return;

            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement.InputBlocked = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement.InputBlocked = false;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    // --- НОВЫЙ МЕТОД ДЛЯ КНОПКИ "ПОДТВЕРДИТЬ" ---
    public void SaveSettings()
    {
        if (musicSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);

        if (sfxSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.Save();
        Debug.Log("Настройки сохранены из меню паузы!");
    }

    // Загрузка настроек при старте сцены
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVolume");
            if (musicSlider != null) musicSlider.value = musicVol;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume");
            if (sfxSlider != null) sfxSlider.value = sfxVol;
        }
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        PlayerMovement.InputBlocked = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }
}