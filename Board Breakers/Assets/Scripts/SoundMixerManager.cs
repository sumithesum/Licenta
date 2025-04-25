using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void setMasterVolume(float volume)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10( volume) * 20f);
    }

    public void setMusicVolume(float volume)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void setFxVolume(float volume)
    {
        mixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
    }
}
