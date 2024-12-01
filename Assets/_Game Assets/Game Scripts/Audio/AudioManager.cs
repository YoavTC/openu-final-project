using System;
using System.Collections;
using External_Packages;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private float destroyAudioSourceDelay;
    [SerializeField] private Vector2 randomPitchRange;
    
    public void PlayAudioClip(AudioClip audioClip, AudioClipSettingsStruct audioClipSettings)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = audioClipSettings.volume;
        audioSource.loop = audioClipSettings.loop;
        audioSource.pitch = audioClipSettings.randomPitch ? Random.Range(randomPitchRange.x, randomPitchRange.y) : 1f;
        
        audioSource.Play();

        if (!audioClipSettings.loop)
        {
            StartCoroutine(DestroyAudioSource(audioSource));
        }
    }

    IEnumerator DestroyAudioSource(AudioSource audioSource)
    {
        float audioClipLength = audioSource.clip.length;
        yield return new WaitForSeconds(audioClipLength + destroyAudioSourceDelay);
        Destroy(audioSource);
    }
}

[Serializable]
public struct AudioClipSettingsStruct
{
    public float volume;
    public bool loop;
    public bool randomPitch;
}