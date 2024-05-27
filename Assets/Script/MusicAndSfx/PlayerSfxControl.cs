using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSfxControl : AudioSourceCtr
{
    [SerializeField] private AudioClipObj shootSfx;
    [SerializeField] private AudioClipObj getEnergyHitBackSfx;





    internal void PlayShootSfx()
    {
        PlayOnce(shootSfx);
    }

    internal void PlayGetEnergyHitBackSfx()
    {
        PlayOnce(getEnergyHitBackSfx);
    }
}
