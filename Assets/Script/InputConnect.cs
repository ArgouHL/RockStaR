using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputConnect : MonoBehaviour
{
    [SerializeField] private PlayerCtr[] playerCtrs;
    [SerializeField] private bool test = false;
    private PlayerInputManager testPlayerInput;
    [SerializeField] private GameObject testPlayer;
    private List<PlayerConfig> playerConfigs = new List<PlayerConfig>();
    private PlayerSkinManagment[] playerSkinManagments;



    private void Start()
    {
        if (test)
            ConnectTestInput();
        else
            ConnectInputs();
    }

    private void ConnectInputs()
    {
        int index = PlayerConfigManager.instance.GetPlayerCount();
        Debug.Log("PlayerCount:" + index);

        for (int i = 0; i < index; i++)
        {
            Debug.Log("Connect times:" + i);
            var config = PlayerConfigManager.instance.GetPlayerConfig(i);
            playerCtrs[i].SetInput(config);
            playerCtrs[i].playerConfig.ChangeInputMap(InputType.player);

        }
        ApplySkin();
        PlayerConfigManager.instance.ChangeActionMap(InputType.player);
    }

    private void ApplySkin()
    {

        for (int i = 0; i < 4; i++)
        {

            CharaSwitcher.instance.SetPlayerSkinColor(i, PlayerConfigManager.instance.GetPlayerConfig(i).CharaterColorIndex);
            CharaSwitcher.instance.SetPlayerSkin(i, PlayerConfigManager.instance.GetPlayerConfig(i).CharaterIndex);

            //playerSkinManagments[i].ChangeSkin(CharaterSelect.instance.GetChara(PlayerConfigManager.instance.GetPlayerConfig(i).CharaterIndex));
        }



    }


    private void OnDisable()
    {
        if (test)
            testPlayerInput.onPlayerJoined -= PlayerJoin;
    }

    private void ConnectTestInput()
    {
        gameObject.AddComponent<PlayerConfigManager>();
        testPlayerInput = gameObject.AddComponent<PlayerInputManager>();
        testPlayerInput.playerPrefab = testPlayer;
        testPlayerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        testPlayerInput.onPlayerJoined += PlayerJoin;
    }

    public void PlayerJoin(PlayerInput playerInput)
    {
        Debug.Log("Player" + playerInput.playerIndex + " Joined");

        var config = new PlayerConfig(playerInput);

        playerInput.transform.SetParent(transform);


        playerCtrs[0].SetInput(config);
        playerCtrs[0].playerConfig.ChangeInputMap(InputType.player);
        PlayerConfigManager.instance.AddConfigTest(config);


    }
}
