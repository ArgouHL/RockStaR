using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtr : MonoBehaviour
{

    [SerializeField] private float gameTime;
    [SerializeField] private float time = 0;

    void Start()
    {
        StartCoroutine(GameIE());
    }

    private IEnumerator GameIE()
    {
        ScoreSys.instance.Inst();

        LeanTween.delayedCall(2, () => JewelrySystem.instance.SpawnJewelry());
        time = 0;
        while (time < gameTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        GameStop();


    }

    private void GameStop()
    {
        JewelrySystem.instance.EndDespawn();
        PlayerConfigManager.instance.ChangeActionMap(InputType.UI);

        EndCountShow.instance.TestShow();
    }
}
