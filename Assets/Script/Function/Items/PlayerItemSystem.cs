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
            if (holdingItem != null)
                return;
            if (other.TryGetComponent<ItemObj>(out ItemObj i))
            {
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
                PlaceMine();
                break;

        }
        holdingItem = null;
        itemInd.gameObject.SetActive(false);

    }

    private void DropRock()
    {
        Debug.Log("Drop");
        AreaEffectSystem.instance.DropRock(playerModel.forward, playerModel.position);
    }

    private void PlaceMine()
    {
        Debug.Log("PlaceMine");
        AreaEffectSystem.instance.SetMine(transform.position);
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
