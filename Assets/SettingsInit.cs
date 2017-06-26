using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsInit : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer soundMixer;


    public void setMusicLevel(float mvalue)
    {
        musicMixer.SetFloat("MusicVolume", mvalue);
    }

    public void setSoundLevel(float svalue)
    {
        musicMixer.SetFloat("SoundVolume", svalue);
    }

    public void ToggleFullScreen()
    {
        if (Screen.fullScreen = !Screen.fullScreen)
        {
            Screen.fullScreen = Screen.fullScreen;
            Screen.SetResolution(Screen.width, Screen.height, true, 60);
        }
        else
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}