using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerGun : MonoBehaviour
{
    internal PlayerCtr playerCtr;
    internal Transform ownPlayer => playerCtr.transform;
    [SerializeField] private Transform powerStartPos;

    // private Vector3 _powerDir;
    [SerializeField] internal float pushPower = 5;

    [SerializeField] internal float speed = 2;
    [SerializeField] private GameObject powerPerfab;
    [SerializeField] private Queue<GameObject> powers;
    //  private JewelryCtr jewelryCtr;

    [SerializeField] private float maxDis = 5;
    //  private float pullForce;

    [SerializeField] internal Material powerMat;
    private bool canShoot;

    private EnergySfxControl energySfxControl;
    private void Awake()
    {
        playerCtr = GetComponentInParent<PlayerCtr>();
        energySfxControl = GetComponentInChildren<EnergySfxControl>();
        Debug.Log("PowerAwake");

    }

    private void Start()
    {
        powers = new Queue<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("q");
            GameObject powerObj = Instantiate(powerPerfab, transform);
            powers.Enqueue(powerObj);
        }
       // SetTeam(playerCtr.choosedTeam);
    }


    internal void Shoot(Vector3 shootDir)
    {
        if (powers == null || powers.Count <= 0)
            return;
        ShootOutPower _power = powers.Dequeue().GetComponent<ShootOutPower>();
        shootDir.y = 0;
        shootDir = shootDir.normalized;
        playerCtr.playerSfxControl.PlayShootSfx();
        _power.ShootOut(shootDir, maxDis, transform.position, speed);
        //_powerDir = shootDir;

        //if (powerFlayingCoro != null)
        //    return;

        // powerFlayingCoro = StartCoroutine(PowerFlayingIE(shootDir, maxDis));


    }







    //internal void SetTeam(Team team)
    //{
    //    foreach (var power in powers)
    //    {
    //        power.GetComponent<ShootOutPower>().SetTeam(team);
    //    }

    //}

    internal Transform PowerBackPool(ShootOutPower shootOutPower)
    {
        powers.Enqueue(shootOutPower.gameObject);
        return powerStartPos;
    }

    internal void PlayEnergyHitSfx(Vector3 pos)
    {
        energySfxControl.PlayEnergyHitSfx(pos);
    }
}
