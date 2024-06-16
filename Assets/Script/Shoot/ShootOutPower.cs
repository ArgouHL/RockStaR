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
    private Collider collider;
    [SerializeField] private EffectSwitcher shootEnergyEffect;
    [SerializeField] private EffectSwitcher hitEffect;
    [SerializeField] private Light light;
    private Team nowTeam;

    private void Awake()
    {

        powerGun = GetComponentInParent<PowerGun>();
        rig = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        //   GetComponentInChildren<MeshRenderer>().material = new Material(powerGun.powerMat);
    }

    internal void ShootOut(Vector3 shootDir, float maxDis, Vector3 startPos, float speed)
    {
       
       
        shootEnergyEffect.StopEffect();
        light.enabled = true;
        transform.parent = null;
        transform.position = startPos + shootDir.normalized * 1f;
      
        
        
        
        powerFlayingCoro = StartCoroutine(PowerFlayingIE(shootDir, maxDis, speed, transform.position));
    }

    private IEnumerator PowerFlayingIE(Vector3 shootDir, float maxDis, float speed, Vector3 startPos)
    {
        yield return null;
        collider.enabled = true;

        SetTeam(powerGun.playerCtr.choosedTeam);
        float dis = 0;
        while (dis < maxDis)
        {
            //   float translateDis = Time.deltaTime * speed;
            dis += Time.fixedDeltaTime * speed;
            rig.MovePosition(startPos + shootDir.normalized * dis);

            yield return new WaitForFixedUpdate();
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
            hitEffect.StartEffect(nowTeam);
        }

        if (other.CompareTag("Jewelry"))
        {
            JewelrySystem.instance.ChangeTeam(powerGun.playerCtr.choosedTeam);
            if (powerFlayingCoro != null)
            {
                StopCoroutine(powerFlayingCoro);
                powerFlayingCoro = null;

            }
            hitEffect.StartEffect(nowTeam);
            PowerDisapper();
        }
        if (other.gameObject.CompareTag("Wall"))
        {
            if (powerFlayingCoro != null)
            {
                StopCoroutine(powerFlayingCoro);
                powerFlayingCoro = null;

            }
            hitEffect.StartEffect(nowTeam);
            PowerDisapper();
        }
    }

   

    internal void SetTeam(Team team)
    {
        nowTeam = team;
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
        light.enabled = false;
        if (playHitSfx)
            powerGun.PlayEnergyHitSfx(transform.position);
        if (powerFlayingCoro != null)
            StopCoroutine(powerFlayingCoro);
        powerGun.PowerBackPool(this);
        // Debug.Log("PowerDisapper");
        collider.enabled = false;

    }
}
