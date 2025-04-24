using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public static class VolumeManager
{
    public static AudioMixer audioMixer;

    public static Slider masterSlider;
    public static Slider musicSlider;
    public static Slider sfxSlider;

    public static void Init(AudioMixer mixer)
    {
        audioMixer = mixer;
        LoadVolume();
    }

    public static void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        if (masterSlider != null) masterSlider.value = volume;
    }

    public static void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
        if (musicSlider != null) musicSlider.value = volume;
    }

    public static void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        if (sfxSlider != null) sfxSlider.value = volume;
    }

    public static void LoadVolume()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 0f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0f);

        audioMixer.SetFloat("MasterVolume", master);
        audioMixer.SetFloat("MusicVolume", music);
        audioMixer.SetFloat("SFXVolume", sfx);

        if (masterSlider != null) masterSlider.value = master;
        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}
