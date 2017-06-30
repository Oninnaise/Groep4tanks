using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsInit : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer soundMixer;


    public void setMusicLevel(float mvalue) // Aanpassen volume muziek
    {
        musicMixer.SetFloat("MusicVolume", mvalue);
    }

    public void setSoundLevel(float svalue) // Aanpassen volume geluid
    {
        musicMixer.SetFloat("SoundVolume", svalue);
    }

    public void ToggleFullScreen() // Wisselen tussen full screen en windowed
    {
        if (Screen.fullScreen = !Screen.fullScreen)
        {
            Screen.fullScreen = Screen.fullScreen;
            Screen.SetResolution(Screen.width, Screen.height, true, 60); // width height framerate
        }
        else
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}