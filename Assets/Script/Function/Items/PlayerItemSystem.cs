using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemSystem : MonoBehaviour
{
    [SerializeField] private float throwForce = 2;
    [SerializeField] private MeshRenderer itemInd;

    private PlayerCtr player=> GetComponent<PlayerCtr>();
    private Transform playerModel => player.playerModel;
    [SerializeField] private ItemObj holdingItem;
    private PlayerBuffsContainer buffsContainer => GetComponent<PlayerBuffsContainer>();


    private void OnEnable()
    {
        player.OnUseItem += UseItem;
    }

    private void OnDisable()
    {
        player.OnUseItem -= UseItem;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Item"))
        {
            Debug.Log(other.name);
            if (holdingItem != null)
                return;
            Debug.Log(other.name+"2");
            if (other.TryGetComponent<ItemObj>(out ItemObj i))
            {
                Debug.Log(other.name + "3");
                holdingItem = i;
                itemInd.material = i.GrapedUp();
                itemInd.gameObject.SetActive(true);

            }
        }
    }

    private void UseItem()
    {
        if (player.isStunned)
            return;

        if (holdingItem == null)
            return;
        
        ItemType _type = holdingItem.GetItemType();
        switch (_type)
        {
            case ItemType.RockFall:
                DropRock();
                break;
            case ItemType.Toxic:
                ShootToxic();
                break;
            case ItemType.Inf:
                AddInf();
                break;  
            case ItemType.Mines:
                PlaceMine(player.playerID);
                break;

        }
        
        itemInd.gameObject.SetActive(false);
        holdingItem = null;
    }

    private void DropRock()
    {
        Debug.Log("Drop");
        AreaEffectSystem.instance.DropRock();
    }

    private void PlaceMine(int playerID)
    {
        Debug.Log("PlaceMine");
        AreaEffectSystem.instance.SetMine(transform.position, playerID);
    }

    private void ShootToxic()
    {
        Debug.Log("ShootToxic");
        AreaEffectSystem.instance.ShootToxic(playerModel.forward, playerModel.position);
    }

    private void AddInf()
    {
        Debug.Log("inf");
        buffsContainer.AddBuff(new InfBuff(buffsContainer, 10));
    }
}
