using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamChooseCtr : MonoBehaviour
{
    [SerializeField] private TeamChooseDetecter blueField;
    [SerializeField] private TeamChooseDetecter redField;
    [SerializeField] private int targetNumber = 1;

    private Coroutine gameStartCountdown;
    [SerializeField] private int countDownTime;

    private void Start()
    {
        blueField.GetMembersOnField(targetNumber);
        redField.GetMembersOnField(targetNumber);
    }

    internal void Check()
    {
        int blueCount = blueField.GetMembersOnField(targetNumber);
        int redCount = redField.GetMembersOnField(targetNumber);
        Debug.Log(blueCount + redCount + "/" + targetNumber * 2);
        if (blueCount < targetNumber || redCount < targetNumber)
        {
            Debug.Log("LessThantargetNumber");
            if (gameStartCountdown != null)
                StopCoroutine(gameStartCountdown);
        }

        else if (blueCount == targetNumber && redCount == targetNumber)
        {
            if (gameStartCountdown == null)
                gameStartCountdown = StartCoroutine(CountDownIE(countDownTime));
        }



    }

    private IEnumerator CountDownIE(int countDownTime)
    {
        Debug.Log("StartCountDownIE");
        float time = 0;
        while(time< countDownTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Debug.Log("GameStart");
        SceneManager.LoadScene(1);

    }
}
