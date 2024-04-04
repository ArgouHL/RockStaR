using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEffect : AreaEffect
{
    [SerializeField] private float exploseRadius;
    internal override void EffectStart()
    {
        EffectStartCount();
    }

    protected override void EffectEnd()
    {
        triggerCollider.enabled = true;
        VisualShow();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
         
            var cols=Physics.OverlapSphere(transform.position, exploseRadius);
            
            foreach(var col in cols)
            {
                if(col.CompareTag("Player"))
                {
                    if(col.TryGetComponent<PlayerCtr>(out PlayerCtr po))
                    {
                        Debug.Log("ex");
                        Vector3 dir = col.transform.position - transform.position;
                        dir.y = 0;
                        dir = dir.normalized;
                        
                        po.VelocityChange(dir*4 + new Vector3(0, 2, 0));
                    }
                }
            }
            triggerCollider.enabled = false;
            VisualHide();
        }

    }
}
