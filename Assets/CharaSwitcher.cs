using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharaSwitcher : MonoBehaviour
{
    internal static CharaSwitcher instance;
    [SerializeField] private List<PlayerSkinManagment> playerSkinManagments;

    private void Awake()
    {
        playerSkinManagments = new List<PlayerSkinManagment>();
        instance = this;
    }

    internal void AddPlayerSkinManagment(PlayerSkinManagment playerSkinManagment)
    {
        playerSkinManagments.Add(playerSkinManagment);
    }
    internal void SetPlayerSkinColor(int playerIndex, int newIndex)
    {
        CharaType charaType = (CharaType)PlayerConfigManager.instance.GetPlayerConfig(playerIndex).CharaterIndex;

        playerSkinManagments[playerIndex].SetColor(newIndex);
    }

    internal void SetPlayerSkin(int playerIndex, int newIndex)
    {
        playerSkinManagments[playerIndex].SetModel(newIndex);
    }

    internal bool IsColorUsing(int charaterIndex, int colorIndex)
    {
        var usedSkin =playerSkinManagments.Where(skinManagment=> skinManagment.gameObject.activeInHierarchy).Select(skinManagment => skinManagment.GetSkinID());
        foreach (var v2 in usedSkin)
        {
            Debug.Log(v2);

        }
        Debug.Log("now " + charaterIndex + "," + colorIndex);
        bool b = usedSkin.Contains(new Vector2Int(charaterIndex, colorIndex));
        Debug.Log(b);
        return b;

    }
}
