using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCtr))]
public class CharaTeamCtr : MonoBehaviour
{
    private Team tempTeam = Team.None;
    internal PlayerCtr playerCtr => GetComponent<PlayerCtr>();
    PlayerConfig playerConfig => playerCtr.playerConfig;
    internal bool isReady = false;
    public EffectSwitcher confirmEffect;
    internal void SetTeamInConfig()
    {
        if (!CharaTeamManger.instance.canChangeTeam)
            return;
        if (tempTeam == Team.None)
            return;
        playerCtr.CanMove(false);      
        playerConfig.SetPlayerTeam(tempTeam);
        GetComponentInChildren<TeamPlateShow>().ChangeTeam(tempTeam);
        playerCtr.choosedTeam = tempTeam;
        isReady = true;
        confirmEffect.StartEffect(tempTeam);
        playerCtr.teamShowEffect.StartEffect(tempTeam);
        CharaTeamManger.instance.CheckReady();
    }

    internal void CancelTeamInConfig()
    {
        if (!CharaTeamManger.instance.canChangeTeam)
            return;
        playerCtr.CanMove(true);
        playerConfig.SetPlayerTeam(Team.None);
        GetComponentInChildren<TeamPlateShow>().ChangeTeam(Team.None);
        playerCtr.choosedTeam = Team.None;
        isReady = false;
        CharaTeamManger.instance.StopCount();
        playerCtr.teamShowEffect.StopEffect();
    }

    internal void SetTempTeam(Team team)
    {
        tempTeam = team;
    }
}
