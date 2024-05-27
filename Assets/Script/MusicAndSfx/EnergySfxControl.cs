using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnergySfxControl : AudioSourceCtr
{
    [SerializeField] private AudioClipObj energtHit;
   



    internal void PlayEnergyHitSfx(Vector3 pos)
    {
        transform.position = pos;
        PlayOnce(energtHit);
    }

    

}
