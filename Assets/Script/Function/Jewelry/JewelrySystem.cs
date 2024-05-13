using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelrySystem : MonoBehaviour
{
    private JewelryCtr jewelryCtr;
    public static JewelrySystem instance;
    [SerializeField] private Transform spwanPos;
    [SerializeField] internal bool modeA;
    [SerializeField] internal bool modeB;



    internal bool instFinish = false;
    [SerializeField] internal Material grayJew;
    [SerializeField] internal Material blueJew;
    [SerializeField] internal Material yellowJew;

    [SerializeField] internal float totalTime = 2f;
    [SerializeField] internal int countTimes = 5;
    private Coroutine GetPountCountDown;

    private void Awake()
    {
        instance = this;
        jewelryCtr = GetComponentInChildren<JewelryCtr>();
        instFinish = true;

    }

    private Team nowTeam = Team.None;
    internal Team NowTeam()
    {
        return nowTeam;
    }

    internal void ChangeTeam(Team team)
    {
        if (JewelrySystem.instance.NowTeam() == team)
            return;
        nowTeam = team;
        jewelryCtr.SetTeam(team);
        if (GetPountCountDown != null)
            StopCoroutine(GetPountCountDown);
        GetPountCountDown = StartCoroutine(GetPountIE(countWait: totalTime/ countTimes  , _totalTime: totalTime));
    }

    private IEnumerator GetPountIE(float countWait, float _totalTime)
    {
        float time = 0;
        int count = 0;
        
        while (time < _totalTime)
        {
            count++;
            JewelryCounterUI.instance.ShowCount(nowTeam,count);
            ScoreSys.instance.AddScore(nowTeam, 1);
            yield return new WaitForSeconds(countWait);
            time += countWait;
        }
        ScoreSys.instance.AddScore(nowTeam, 5);
        JewelryCounterUI.instance.ShowEnd(nowTeam);
        nowTeam = Team.None;
        jewelryCtr.SetTeam(Team.None);
        GetPountCountDown = null;
    }

   

    //private void Start()
    //{
    //    ReSpawnJewelry();
    //}
    internal void ReSpawnJewelry()
    {
        HolesSys.instance.Shield();

        if (!modeB)
        {
            jewelryCtr.Inst();
            return;
        }


        jewelryCtr.Stop();
        jewelryCtr.Reset(spwanPos.position);

    }
    internal void SpawnJewelry()
    {
        if (HolesSys.instance!=null)
        HolesSys.instance.Shield();
        jewelryCtr.Reset(spwanPos.position);
    }
}
