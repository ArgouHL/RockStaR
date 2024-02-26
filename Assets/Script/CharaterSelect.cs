using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : MonoBehaviour
{
    public static CharaterSelect instance;
    public Color[] charaters;
    public CharaterUICtr[] charaterUICtr;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        PlayerConfigManager.instance.playerJoin += ShowChara;
    }

    private void OnDisable()
    {
        PlayerConfigManager.instance.playerJoin -= ShowChara;
    }

    internal void ChangeShowChara(int playerIndex, int newIndex)
    {
        charaterUICtr[playerIndex].ChangeColor(charaters[newIndex]);
    }

    internal void ShowChara(int playerIndex)
    {
        charaterUICtr[playerIndex].ShowChara();
    }

    internal void ShowConfirm(int playerIndex)
    {
        charaterUICtr[playerIndex].ShowConfirm();
    }
}
