using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource sFXPlayer;

    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.1f;

    public void PlaySFX(AudioData audioData)
    { 
        sFXPlayer.PlayOneShot(audioData.audioClip, audioData.volume); 
    }

    public void Shutdown()
    {
        sFXPlayer.Stop();
    }

    public void PlayerRandomSFX(AudioData audioData)
    {
        sFXPlayer.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        PlaySFX(audioData);
    }

    public void PlayerRandomSFX(AudioData[] audioDatas)
    {
        PlayerRandomSFX(audioDatas[Random.Range(0, audioDatas.Length)]);
    }
}