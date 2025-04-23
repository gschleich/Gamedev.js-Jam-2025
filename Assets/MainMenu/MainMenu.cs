using System.Collections;
using System.Collections.Generic;
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
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public void Play()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMasterVolume(float volume)
    {
        Debug.Log("Master Volume: " + volume);
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void UpdateMusicVolume(float volume)
    {
        Debug.Log("Music Volume: " + volume);
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        Debug.Log("SFX Volume: " + volume);
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MasterVolume", out float masterVolume);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);

        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            masterSlider.value = 0.75f;
            musicSlider.value = 0.75f;
            sfxSlider.value = 0.75f;

            PlayerPrefs.SetFloat("MasterVolume", 0.75f);
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);
            PlayerPrefs.SetFloat("SFXVolume", 0.75f);
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.Save();
        }
        else
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        }
    }
}
