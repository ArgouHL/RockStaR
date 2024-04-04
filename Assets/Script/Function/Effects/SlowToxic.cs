using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowToxic : AreaEffect
{


    internal override void EffectStart()
    {
        Debug.Log("EffectStart");
        triggerCollider.enabled = true;
        VisualShow();
        EffectStartCount();
    }


    protected override void EffectEnd()
    {
        triggerCollider.enabled = false;
        VisualHide();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            if (other.TryGetComponent<PlayerBuffsContainer>(out PlayerBuffsContainer pfc))
            {
                Debug.Log(other.gameObject.name);
                pfc.AddDeBuff(new SlowBuff(pfc, 3));
            }
        }

    }



}
