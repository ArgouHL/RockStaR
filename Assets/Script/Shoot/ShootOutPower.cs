using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootOutPower : MonoBehaviour
{
    private PowerGun powerGun;
    private Coroutine powerFlayingCoro;




    private void Awake()
    {

        powerGun = GetComponentInParent<PowerGun>();
        GetComponentInChildren<MeshRenderer>().material = new Material(powerGun.powerMat);
    }

    internal void ShootOut(Vector3 shootDir, float maxDis, Vector3 startPos, float speed)
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        SetTeam(powerGun.playerCtr.choosedTeam);
        powerFlayingCoro = StartCoroutine(PowerFlayingIE(shootDir, maxDis, speed));
    }

    private IEnumerator PowerFlayingIE(Vector3 shootDir, float maxDis, float speed)
    {

        transform.parent = null;

        float dis = 0;
        while (dis < maxDis)
        {
            float translateDis = Time.deltaTime * speed;
            dis += Time.deltaTime * speed;
            transform.position += shootDir * translateDis;

            yield return null;
        }

        PowerDisapper(false);
        transform.parent = powerGun.transform;
        powerFlayingCoro = null;
    }




    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.gameObject.name);


        if (other.TryGetComponent(out PushableObj pullableObj))
        {

            if (pullableObj.transform == powerGun.ownPlayer)
            {
                // Debug.Log("s");
                return;
            }

            PushObj(pullableObj);
            if (powerFlayingCoro != null)
            {
                StopCoroutine(powerFlayingCoro);
                powerFlayingCoro = null;
            }
            PowerDisapper();
        }

        if (other.CompareTag("Jewelry"))
        {
            JewelrySystem.instance.ChangeTeam(powerGun.playerCtr.choosedTeam);
            PowerDisapper();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            PowerDisapper();
        }
    }

    internal void SetTeam(Team team)
    {
        Color color;
        switch (team)
        {

            case Team.Blue:
                color = Color.cyan;
                break;
            case Team.Yellow:
                color = Color.yellow;
                break;
            default:
                color = Color.gray;

                break;

        }
        GetComponentInChildren<MeshRenderer>().material.color = color;
        GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor", color);
        Debug.Log("Set " + team);
    }

    private void PushObj(PushableObj pullableObj)
    {
        Vector3 _powerDir = pullableObj.transform.position - transform.position;
        _powerDir.y = 0;
        _powerDir = _powerDir.normalized;
        pullableObj.PowerPushed(_powerDir * powerGun.pushPower);

    }


    private void PowerDisapper(bool playHitSfx = true)
    {
        if (playHitSfx)
            powerGun.PlayEnergyHitSfx(transform.position);
        transform.position = powerGun.PowerBackPool(this).position;
        // Debug.Log("PowerDisapper");
        gameObject.SetActive(false);

    }
}
