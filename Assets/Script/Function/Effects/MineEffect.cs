using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEffect : AreaEffect
{
    [SerializeField] private float exploseRadius;
    public int ownerID;

    internal void EffectStart(int playerID)
    {

        ownerID = playerID;
        EffectStart();
    }

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

        if (other.CompareTag("Player") && other.TryGetComponent<PlayerCtr>(out PlayerCtr po))
        {
            if (ownerID == po.playerID)
                return;
            var cols = Physics.OverlapSphere(transform.position, exploseRadius);

            foreach (var col in cols)
            {
                if (col.CompareTag("Player"))
                {
                    if (ownerID == po.playerID)
                        continue;

                    Debug.Log("ex");
                    Vector3 dir = col.transform.position - transform.position;
                    dir.y = 0;
                    dir = dir.normalized;

                    po.VelocityChange(dir * 4 + new Vector3(0, 2, 0));
                }
            }
            triggerCollider.enabled = false;
            VisualHide();
        }

    }
}
