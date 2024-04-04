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
    public InputManager inputManager;

    private InputActionAsset inputAsset;
    internal InputActionMap selectInput;
    internal InputActionMap gameInput;


    public PlayerConfig(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
        inputAsset = playerInput.actions;
        selectInput = inputAsset.FindActionMap("SelectChara");
        gameInput = inputAsset.FindActionMap("Player");
    }

    public void ChangeCharater(int val)
    {
        CharaterIndex = val;
    }

    internal void ChangeInputMap(InputType inputType)
    {
        selectInput.Disable();
        gameInput.Disable();
        switch (inputType)
        {
            case InputType.selectChara:
                selectInput.Enable();
                break;
            case InputType.player:
                gameInput.Enable();
                break;
        }
    }
}
