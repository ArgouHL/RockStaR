using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : MonoBehaviour
{
    public static CharaterSelect instance;
   

    public PlayerCtr[] playerCtrs => FindObjectOfType<SelectInputConnect>().playerCtrs;


    private void Awake()
    {
        
        instance = this;

    }



    internal void ChangeShowChara(int playerIndex, int newIndex, int colorIndex)
    {
        PlayerConfigManager.instance.GetPlayerConfig(playerIndex).ChangeCharater(newIndex);
        CharaSwitcher.instance.SetPlayerSkin(playerIndex, newIndex);
        ChangeShowColor(playerIndex, colorIndex);
       // playerSkinManagment[playerIndex].ChangeSkin(charaterSettings[newIndex]);
        //charaterUICtr[playerIndex].ChangeColor(charaters[newIndex]);
    }

    internal void ChangeShowColor(int playerIndex, int newIndex)
    {
        PlayerConfigManager.instance.GetPlayerConfig(playerIndex).ChangeCharaterColor(newIndex);
        CharaSwitcher.instance.SetPlayerSkinColor(playerIndex,newIndex);

    }



    
}
