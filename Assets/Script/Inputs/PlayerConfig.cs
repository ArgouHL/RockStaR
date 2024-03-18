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

    public PlayerConfig(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;
    }

    public void ChangeCharater(int val)
    {
        CharaterIndex = val;
    }
}
