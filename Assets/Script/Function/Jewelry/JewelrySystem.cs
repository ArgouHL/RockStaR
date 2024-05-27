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
    private Coroutine GetPountCountDown;

    private int count = 0;

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
        jewelryCtr.crystalSfxControl.PlayCrystalChangeTeamSfx();
        jewelryCtr.SetTeam(team);
        JewelryCounterUI.instance.ShowCount(nowTeam, count);
        //if (GetPountCountDown != null)
        //    StopCoroutine(GetPountCountDown);
        //GetPountCountDown = StartCoroutine(GetPountIE(countWait: totalTime/ countTimes  , _totalTime: totalTime));
    }

    private IEnumerator GetPountIE(float countWait, float _totalTime)
    {
        float time = 0;
        count = 0;
        JewelryCounterUI.instance.ShowEmpty();
        StartCoroutine(CrystalWorning(_totalTime+ countWait*0.75f));
        while (time < _totalTime)
        {
            yield return new WaitForSeconds(countWait);
            count++;
            JewelryCounterUI.instance.ShowCount(nowTeam, count);
            time += countWait;
        }
        yield return new WaitForSeconds(countWait);
        count++;
        ScoreSys.instance.AddScore(nowTeam, 1);

        JewelryCounterUI.instance.ShowEnd(nowTeam);
        nowTeam = Team.None;
        jewelryCtr.SetTeam(Team.None);
        jewelryCtr.Stop();
        LeanTween.delayedCall(coolDownTime, () => SpawnJewelry());
        GetPountCountDown = null;

    }

    private IEnumerator CrystalWorning(float _totalTime)
    {
        float startPeriod = 0.7f;
        float endPeriod = 0.01f;
        float elapsedTime = 0f;

        while (elapsedTime < _totalTime)
        {
            jewelryCtr.crystalSfxControl.PlayCrystalWrongingSfx();

            Debug.Log("Wronging");

            // Calculate the current period based on elapsed time
            float t = elapsedTime / _totalTime; // Normalized time (0 to 1)
            float currentPeriod = Mathf.Lerp(startPeriod, endPeriod, t);

            yield return new WaitForSeconds(currentPeriod);
            elapsedTime += currentPeriod;
        }
       

    }

    internal void SpawnJewelry()
    {
        jewelryCtr.Appear(sceneRadius);
        if (GetPountCountDown != null)
            StopCoroutine(GetPountCountDown);
        GetPountCountDown = StartCoroutine(GetPountIE(countWait: totalTime / countTimes, _totalTime: totalTime));
    }
}

[Serializable]
public struct JewLight
{
    public Color color;
    public float intensity;
}
