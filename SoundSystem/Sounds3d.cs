using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds3d : MonoBehaviour
{
    [HideInInspector] public AudioSource audioSource;


    
    public AudioClip destroyTurret;

    public static Sounds3d sounds3d;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        if (sounds3d != null) return;
        else sounds3d = this;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        Debug.Log("Destroy turret sound");
    }
}
