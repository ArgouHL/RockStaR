using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new AudioClip", menuName = "AudioClipObj/new AudioClip")]
public class AudioClipObj :  ScriptableObject
{
    public AudioClip audioClip;
    public AudioSetting audioSetting;
    
}

[Serializable]
public class AudioSetting
{

}