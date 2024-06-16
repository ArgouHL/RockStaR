using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSwitcher : TeamEffect
{
    [SerializeField] private Color yellow;
    [SerializeField] private Color cyan;
    [SerializeField] private bool setActive = false;
    [SerializeField] private ParticleSystem noTeamEffect;
    [SerializeField] private ParticleSystem yellowEffect;
    [SerializeField] private ParticleSystem blueEffect;
    [SerializeField] private Light _light;

    internal override void StartEffect(Team team)
    {
        StopEffect();
        ParticleSystem effect = null;
        switch (team)
        {
            case Team.None:
                if (noTeamEffect == null)
                    return;
                effect = noTeamEffect;
                break;
            case Team.Blue:
                effect = blueEffect;


                break;
            case Team.Yellow:
                effect = yellowEffect;
                break;

        }
        if (effect != null)
        {
            effect.Play();
            if (setActive)
                effect.gameObject.SetActive(true);
        }
        if (_light != null)
            StartLight(team);
    }

    internal void StartEffectInTime(Team team, float startTime)
    {
        StopEffect();
        switch (team)
        {
            case Team.None:
                if (noTeamEffect != null)
                {
                    noTeamEffect.Simulate(startTime, true, true);
                    noTeamEffect.Play();
                }


                break;
            case Team.Blue:
                blueEffect.Simulate(startTime, true, true);
                blueEffect.Play();

                break;
            case Team.Yellow:
                yellowEffect.Simulate(startTime, true, true);
                yellowEffect.Play();

                break;

        }
        if (_light != null)
            StartLight(team);
    }


    internal override void StopEffect()
    {
        blueEffect.Stop();
        yellowEffect.Stop();
        if (noTeamEffect != null)
            noTeamEffect.Stop();
        if (setActive)
        {
            if (noTeamEffect != null)
                noTeamEffect.gameObject.SetActive(true);
            blueEffect.gameObject.SetActive(true);
            yellowEffect.gameObject.SetActive(true);
        }

        if (_light != null)
            StopLight();
    }



    private void StartLight(Team team)
    {
        _light.enabled = true;
        switch (team)
        {
            case Team.None:
                _light.color = Color.gray;
                break;
            case Team.Blue:
                _light.color = cyan;

                break;
            case Team.Yellow:
                _light.color = yellow;
                break;

        }
    }
    private void StopLight()
    {
        _light.enabled = false;
    }
}
