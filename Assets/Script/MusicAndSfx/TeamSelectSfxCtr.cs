using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectSfxCtr : AudioSourceCtr
{
    [SerializeField] private AudioClipObj teleSfx;
    [SerializeField] private AudioClipObj conformSfx;

    internal void PlayTele()
    {
        PlayOnce(teleSfx);
    }

    internal void PlayConfirm()
    {
        PlayOnce(conformSfx);
    }
}
