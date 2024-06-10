using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    [SerializeField]
    private List<PlayerConfig> playerConfigs;
    internal List<PlayerConfig> PlayerConfigs()
    {
        return playerConfigs;
    }
    const int MaxPlayerCount = 4;

    public static PlayerConfigManager instance { get; private set; }


    //public delegate void PlayerJoinEvent(int playerIndex);
    //public PlayerJoinEvent playerJoin;

    private void Awake()
    {
        playerConfigs = new List<PlayerConfig>();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
            playerInputManager = GetComponent<PlayerInputManager>();

        }

    }

    

    public void ChangeActionMap(InputType inputType)
    {
        foreach (var config in playerConfigs)
        {

            switch (inputType)
            {
                case InputType.player:
                    config.ChangeInputMap(InputType.player);
                    break;
                case InputType.selectChara:
                    config.ChangeInputMap(InputType.selectChara);
                    break;
                case InputType.UI:
                    config.ChangeInputMap(InputType.UI);
                    break;
            }
        }
    }



    internal void Destroy()
    {
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        SceneMgr.OnSceneEnd += Destroy;
        playerInputManager.onPlayerJoined += PlayerJoin;
       
    }

    private void OnDisable()
    {
        SceneMgr.OnSceneEnd -= Destroy;
        playerInputManager.onPlayerJoined -= PlayerJoin;
        
    }




    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.All(p => p.IsReady))
        {
            AllReady();
        }
    }

    public void AllReady()
    {
        SceneManager.LoadScene(1);
    }


    public void PlayerJoin(PlayerInput playerInput)
    {

        Debug.Log("Player" + playerInput.playerIndex + " Joined");
        if (!playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            var config = new PlayerConfig(playerInput);
            playerConfigs.Add(config);
            playerInput.GetComponent<CharaterConfigCtr>().SetConfig(config);
            playerInput.transform.SetParent(transform);

            // playerJoin.Invoke(playerConfigs.Count - 1);
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

    internal void Cancel(int playerIndex)
    {
        throw new NotImplementedException();
    }


    internal void AddConfigTest(PlayerConfig config)
    {
        playerConfigs.Add(config);
    }



    
}

public enum InputType { player, selectChara ,UI}