using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderMusic, sliderSound;

    void Start(){
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVol", 0f);
        sliderSound.value = PlayerPrefs.GetFloat("SoundVol", 0f);
    }

    public void SetVolumeMusic (float volume) {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }
    public void SetVolumeSound (float volume) {
        audioMixer.SetFloat("SoundEffectVolume", volume);
         PlayerPrefs.SetFloat("SoundVol", volume);
    }
}
