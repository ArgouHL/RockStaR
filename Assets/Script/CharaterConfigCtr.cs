using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharaterConfigCtr : MonoBehaviour
{
    private PlayerConfig playerConfig;
    private PlayerInput input;
    private InputActionAsset inputAsset;

    [SerializeField] private InputActionMap selectInput;
    private void OnEnable()
    {
        selectInput.FindAction("PreChara").performed += SelectPreChara;
        selectInput.FindAction("NextChara").performed += SelectNextChara;
        selectInput.FindAction("PreColor").performed += SelectPreColor;
        selectInput.FindAction("NextColor").performed += SelectNextColor;
        selectInput.FindAction("Confirm").performed += Confirm;
        selectInput.FindAction("Cancel").performed += Cancel;
    }


    private void OnDisable()
    {
        selectInput.FindAction("PreChara").performed -= SelectPreChara;
        selectInput.FindAction("NextChara").performed -= SelectNextChara;
        selectInput.FindAction("PreColor").performed -= SelectPreColor;
        selectInput.FindAction("NextColor").performed -= SelectNextColor;
        selectInput.FindAction("Confirm").performed -= Confirm;
        selectInput.FindAction("Cancel").performed -= Cancel;
    }

    private void SelectNextChara(InputAction.CallbackContext obj)
    {
        Debug.Log("Change1");
        // ChangeCharaColor(1);
        ChangeChara(1);
    }

    private void SelectPreChara(InputAction.CallbackContext obj)
    {
        Debug.Log("Change-");
        // ChangeCharaColor(-1);
        ChangeChara(-1);

    }
    private void ChangeChara(int value)
    {
        //Debug.Log("Change");
        var config = PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex);
        //if (PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex).IsReady)
        //    return;
        int newIndex = config.CharaterIndex + value;


        if (newIndex < 0)
        {
            newIndex = 3;
        }
        else if (newIndex > 3)
        {
            newIndex = 0;
        }
       
        int colorIndex = 0;

        while (CharaSwitcher.instance.IsColorUsing(newIndex, colorIndex))
        {

            colorIndex++;
            if (colorIndex >3)
            {
                colorIndex = 0;
            }
            Debug.Log("colorIndex " + colorIndex);
        }
        config.CharaterColorIndex = colorIndex;
        config.CharaterIndex = newIndex;
        CharaterSelect.instance.ChangeShowChara(input.playerIndex, newIndex, colorIndex);
       
    }

    private void SelectNextColor(InputAction.CallbackContext obj)
    {
        Debug.Log("ChangeNextColo");
        ChangeCharaColor(1);
    }

    private void SelectPreColor(InputAction.CallbackContext obj)
    {
        Debug.Log("ChangePreColor");
        ChangeCharaColor(-1);
    }

    private void ChangeCharaColor(int value)
    {
        Debug.Log("ChangeColor");
        var config = PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex);
        //if (PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex).IsReady)
        //    return;
        int newIndex = config.CharaterColorIndex + value;
        CharaType charaType = (CharaType)config.CharaterIndex;
        int colorCount = SkinSystem.instance.GetCharaData(charaType).materials.Length;
        if (newIndex < 0)
        {
            newIndex = colorCount - 1;
        }
        else if (newIndex >= colorCount) //CharaterSelect.instance.charaterColorSettingsDict[t].Count - 1)
        {
            newIndex = 0;
        }
        while (CharaSwitcher.instance.IsColorUsing(config.CharaterIndex, newIndex))
        {

            newIndex+=value;
            if (newIndex < 0)
            {
                newIndex = colorCount - 1;
            }
            else if (newIndex >= colorCount) //CharaterSelect.instance.charaterColorSettingsDict[t].Count - 1)
            {
                newIndex = 0;
            }
        }
      
        config.CharaterColorIndex = newIndex;
        CharaterSelect.instance.ChangeShowColor(input.playerIndex, newIndex);
    }

    

    private void Confirm(InputAction.CallbackContext obj)
    {
        CharaTeamManger.instance.ConfirmTeam(input.playerIndex);
        //CharaterSelect.instance.ShowConfirm(input.playerIndex);
        // PlayerConfigManager.instance.ReadyPlayer(input.playerIndex);


    }
    private void Cancel(InputAction.CallbackContext obj)
    {
        CharaTeamManger.instance.CancelTeam(input.playerIndex);
        //CharaterSelect.instance.ShowConfirm(input.playerIndex);
        // PlayerConfigManager.instance.Cancel(input.playerIndex);

    }

    internal void SetConfig(PlayerConfig config)
    {
        playerConfig = config;
        input = config.Input;
        selectInput = config.selectInput;
        config.ChangeInputMap(InputType.selectChara);
        PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex).IsReady = false;
    }
}
