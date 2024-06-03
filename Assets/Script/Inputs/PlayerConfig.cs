using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfig
{

    public PlayerInput Input;
    public int PlayerIndex;
    public bool IsReady;
    public int CharaterIndex = 0;
    public int CharaterColorIndex = 0;
    public InputManager inputManager;

    private InputActionAsset inputAsset;
    internal InputActionMap selectInput;
    internal InputActionMap gameInput;
    internal InputActionMap uiInput;

    internal Team PlayerTeam;

    public PlayerConfig(PlayerInput playerInput)
    {
      
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
        inputAsset = playerInput.actions;
        selectInput = inputAsset.FindActionMap("SelectChara");
        gameInput = inputAsset.FindActionMap("Player");
        uiInput = inputAsset.FindActionMap("UI");
        ChangeCharater(PlayerIndex);
    }

    public void ChangeCharater(int val)
    {
        CharaterIndex = val;
    }

    public void ChangeCharaterColor(int val)
    {
        CharaterColorIndex = val;
    }

    internal void ChangeInputMap(InputType inputType)
    {
        Debug.Log(inputType);
        selectInput.Disable();
        gameInput.Disable();
        uiInput.Disable();
        switch (inputType)
        {
            case InputType.selectChara:
                selectInput.Enable();
                gameInput.Enable();
                break;
            case InputType.player:
                gameInput.Enable();
                break;
            case InputType.UI:
                uiInput.Enable();
                break;
        }
    }

    public void SetPlayerTeam(Team team)
    {
        PlayerTeam = team;

    }
}
