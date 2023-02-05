using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private class ClipCollection
    {
        [SerializeField] public ClipPurpose ClipPurpose;
        [SerializeField] public AudioClip AudioClip; //TODO list & ranomize button
    }
    [Serializable]
    private class MusicCollection
    {
        [SerializeField] public MusicPurpose MusicPurpose;
        [SerializeField] public AudioClip AudioClip; //TODO list & ranomize button
    }
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private List<ClipCollection> clipCollection;
    [SerializeField] private List<MusicCollection> musicCollections;

    public static AudioManager Instance = null;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public void PlayAudio(ClipPurpose clipPurpose)
    {
        //TODO probably need to spawn audiosources
        AudioClip clip = clipCollection.Where(x => x.ClipPurpose == clipPurpose).Select(x => x.AudioClip).FirstOrDefault();
        if (clip == null)
            return;
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayMusic(MusicPurpose musicPurpose, bool loop = true)
    {
        if(musicSource.isPlaying){
            Debug.Log("MusicSource is playing, switching to " + musicPurpose);
            //TODO 2 sources fadein & out
        }
        //TODO FADE
        AudioClip clip = musicCollections.Where(x => x.MusicPurpose == musicPurpose).Select(x => x.AudioClip).FirstOrDefault();
        if (clip == null)
            return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
}
[Serializable]
public enum ClipPurpose
{
    Planting,
    Watering,
    Gathering,
    Stomping,
    WinningSound,
    GameOverSound,
    RootSound
}
[Serializable]
public enum MusicPurpose
{
    Chill,
    Hectic,
    Eclipse,
    Win
}
