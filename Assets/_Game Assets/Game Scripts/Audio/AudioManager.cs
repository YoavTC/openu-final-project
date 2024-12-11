using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using External_Packages;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Header("Mixer")] 
    [SerializeField] private SerializedDictionary<AudioType, float> audioTypeVolumeMixer;

    [SerializeField] private float destroyAudioSourceDelay;
    [SerializeField] private Vector2 randomPitchRange;
    
    public void PlayAudioClip(AudioClip audioClip, AudioClipSettingsStruct audioClipSettings)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = audioClipSettings.volume * audioTypeVolumeMixer[audioClipSettings.type];
        audioSource.loop = audioClipSettings.loop;
        audioSource.pitch = audioClipSettings.randomPitch ? Random.Range(randomPitchRange.x, randomPitchRange.y) : 1f;
        
        audioSource.Play();

        if (!audioClipSettings.loop)
        {
            StartCoroutine(ProcessAudioSource(audioSource));
        }
    }

    IEnumerator ProcessAudioSource(AudioSource audioSource)
    {
        float audioClipLength = audioSource.clip.length;
        yield return new WaitForSecondsRealtime(audioClipLength + destroyAudioSourceDelay);
        Destroy(audioSource);
        // emptyAudioSourceQueue.Enqueue(audioSource);
    }

    private Queue<AudioSource> emptyAudioSourceQueue = new Queue<AudioSource>();

    private AudioSource GetEmptyAudioSource()
    {
        if (emptyAudioSourceQueue.TryDequeue(out AudioSource audioSource))
        {
            if (emptyAudioSourceQueue.Count > 15) emptyAudioSourceQueue.Clear();
            return audioSource;
        }

        return gameObject.AddComponent<AudioSource>();
    }
}

[Serializable]
public struct AudioClipSettingsStruct
{
    public AudioType type;
    public float volume;
    public bool loop;
    public bool randomPitch;
}