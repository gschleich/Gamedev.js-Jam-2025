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
