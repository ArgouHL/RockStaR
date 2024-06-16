using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharaTeamManger : MonoBehaviour
{
    public static CharaTeamManger instance;
    [SerializeField]
    internal List<CharaTeamCtr> charaTeamCtrs;
    [SerializeField] private int playerNum = 4;
    [SerializeField] private float countTime = 3;
    private Coroutine teleCountCoro;
    internal bool canChangeTeam = true;
    [SerializeField] internal TeamSelectSfxCtr teamSelectSfxCtr;
    public static CharaType[] yellowTeam;
    public static CharaType[] blueTeam;
    private void Awake()
    {
        instance = this;
        charaTeamCtrs = new List<CharaTeamCtr>();
        yellowTeam = null;
        blueTeam = null;
    }

    internal void AddTeamCtr(CharaTeamCtr ctr)
    {
        charaTeamCtrs.Add(ctr);
        Debug.Log("AddTeamCtr");
    }

    internal bool ConfirmTeam(int playerIndex)
    {

        charaTeamCtrs[playerIndex].SetTeamInConfig();
        Debug.Log("ConfirmTeam");
        return false;
    }

    internal bool CancelTeam(int playerIndex)
    {
        charaTeamCtrs[playerIndex].CancelTeamInConfig();
        return true;
    }

    internal void CheckReady()
    {
        teamSelectSfxCtr.PlayConfirm();
        if (charaTeamCtrs.Count >= playerNum && charaTeamCtrs.All(x => x.isReady))
        {
            if (charaTeamCtrs.Count(x => x.playerCtr.choosedTeam == Team.Blue) != charaTeamCtrs.Count(x => x.playerCtr.choosedTeam == Team.Yellow))
                return;
            if (teleCountCoro != null)
                return;
            teleCountCoro = StartCoroutine(CountDownIE());


        }
    }


    internal void StopCount()
    {
        if (teleCountCoro == null)
            return;
        StopCoroutine(teleCountCoro);
        teleCountCoro = null;
    }

    private IEnumerator CountDownIE()
    {
        yield return new WaitForSeconds(countTime);
        canChangeTeam = false;
        teamSelectSfxCtr.PlayTele();
        yellowTeam = charaTeamCtrs.Where(x => x.playerCtr.choosedTeam == Team.Yellow).Select(x => (CharaType)x.playerCtr.playerConfig.CharaterIndex).ToArray();
        blueTeam = charaTeamCtrs.Where(x => x.playerCtr.choosedTeam == Team.Blue).Select(x => (CharaType)x.playerCtr.playerConfig.CharaterIndex).ToArray();
        yield return new WaitForSeconds(2);
        switch(playerNum)
        {
            case 2:
                SceneMgr.instance.LoadGame(playMode.Two);

                break;
            case 4:
                SceneMgr.instance.LoadGame(playMode.Four);
                break;
        }
        
    }
}
