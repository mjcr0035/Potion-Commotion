using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public AudioClip[] soundClips; // Array to hold multiple audio clips
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < soundClips.Length)
        {
            audioSource.PlayOneShot(soundClips[clipIndex]);
        }
        else
        {
            Debug.LogWarning("Clip index out of range.");
        }
    }

}
