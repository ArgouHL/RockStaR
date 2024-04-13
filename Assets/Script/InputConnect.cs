using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputConnect : MonoBehaviour
{
    [SerializeField] private PlayerCtr[] playerCtrs;
    [SerializeField] private bool test = false;
    private PlayerInputManager testPlayerInput;
    [SerializeField] private GameObject testPlayer;
    private List<PlayerConfig> playerConfigs=new List<PlayerConfig>();
    private PlayerSkinManagment[] playerSkinManagments;

    private void Awake()
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
            Debug.Log("Connect times:" + i + 1);
            var config = PlayerConfigManager.instance.GetPlayerConfig(i);
            playerCtrs[i].SetInput(config);
        }
        ApplySkin();







        PlayerConfigManager.instance.ChangeActionMap(InputType.player);




    }

    private void ApplySkin()
    {
        playerSkinManagments = new PlayerSkinManagment[playerCtrs.Length];
        for (int i = 0; i < 4; i++)
        {
            playerSkinManagments[i] = playerCtrs[i].GetComponent<PlayerSkinManagment>();

            playerSkinManagments[i].ChangeSkin(CharaterSelect.instance.GetChara(PlayerConfigManager.instance.GetPlayerConfig(i).CharaterIndex));
        }



    }

    private void OnEnable()
    {


    }

    private void OnDisable()
    {
        if (test)
            testPlayerInput.onPlayerJoined -= PlayerJoin;
    }

    private void ConnectTestInput()
    {
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



    }
}
