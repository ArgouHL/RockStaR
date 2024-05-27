using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleCtr : MonoBehaviour
{
    [SerializeField] private Team team;
    private Collider holeCollider;
    [SerializeField] private GameObject shield;


    private void Awake()
    {
        holeCollider = GetComponent<Collider>();
       
    }

    internal void EnableShield(bool isShield)
    {
        holeCollider.enabled=!isShield;
        shield.SetActive(isShield);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jewelry")&& other.TryGetComponent<JewelryCtr>(out JewelryCtr jewelryCtr))
        {
            if (JewelrySystem.instance.NowTeam() == Team.None)
                return;
            ScoreSys.instance.AddScore(JewelrySystem.instance.NowTeam(), 1);
           // JewelrySystem.instance.ReSpawnJewelry();
        }
    }
}
