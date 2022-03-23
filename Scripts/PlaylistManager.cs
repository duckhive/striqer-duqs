using System;
using System.Collections;
using System.Collections.Generic;
using Audio.DATA;
using Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class PlaylistManager : MonoBehaviour
{
    public static PlaylistManager instance;
    
    [BoxGroup("Track List")] public List<Track> audioTracks;
    [BoxGroup("UI")] public TMP_Text trackTextUi;

    public int trackIndex;
    [HideInInspector] public AudioSource playlistAudioSource;

    public PlayOrder playOrder;
    public bool loop;
    public bool playNextWhenTrackEnds;
    
    public Vector2 volume = new Vector2(0.5f, 0.5f);
    //[Required] [BoxGroup("Scriptable Object")] public SoundFXData data;
    

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        playlistAudioSource = GetComponentInChildren<AudioSource>();
        trackIndex = 0;
        playlistAudioSource.clip = audioTracks[trackIndex].trackAudioClip;
        trackTextUi.text = audioTracks[trackIndex].name;
        PlayAudio();
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (playlistAudioSource.isPlaying == false)
        {
            GetAudioClip();
            UpdateTrack(trackIndex);
            PlayAudio();
        }
            
    }

    public void NextTrack()
    {
        if (trackIndex < audioTracks.Count - 1)
        {
            trackIndex++;
            StartCoroutine(FadeOut(playlistAudioSource, 0.5f));
        }
    }

    public void PreviousTrack()
    {
        if (trackIndex >= 1)
        {
            trackIndex--;
            StartCoroutine(FadeOut(playlistAudioSource, 0.5f));
        }
    }
    
    private void UpdateTrack(int index)
    {
        playlistAudioSource.clip = audioTracks[index].trackAudioClip;
        trackTextUi.text = audioTracks[index].name;
    }

    public void AudioVolume(float volume)
    {
        playlistAudioSource.volume = volume;
    }

    public void PlayAudio()
    {
        StartCoroutine(FadeIn(playlistAudioSource, 0.5f));
    }
    
    public void PauseAudio()
    {
        playlistAudioSource.Pause();
    }
    
    public void StopAudio()
    {
        StartCoroutine(FadeOut(playlistAudioSource, 0.5f));
    }

    public IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        var startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = startVolume;
        UpdateTrack(trackIndex);
    }
    
    public IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        var startVolume = audioSource.volume;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        
        audioSource.volume = startVolume ;
    }
    public Track GetAudioClip()
    {
        // get current clip:
        var clip = audioTracks[trackIndex >= audioTracks.Count ? 0 : trackIndex];
            
        // find next clip:
        switch (playOrder)
        {
            case PlayOrder.InOrder:
                trackIndex = (trackIndex + 1) % audioTracks.Count;
                break;
            case PlayOrder.Random:
                trackIndex = Random.Range(0, audioTracks.Count);
                break;
            case PlayOrder.Reverse:
                trackIndex = (trackIndex + audioTracks.Count - 1) % audioTracks.Count;
                break;
        }
            
        return clip;
    }
    
    public AudioSource PlayAudioSource(AudioSource audioSourceParam = null)
    {
        if (audioTracks.Count == 0)
        {
            Debug.LogWarning($"Missing sound clips for {name}");
            return null;
        }

        var source = audioSourceParam;
        if (source == null)
        {
            var obj = new GameObject("Sound", typeof(AudioSource));
            source = obj.GetComponent<AudioSource>();
        }
        source.Play();

        Destroy(source.gameObject, source.clip.length / source.pitch);

        return source;
    }

    public enum PlayOrder
    {
        Random,
        InOrder,
        Reverse
    }
    
}
