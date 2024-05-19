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

    //private void Awake()
    //{



    //}

    private void OnEnable()
    {
        selectInput.FindAction("Previous").performed += SelectPrevious;
        selectInput.FindAction("Next").performed += Next;
        selectInput.FindAction("Confirm").performed += Confirm;
    }


    private void OnDisable()
    {
        selectInput.FindAction("Previous").performed -= SelectPrevious;
        selectInput.FindAction("Next").performed -= Next;
        selectInput.FindAction("Confirm").performed -= Confirm;
    }

    private void Next(InputAction.CallbackContext obj)
    {
        Debug.Log("Change1");
        ChangeCharaColor(1);
        // ChangeChara(1);
    }



    private void SelectPrevious(InputAction.CallbackContext obj)
    {
        Debug.Log("Change-");
        ChangeCharaColor(-1);
        //ChangeChara(-1);

    }
    private void ChangeCharaColor(int value)
    {
        throw new NotImplementedException();
    }

    //private void ChangeChara(int value)
    //{
    //    Debug.Log("Change");
    //    var config = PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex);
    //    if (PlayerConfigManager.instance.GetPlayerConfig(input.playerIndex).IsReady)
    //        return;
    //    int newIndex = config.CharaterIndex + value;
    //    if (newIndex < 0)
    //    {
    //        newIndex = CharaterSelect.instance.charaterSettings.Length - 1;
    //    }
    //    else if (newIndex > CharaterSelect.instance.charaterSettings.Length - 1)
    //    {
    //        newIndex = 0;
    //    }
    //    config.CharaterIndex = newIndex;
    //    CharaterSelect.instance.ChangeShowChara(input.playerIndex, newIndex);
    //}

    private void Confirm(InputAction.CallbackContext obj)
    {

        CharaterSelect.instance.ShowConfirm(input.playerIndex);
        PlayerConfigManager.instance.ReadyPlayer(input.playerIndex);

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
