using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelrySystem : MonoBehaviour
{
    private JewelryCtr jewelryCtr;
    public static JewelrySystem instance;
    [SerializeField] private float sceneRadius = 13;
    [SerializeField] internal bool modeA;
    [SerializeField] internal bool modeB;



    internal bool instFinish = false;


    [SerializeField] internal float totalTime = 2f;
    [SerializeField] internal int countTimes = 5;
    [SerializeField] internal float coolDownTime = 3f;
    private Coroutine GetPountCountDownCoro;
    private Coroutine WarningCoro;

    private int count = 0;
    float countTime = 0;

    private void Awake()
    {
        instance = this;
        jewelryCtr = GetComponentInChildren<JewelryCtr>();
        instFinish = true;

    }

    private Team nowTeam;
    internal Team NowTeam()
    {

        return nowTeam;
    }

    internal void ChangeTeam(Team team)
    {
        if (JewelrySystem.instance.NowTeam() == team)
            return;
        nowTeam = team;
        jewelryCtr.crystalSfxControl.PlayCrystalChangeTeamSfx();
        jewelryCtr.SetTeamVisual(team);
        jewelryCtr.countEffect.StartEffectInTime(team, countTime);
        jewelryCtr.absorbEnergyEffect.StartEffect(team);
        //JewelryCounterUI.instance.ShowCount(nowTeam, count);
        //if (GetPountCountDown != null)
        //    StopCoroutine(GetPountCountDown);
        //GetPountCountDown = StartCoroutine(GetPountIE(countWait: totalTime/ countTimes  , _totalTime: totalTime));
    }

    private IEnumerator GetPountIE(float countWait, float _totalTime)
    {
        countTime = 0;
        count = 0;
        //  JewelryCounterUI.instance.ShowEmpty();
        WarningCoro=StartCoroutine(CrystalWorning(_totalTime + countWait * 0.75f));
        while (countTime < _totalTime)
        {
            yield return new WaitForSeconds(countWait);
            count++;
            //JewelryCounterUI.instance.ShowCount(nowTeam, count);
            countTime += countWait;
        }
        yield return new WaitForSeconds(countWait);
        count++;
        ScoreSys.instance.AddScore(nowTeam, 1);
        jewelryCtr.disappearEffect.StartEffect(nowTeam);
        jewelryCtr.crystalSfxControl.PlayCrystalDisappearSfx();
        //  JewelryCounterUI.instance.ShowEnd(nowTeam);
        nowTeam = Team.None;
        jewelryCtr.SetTeamVisual(Team.None);
        jewelryCtr.Stop();
        LeanTween.delayedCall(coolDownTime, () => SpawnJewelry());
        GetPountCountDownCoro = null;

    }

    private IEnumerator CrystalWorning(float _totalTime)
    {
        float startPeriod = 0.7f;
        float endPeriod = 0.01f;
        float elapsedTime = 0f;

        while (elapsedTime < _totalTime)
        {
            jewelryCtr.crystalSfxControl.PlayCrystalWrongingSfx();



            // Calculate the current period based on elapsed time
            float t = elapsedTime / _totalTime; // Normalized time (0 to 1)
            float currentPeriod = Mathf.Lerp(startPeriod, endPeriod, t);

            yield return new WaitForSeconds(currentPeriod);
            elapsedTime += currentPeriod;
        }

        WarningCoro = null;
    }

    internal void SpawnJewelry()
    {
        jewelryCtr.Appear(sceneRadius);
        if (GetPountCountDownCoro != null)
            StopCoroutine(GetPountCountDownCoro);
        GetPountCountDownCoro = StartCoroutine(GetPountIE(countWait: totalTime / countTimes, _totalTime: totalTime));
    }


    internal void EndDespawn()
    {
        if (GetPountCountDownCoro != null)
            StopCoroutine(GetPountCountDownCoro);
        if(WarningCoro!=null) 
            StopCoroutine(WarningCoro);
        jewelryCtr.Stop();
    }
}

[Serializable]
public struct JewLight
{
    public Color color;
    public float intensity;
}
