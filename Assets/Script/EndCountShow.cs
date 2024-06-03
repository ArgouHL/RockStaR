using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndCountShow : MonoBehaviour
{
    public static EndCountShow instance;
    [SerializeField] private CanvasGroup backgroundCanvas;
    [SerializeField] private CanvasGroup teamCanvas;
    [SerializeField] private TMP_Text blueTeamScoreText;
    [SerializeField] private TMP_Text yellowTeamScoreText;

    private void Awake()
    {
        instance = this;
    }

    internal void TestShow()
    {
        backgroundCanvas.alpha = 1;
        teamCanvas.alpha = 1;
        blueTeamScoreText.text = ScoreSys.blueScore.ToString();
        yellowTeamScoreText.text = ScoreSys.yellowScore.ToString();
    }
}
