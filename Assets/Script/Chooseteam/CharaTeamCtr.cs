using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCtr))]
public class CharaTeamCtr : MonoBehaviour
{
    private Team tempTeam = Team.None;
    PlayerCtr playerCtr => GetComponent<PlayerCtr>();
    PlayerConfig playerConfig => playerCtr.playerConfig;

    internal void SetTeamInConfig()
    {
        if (tempTeam == Team.None)
            return;
        playerCtr.CanMove(false);
        playerConfig.SetPlayerTeam(tempTeam);
        GetComponentInChildren<TeamPlateShow>().ChangeTeam(tempTeam);

       
    }

    internal void CancelTeamInConfig()
    {
        playerCtr.CanMove(true);
        playerConfig.SetPlayerTeam(Team.None);
        GetComponentInChildren<TeamPlateShow>().ChangeTeam(Team.None);


    }

    internal void SetTempTeam(Team team)
    {
        tempTeam = team;
    }
}
