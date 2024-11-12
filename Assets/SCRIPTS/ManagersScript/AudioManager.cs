using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //matthew - manager that handles instantiation of gameobjects with audio sources that play selected audio clip(s)
    //based on implementation by @Sasquatch B Studios on YT

    public static AudioManager Instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] public AudioSource currentMusicTrack;

    public AudioClip[] menuSounds;
    public AudioClip[] pageSounds;
    public AudioClip[] swapSounds;

    public AudioClip[] musicTrackClips;

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


    //fade between an existing audioclip in the audio source MusicObject and a desired audioclip
    public IEnumerator FadeTrack(int newMusicClip)
    {
        float fadeTime = 1.5f;
        float timeElapsed = 0;
        
        //fades out current audioclip
        while (timeElapsed < fadeTime)
        {
            currentMusicTrack.volume = Mathf.Lerp(1, 0f, timeElapsed / fadeTime);
            timeElapsed += Time.deltaTime;


            yield return null;
        }
        
        //checks the current point in seconds in the track the song is playing
        float musicPlaybackPoint = currentMusicTrack.time;
        
        //switches clips, then plays at the stored musictrack.time point
        currentMusicTrack.clip = musicTrackClips[newMusicClip];
        currentMusicTrack.Play();
        currentMusicTrack.time = musicPlaybackPoint;
        
        //resets fadetime for upcoming while loop        
        fadeTime = 1.5f;
        timeElapsed = 0;

        //fades in desired audioclip
        while (timeElapsed < fadeTime)
        {
            currentMusicTrack.volume = Mathf.Lerp(0f, 1, timeElapsed / fadeTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
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

