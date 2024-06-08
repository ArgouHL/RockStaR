using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCtr : MonoBehaviour
{

    [SerializeField] private int gameTime;
    [SerializeField] private int time = 0;
    [SerializeField] TMP_Text timeText;
    void Start()
    {
        StartCoroutine(GameIE());
    }

    private IEnumerator GameIE()
    {
        ScoreSys.instance.Inst();

        LeanTween.delayedCall(2, () => JewelrySystem.instance.SpawnJewelry());
        time = gameTime;
        while (time >0)
        {
            time -= 1;
            timeText.text = time.ToString();
            yield return new WaitForSeconds(1);
        }
        GameStop();


    }

    private void GameStop()
    {
        JewelrySystem.instance.EndDespawn();
        PlayerConfigManager.instance.ChangeActionMap(InputType.UI);

        EndCountShow.instance.ShowEndCount();
    }
}
