using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{

    private Transform itempool;
    public Transform GetPareant { get { return itempool; } }
    private Rigidbody rig;
    private Collider triggerCollider;

    private ItemPooling itemPooling;

    [SerializeField] internal ItemType itemType;
    [SerializeField] private GameObject visual;
    [SerializeField] private MeshRenderer meshRenderer;


    private void Awake()
    {
        Init();

    }

    internal Material GrapedUp()
    {
        itemPooling.RecycleItem(this);
        return this.meshRenderer.sharedMaterial;
    }

    internal ItemType GetItemType()
    {
        return itemType;
    }


    internal void DirectUse(Vector3 direction)
    {
        //ItemEffectStart();
    }

    internal void disSpawn()
    {
        visual.SetActive(false);
        rig.isKinematic = true;
        triggerCollider.enabled = false;
    }



    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.transform.CompareTag("Ground"))
    //        return;
    //    onItemHitGround?.Invoke();

    //}

    //private void ItemEffectStart()
    //{

    //    onItemHitGround -= ItemEffectStart;
    //    transform.parent = itempool;

    //    effect.EffectStart();


    //}

    internal void Init()
    {
        rig = GetComponent<Rigidbody>();
        itempool = transform.parent;
        itemPooling = itempool.GetComponent<ItemPooling>();
        triggerCollider = GetComponent<Collider>();
        rig.isKinematic = true;
        visual.SetActive(false);
    }

    internal void ChangeTypeAndShow(ItemType t, Material m)
    {
        meshRenderer.material = m;
        itemType = t;
        visual.SetActive(true);
        triggerCollider.enabled = true;
        rig.isKinematic = false;
    }

}
