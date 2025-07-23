using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeSliders : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider MasterVolumeSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider VoiceSlider;
    private AudioManager audioManager;

    //Functions ensures that player prefs for volume are set. If not, it sets them to 0. 
    //Otherwise, it loads the values.
    //Current Devs:
    //Fredrick Bouloute (bouloutef04)
    void Start()
    {
        if (!PlayerPrefs.HasKey("Master"))
        {
            PlayerPrefs.SetFloat("Master", 0.7f);
        }

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", -10);
        }

        if (!PlayerPrefs.HasKey("SFX"))
        {
            PlayerPrefs.SetFloat("SFX", -10);
        }
        if (!PlayerPrefs.HasKey("Voice"))
        {
            PlayerPrefs.SetFloat("Voice", -10);
        }
        //Load after ensuring playerprefs exist
        Load();
    }

    //Function takes the master volume slide and sets audiolisten (overall audio) to its value.
    public void SetMasterVolume()
    {
        AudioListener.volume = MasterVolumeSlider.value;
        PlayerPrefs.Save();
    }
    //Function takes in the volume slider value and sets the music parameter in mixer
    //to the value given.
    public void SetMusicVolume()
    {
        audioMixer.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFX", SFXSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
        PlayerPrefs.Save();
    }
    public void SetVoiceVolume()
    {
        audioMixer.SetFloat("Voice", VoiceSlider.value);
        PlayerPrefs.SetFloat("Voice", VoiceSlider.value);
        PlayerPrefs.Save();
    }
    //Set sliders to player prefs.
    private void Load()
    {
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("Master");
        AudioListener.volume = MasterVolumeSlider.value;

        MusicSlider.value = PlayerPrefs.GetFloat("Music");
        PlayerPrefs.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
  
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));

        VoiceSlider.value = PlayerPrefs.GetFloat("Voice");
        audioMixer.SetFloat("Voice", PlayerPrefs.GetFloat("Voice"));
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Master", MasterVolumeSlider.value);
        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);
        PlayerPrefs.SetFloat("Voice", VoiceSlider.value);
        PlayerPrefs.Save();
    }
}
