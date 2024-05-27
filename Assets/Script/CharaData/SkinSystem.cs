using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSystem : MonoBehaviour
{
    public static SkinSystem instance;

   [SerializeField] private Dictionary<int,PlayerSkinManagment> playerSkinManagments;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerSkinManagments = new Dictionary<int, PlayerSkinManagment>();
    }

    internal void AddPlayerSkinManagment(int playerIndex, PlayerSkinManagment playerSkinManagment)
    {
        playerSkinManagments.Add(playerIndex, playerSkinManagment);
        Debug.Log("Add" + playerIndex);
    }

    internal PlayerSkinManagment GetPlayerSkinManagment(int playerIndex)
    {
        return playerSkinManagments[playerIndex];
    }
}
