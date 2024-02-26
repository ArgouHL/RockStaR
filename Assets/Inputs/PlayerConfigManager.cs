using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    private List<PlayerConfig> playerConfigs;
  
    const int MaxPlayerCount = 4;

    public static PlayerConfigManager instance { get; private set; }


    public delegate void PlayerJoinEvent(int playerIndex);
    public PlayerJoinEvent playerJoin;

    private void Awake()
    {
        if (instance != null)
        {

            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            playerInputManager = GetComponent<PlayerInputManager>();
            playerConfigs = new List<PlayerConfig>();
        }

    }

    public void ChangeActionMap(InputType inputType)
    {
        foreach (var config in playerConfigs)
        {
            var input = config.Input;
            switch (inputType)
            {
                case InputType.player:
                    input.defaultActionMap = "Player";
                    break;
                case InputType.selectChara:
                    input.defaultActionMap = "SelectChara";
                    break;
            }
        }
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += PlayerJoin;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= PlayerJoin;
    }
    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if ( playerConfigs.All(p => p.IsReady))
        {
            AllReady();
        }
    }

    private void AllReady()
    {
        SceneManager.LoadScene(1);
    }


    public void PlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Player" + playerInput.playerIndex + " Joined");
        if (!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerConfigs.Add(new PlayerConfig(playerInput));
            playerInput.transform.SetParent(transform);

            playerJoin.Invoke(playerConfigs.Count - 1);
        }

    }

    public PlayerConfig GetPlayerConfig(int index)
    {
        return playerConfigs[index];
    }

    internal int GetPlayerCount()
    {
        return playerConfigs.Count;
    }


}

public enum InputType { player, selectChara }