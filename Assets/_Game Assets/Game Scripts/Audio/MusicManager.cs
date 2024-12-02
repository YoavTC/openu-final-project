using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    [Header("Settings")] 
    [SerializeField] private bool playOnAwake;
    [SerializeField] private AudioClipSettingsStruct audioClipSettings;

    private void Start()
    {
        if (playOnAwake)
        {
            AudioManager.Instance.PlayAudioClip(music, audioClipSettings);
        }
    }
}
