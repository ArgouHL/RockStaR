using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharaterSelect : MonoBehaviour
{
    public static CharaterSelect instance;
    [SerializeField] internal CharaterColorSettingObj[] charaterSettings;

  
    [SerializeField] internal Dictionary<CharaType, List<CharaterColorSetting>> charaterColorSettingsDict;
    // public CharaterUICtr[] charaterUICtr;

    public PlayerCtr[] playerCtrs => FindObjectOfType<SelectInputConnect>().playerCtrs;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        charaterColorSettingsDict = new Dictionary<CharaType, List<CharaterColorSetting>>();
        foreach(var charaterSetting in charaterSettings)
        {
            CharaType _type = charaterSetting.charaterColorSetting.charaType;
            
            if(!charaterColorSettingsDict.ContainsKey(_type))
            {
                charaterColorSettingsDict.Add(_type, new List<CharaterColorSetting>()); 
            }

            charaterColorSettingsDict[_type].Add(charaterSetting.charaterColorSetting);
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

    internal void ChangeShowChara(int playerIndex, int newIndex)
    {
        PlayerConfigManager.instance.GetPlayerConfig(playerIndex).ChangeCharater(newIndex);
       // playerSkinManagment[playerIndex].ChangeSkin(charaterSettings[newIndex]);
        //charaterUICtr[playerIndex].ChangeColor(charaters[newIndex]);
    }

    internal void ChangeShowColor(int playerIndex, int newIndex)
    {
        PlayerConfigManager.instance.GetPlayerConfig(playerIndex).ChangeCharaterColor(newIndex);
        SkinSystem.instance.GetPlayerSkinManagment(playerIndex).ChangeCharaterColor(newIndex);
    }


    internal void ShowConfirm(int playerIndex)
    {
     //   charaterUICtr[playerIndex].ShowConfirm();
    }

    //internal CharaterSetting GetChara(int i)
    //{
    //    return charaterSettings[i];
    //}
}
