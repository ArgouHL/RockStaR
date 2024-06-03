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
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);

    }
}
