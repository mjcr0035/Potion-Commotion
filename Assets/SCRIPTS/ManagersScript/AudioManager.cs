using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //matthew - manager that handles instantiation of gameobjects with audio sources that play selected audio clip(s)
    //based on implementation by @Sasquatch B Studios on YT

    public static AudioManager Instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] public AudioSource musicObject;

    public AudioClip[] menuSounds;
    public AudioClip[] pageSounds;
    public AudioClip[] swapSounds;

    public AudioClip[] musicTracks;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, float pitch, string name)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.name = name;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);


    }

    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume, float pitch)
    {
        int rand = Random.Range(0, audioClip.Length);

        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip[rand];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

    }

    public void PlayMusic(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(musicObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

    }

    public void DynMusicSwap(AudioClip newMusicClip, Transform spawnTransform, float volume, float transitionTime, int track)
    {
        

        //musicTracks[track] = newMusicClip;

    }

    public void UISound()
    {
        PlayRandomSoundFXClip(menuSounds, transform, 0.6f, Random.Range(1f, 1.4f));
    }
    public void PageTurn()
    {
        PlayRandomSoundFXClip(pageSounds, transform, 0.6f, Random.Range(1f, 1.4f));
    }
    public void ScreenSwap()
    {
        PlayRandomSoundFXClip(swapSounds, transform, 0.6f, Random.Range(1f, 1.4f));
    }

}

