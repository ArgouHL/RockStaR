using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceCtr : MonoBehaviour
{
    protected AudioSource audioSource;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected internal void PlayOnce(AudioClipObj audioClipObj)
    {
      //  Debug.Log("Play:" + audioClipObj.name);
        audioSource.PlayOneShot(audioClipObj.audioClip);
    }

    protected internal void Play(AudioClipObj audioClipObj)
    {
        audioSource.clip = audioClipObj.audioClip;
        audioSource.Play();
    }
}
