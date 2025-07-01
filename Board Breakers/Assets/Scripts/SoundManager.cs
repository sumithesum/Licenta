using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioClip[] sfx; 
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            return;
        }
            DontDestroyOnLoad(this);
    }

    public void PlaySoundFX(int i , Transform transform )
    {

        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);

        audioSource.clip = sfx[i];

        

        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);


    }

}
