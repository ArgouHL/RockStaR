using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalSfxControl : AudioSourceCtr
{
    [SerializeField] private AudioClipObj crystalAppearSfx;
    [SerializeField] private AudioClipObj crystalWrongingSfx;
    [SerializeField] private AudioClipObj crystalDisappearSfx;
    [SerializeField] private AudioClipObj crystalHitSfx;
    [SerializeField] private AudioClipObj crystalChangeTeamSfx;



    internal void PlayCrystalAppearSfx()
    {
        PlayOnce(crystalAppearSfx);
    }

    internal void PlayCrystalWrongingSfx()
    {
        PlayOnce(crystalWrongingSfx);
    }

    internal void PlayCrystalDisappearSfx()
    {
        PlayOnce(crystalDisappearSfx);
    }

    internal void PlayCrystalHitSfx()
    {
        PlayOnce(crystalHitSfx);
    }

    internal void PlayCrystalChangeTeamSfx()
    {
        PlayOnce(crystalChangeTeamSfx);
    }

}
