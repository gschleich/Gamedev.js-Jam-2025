using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public string nextSceneName;

    // Reference to the meter (UI element) that needs to be reset
    public RectTransform meter;

    private void Start()
    {
        MusicManager.Instance.PlayMusic("MainMenu");

        VolumeManager.Init(audioMixer);
        VolumeManager.masterSlider = masterSlider;
        VolumeManager.musicSlider = musicSlider;
        VolumeManager.sfxSlider = sfxSlider;
        VolumeManager.LoadVolume();
    }

    public void Play()
    {
        // Reset the meter position before loading the next scene
        if (meter != null)
        {
            meter.anchoredPosition = new Vector2(0, meter.anchoredPosition.y); // Reset to (0, currentY)
            PlayerPrefs.DeleteKey("MeterPosition"); // Optionally clear the saved position
            PlayerPrefs.Save(); // Ensure changes are saved
        }

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMasterVolume(float volume) => VolumeManager.SetMasterVolume(volume);
    public void UpdateMusicVolume(float volume) => VolumeManager.SetMusicVolume(volume);
    public void UpdateSoundVolume(float volume) => VolumeManager.SetSFXVolume(volume);

    public void SaveVolume()
    {
        VolumeManager.Save();
    }
}
