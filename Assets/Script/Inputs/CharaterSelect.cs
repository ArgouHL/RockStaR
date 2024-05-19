using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : MonoBehaviour
{
    public static CharaterSelect instance;
    [SerializeField] private CharaterSettingObj[] charaters;
    //[SerializeField] internal CharaterSetting[] charaterSettings;
   // public CharaterUICtr[] charaterUICtr;

    public PlayerCtr[] playerCtrs => FindObjectOfType<SelectInputConnect>().playerCtrs;
    private PlayerSkinManagment[] playerSkinManagment;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //charaterSettings = charaters.Select(s => s.charaterSetting).ToArray();

        playerSkinManagment = new PlayerSkinManagment[playerCtrs.Length];
        for (int i = 0; i < 4; i++)
        {
            playerSkinManagment[i] = playerCtrs[i].GetComponent<PlayerSkinManagment>();
        }
    }

    private void OnEnable()
    {
        // PlayerConfigManager.instance.playerJoin += ShowChara;
    }

    private void OnDisable()
    {
        // PlayerConfigManager.instance.playerJoin -= ShowChara;
    }

    //internal void ChangeShowChara(int playerIndex, int newIndex)
    //{
    //    PlayerConfigManager.instance.GetPlayerConfig(playerIndex).ChangeCharater(newIndex);
    //    playerSkinManagment[playerIndex].ChangeSkin(charaterSettings[newIndex]);
    //    //charaterUICtr[playerIndex].ChangeColor(charaters[newIndex]);
    //}



    internal void ShowConfirm(int playerIndex)
    {
     //   charaterUICtr[playerIndex].ShowConfirm();
    }

    //internal CharaterSetting GetChara(int i)
    //{
    //    return charaterSettings[i];
    //}
}
