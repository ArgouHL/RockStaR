using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamChooseDetecter : MonoBehaviour
{

    [SerializeField] private Team team;
    [SerializeField] private float radius = 2;
    private TeamChooseCtr teamChooseCtr;
    private CapsuleCollider checkCollider;
    [SerializeField] private TMP_Text numShow;

    private void Awake()
    {
        teamChooseCtr = GetComponentInParent<TeamChooseCtr>();
        checkCollider = GetComponentInChildren<CapsuleCollider>();
        checkCollider.radius = radius - 0.2f;

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (!other.gameObject.CompareTag("Player"))
            return;
        teamChooseCtr.Check();
        if (other.transform.parent.TryGetComponent<PlayerCtr>(out PlayerCtr playerCtr))
        {
            Debug.Log(other.transform.parent.gameObject.name);
            playerCtr.SetTeamInConfig(team);
        }



    }

    private void OnTriggerExit(Collider other)
    {
      
        if (!other.gameObject.CompareTag("Player"))
            return;
        teamChooseCtr.Check();
        if (other.transform.parent.TryGetComponent<PlayerCtr>(out PlayerCtr playerCtr))
        {
            Debug.Log(other.transform.parent.gameObject.name);
            playerCtr.SetTeamInConfig(Team.None);
        }
    }

    internal int GetMembersOnField(int targetNumber)
    {
        int num = 0;
        var cols = Physics.OverlapSphere(transform.position, radius, 1 << 20, QueryTriggerInteraction.Collide);
        foreach (var col in cols)
        {
            var vector = col.transform.position - transform.position;
            vector.y = 0;
            float disSqrt = vector.sqrMagnitude;
          //  Debug.Log(col.gameObject.name + disSqrt);

            if (disSqrt < radius * radius)
            {

                num++;
            }
        }
        numShow.text = num + "/" + targetNumber;
        return num;
    }



}

public enum Team { None,Blue, Yellow }
