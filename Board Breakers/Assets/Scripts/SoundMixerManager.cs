using GameKit.Dependencies.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    public static SoundMixerManager inst;

    private void Start()
    {
        if (inst == null)
        {
            DontDestroyOnLoad(this);
            inst = this;

        }
    }
    public static void setMasterVolume(float volume)
    {
        inst.mixer.SetFloat("masterVolume", Mathf.Log10( volume) * 20f);
    }

    public void setMusicVolume(float volume)
    {
        inst.mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void setFxVolume(float volume)
    {
        inst.mixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
    }
}
