using UnityEngine;

public class PlayAudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioClipSettingsStruct audioClipSettings;
    
    public void PlayAudio(AudioClip audioClip)
    {
        AudioManager.Instance.PlayAudioClip(audioClip, audioClipSettings);
    }
}
