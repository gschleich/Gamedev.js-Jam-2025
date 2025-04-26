using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;

    private void Start()
    {
        VolumeManager.Init(audioMixer);
        VolumeManager.masterSlider = masterSlider;
        VolumeManager.musicSlider = musicSlider;
        VolumeManager.sfxSlider = sfxSlider;
        VolumeManager.LoadVolume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Play();
            else
                PauseGame();
        }
    }

    public void Play()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        VolumeManager.Save();
        TopDownPlayerMovement.ResetMeter();
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateMasterVolume(float volume) => VolumeManager.SetMasterVolume(volume);
    public void UpdateMusicVolume(float volume) => VolumeManager.SetMusicVolume(volume);
    public void UpdateSoundVolume(float volume) => VolumeManager.SetSFXVolume(volume);
}
