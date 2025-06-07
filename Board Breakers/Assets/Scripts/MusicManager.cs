using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] musicTracks;
    
    void Awake()
    {
        PlayRandomMusic();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!audio.isPlaying || audio.clip == null)
        {
            PlayRandomMusic();
        }
    }

    public void PlayRandomMusic()
    {
        int index = Random.Range(0, musicTracks.Length);
        audio.clip = musicTracks[index];
        audio.Play();
    }
}
