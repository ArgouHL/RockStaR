using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSys : MonoBehaviour
{
    public static ScoreSys instance;
    [SerializeField] private TMP_Text redScoreTMP;
    [SerializeField] private TMP_Text blueScoreTMP;
    private float redScore;
    private float blueScore;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Inst();
        UpdateScore();
    }

    private void UpdateScore()
    {
        redScoreTMP.text= redScore.ToString();
        blueScoreTMP.text = blueScore.ToString();
    }

    private void Inst()
    {
        redScore=0;
        blueScore = 0;
    }


   internal void AddScore(Team team)
    {
        switch(team)
        {
            case Team.Blue:
                blueScore++;
                break;
            case Team.Red:
                redScore++;
                break;
        }
        UpdateScore();
    }
}


