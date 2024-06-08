using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSys : MonoBehaviour
{
    public static ScoreSys instance;
    [SerializeField] private TMP_Text yellowScoreTMP;
    [SerializeField] private TMP_Text blueScoreTMP;
    private CanvasGroup canvasGroup;
    internal static int yellowScore;
    internal static int blueScore;
    private void Awake()
    {
        instance = this;
    }


    private void UpdateScore()
    {
        yellowScoreTMP.text= yellowScore.ToString();
        blueScoreTMP.text = blueScore.ToString();
    }

    internal void Inst()
    {
        yellowScore=0;
        blueScore = 0;
        UpdateScore();
    }


   internal void AddScore(Team team,int score)
    {
        switch(team)
        {
            case Team.Blue:
                blueScore+= score ;
                break;
            case Team.Yellow:
                yellowScore += score;
                break;
        }
        UpdateScore();
    }

    internal void UnShow()
    {
        canvasGroup.alpha = 0;
    }
}


