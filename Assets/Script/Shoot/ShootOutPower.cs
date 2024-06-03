using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootOutPower : MonoBehaviour
{
    private PowerGun powerGun;
    private Coroutine powerFlayingCoro;
    private Rigidbody rig;
    [SerializeField] private EffectSwitcher shootEnergyEffect;


    private void Awake()
    {

        powerGun = GetComponentInParent<PowerGun>();
        rig = GetComponent<Rigidbody>();
     //   GetComponentInChildren<MeshRenderer>().material = new Material(powerGun.powerMat);
    }

    internal void ShootOut(Vector3 shootDir, float maxDis, Vector3 startPos, float speed)
    {
        shootEnergyEffect.StopEffect();
        transform.position = startPos;
        gameObject.SetActive(true);
        SetTeam(powerGun.playerCtr.choosedTeam);
        powerFlayingCoro = StartCoroutine(PowerFlayingIE(shootDir, maxDis, speed, startPos));
    }

    private IEnumerator PowerFlayingIE(Vector3 shootDir, float maxDis, float speed, Vector3 startPos)
    {

        transform.parent = null;

        float dis = 0;
        while (dis < maxDis)
        {
         //   float translateDis = Time.deltaTime * speed;
            dis += Time.fixedDeltaTime * speed;
           rig.MovePosition(startPos+ shootDir.normalized* dis);

            yield return new WaitForFixedUpdate() ;
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
        shootEnergyEffect.StartEffect(team);
        Debug.Log("Set " + team);
    }

    private void PushObj(PushableObj pullableObj)
    {
        Vector3 _powerDir = pullableObj.transform.position - transform.position;
        _powerDir.y = 0;
        _powerDir = _powerDir.normalized;
        pullableObj.PowerPushed(_powerDir * powerGun.pushPower, powerGun.playerCtr.choosedTeam);

    }


    private void PowerDisapper(bool playHitSfx = true)
    {
        shootEnergyEffect.StopEffect();
        if (playHitSfx)
            powerGun.PlayEnergyHitSfx(transform.position);
        if (powerFlayingCoro != null)
            StopCoroutine(powerFlayingCoro);
        transform.position = powerGun.PowerBackPool(this).position;
        // Debug.Log("PowerDisapper");
        gameObject.SetActive(false);

    }
}
