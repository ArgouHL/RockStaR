using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndCountShow : MonoBehaviour
{
    public static EndCountShow instance;
    [SerializeField] private CanvasGroup backgroundCanvas;
    [SerializeField] private CanvasGroup bluePaperCanvas;
    [SerializeField] private CanvasGroup yellowPaperCanvas;
    [SerializeField] private Transform bluePaper;
    [SerializeField] private Transform yellowPaper;

    [SerializeField] private TMP_Text yellowWinLose;
    [SerializeField] private TMP_Text blueWinLose;
    [SerializeField] private TMP_Text yellowNames;
    [SerializeField] private TMP_Text blueNames;

    [SerializeField] private TMP_Text blueTeamScoreText;
    [SerializeField] private TMP_Text yellowTeamScoreText;
    [SerializeField] private float scoreSpeed = 5;
    [SerializeField] private Image[] yellowCharaImgs;
    [SerializeField] private Image[] blueCharaImgs;
    [SerializeField] private CanvasGroup replayBtn;
    [SerializeField] private CanvasGroup menuBtn;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        backgroundCanvas.alpha = 0;
        bluePaperCanvas.alpha = 0;
        yellowPaperCanvas.alpha = 0;
    }


    public void ShowEndCount()
    {
        PlayerConfigManager.instance.ChangeActionMap(InputType.UI);
        BlackFade(PaperFade);
        SetWinLose();



    }

    private void SetWinLose()
    {
        if (CharaTeamManger.yellowTeam == null)
        {
            CharaTeamManger.yellowTeam = new CharaType[] { CharaType.Raccoon, CharaType.Raccoon };
        }
        if (CharaTeamManger.blueTeam == null)
        {
            CharaTeamManger.blueTeam = new CharaType[] { CharaType.Fox, CharaType.Fox };
        }
        yellowNames.text = CharaTeamManger.yellowTeam[0] + " & " + CharaTeamManger.yellowTeam[1];
        blueNames.text = CharaTeamManger.blueTeam[0] + " & " + CharaTeamManger.yellowTeam[1];





        if (ScoreSys.yellowScore == ScoreSys.blueScore)
        {
            yellowWinLose.text = "Draw";
            blueWinLose.text = "Draw";
            for (int i = 0; i < 2; i++)
            {
                yellowCharaImgs[i].sprite = SkinSystem.instance.GetCharaData(CharaTeamManger.yellowTeam[i]).winPic;
                blueCharaImgs[i].sprite = SkinSystem.instance.GetCharaData(CharaTeamManger.blueTeam[i]).winPic;
            }



        }
        else
        {
            bool yellowWin = ScoreSys.yellowScore > ScoreSys.blueScore;
            yellowWinLose.text = yellowWin ? "Win" : "Lose";
            blueWinLose.text = yellowWin ? "Lose" : "Win";
            for (int i = 0; i < 2; i++)
            {
                var yData = SkinSystem.instance.GetCharaData(CharaTeamManger.yellowTeam[i]);
                yellowCharaImgs[i].sprite = yellowWin ? yData.winPic : yData.losePic;
                var bData = SkinSystem.instance.GetCharaData(CharaTeamManger.blueTeam[i]);
                blueCharaImgs[i].sprite = yellowWin ? bData.losePic : bData.winPic;
            }
        }


    }

    private void BlackFade(Action action)
    {
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => backgroundCanvas.alpha = val);
        LeanTween.delayedCall(1f, action);

    }

    private void PaperFade()
    {
        LeanTween.value(0, 1, 0.3f).setOnUpdate((float val) =>
        {

            yellowPaperCanvas.alpha = val;

        });
        LeanTween.value(3, 1, 0.3f).setOnUpdate((float val) =>
        {

            yellowPaper.localScale = Vector3.one * val;

        });

        LeanTween.delayedCall(0.3f, () =>
        {

            LeanTween.value(0, 1, 0.3f).setOnUpdate((float val) =>
            {
                bluePaperCanvas.alpha = val;


            });
            LeanTween.value(3, 1, 0.3f).setOnUpdate((float val) =>
            {
                bluePaper.localScale = Vector3.one * val;


            });
        });
        LeanTween.delayedCall(1f, ScoreShow);
    }

    private void ScoreShow()
    {

        StartCoroutine(ShowScoreAndBtns());

    }

    private IEnumerator ShowScoreRise(float targetScore, TMP_Text scoreText)
    {
        float _score = 0;
        while (_score < targetScore)
        {
            _score += Time.deltaTime * scoreSpeed;
            scoreText.text = Mathf.RoundToInt(_score).ToString();
            yield return null;
        }

    }

    private IEnumerator ShowScoreAndBtns()
    {
        Coroutine showYellow = StartCoroutine(ShowScoreRise(200, yellowTeamScoreText));
        Coroutine showblue = StartCoroutine(ShowScoreRise(200, blueTeamScoreText));
        //Coroutine showYellow = StartCoroutine(ShowScoreRise(ScoreSys.yellowScore, yellowTeamScoreText));
        //Coroutine showblue = StartCoroutine(ShowScoreRise(ScoreSys.blueScore, blueTeamScoreText));
        yield return showYellow;
        yield return showblue;
        replayBtn.blocksRaycasts = true;
        replayBtn.interactable = true;
        menuBtn.blocksRaycasts = true;
        menuBtn.interactable = true;
        LeanTween.value(0, 1, 0.3f).setOnUpdate((float val) => replayBtn.alpha = val);
        LeanTween.delayedCall(0.3f, ()=>LeanTween.value(0, 1, 0.3f).setOnUpdate((float val) => menuBtn.alpha = val));
    }

}
