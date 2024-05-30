using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaTeamManger : MonoBehaviour
{
    public static CharaTeamManger instance;
    [SerializeField]
    internal List<CharaTeamCtr> charaTeamCtrs;

    private void Awake()
    {
        instance = this;
        charaTeamCtrs = new List<CharaTeamCtr>();
    }

    internal void AddTeamCtr(CharaTeamCtr ctr)
    {
        charaTeamCtrs.Add(ctr);
    }

    internal void ConfirmTeam(int playerIndex)
    {
        charaTeamCtrs[playerIndex].SetTeamInConfig();
    }

    internal void CancelTeam(int playerIndex)
    {
        charaTeamCtrs[playerIndex].CancelTeamInConfig();
    }
}
