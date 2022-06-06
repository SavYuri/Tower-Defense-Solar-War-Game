using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{

    [HideInInspector] public AudioSource audioSource;
    

    public AudioClip cancelButton;
    
    
    public  Sounds sounds;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (sounds != null) return;
        else sounds = this;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
