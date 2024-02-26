using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> startingPoints;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        instance = this;
        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);

        Transform playerParent = player.transform;
        playerParent.position = startingPoints[players.Count - 1].position;


    }


    public void PlayerSpawn(int count)
    {
        for(int i=0;i> count;i++)
        {
            playerInputManager.JoinPlayer();
        }
       
    }



}


