using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RockDrop : AreaEffect
{
    [SerializeField] private float hitRadius = 2;
    private DecalProjector decalProjector;
    [SerializeField] private Material decalmat;

    private void Awake()
    {
        decalProjector = GetComponentInChildren<DecalProjector>();
        decalProjector.material = new Material(decalmat);
        Vector3 s = decalProjector.size;
        s.x = hitRadius * 2;
        s.y = hitRadius * 2;
        decalProjector.size = s;

    }
    internal override void EffectStart()
    {
        EffectStartCount();
        VisualShow();
    }

    protected override void EffectEnd()
    {
        //triggerCollider.enabled = true;
        RockHit();
        VisualHide();
    }

    protected override IEnumerator EffectCountDownIE()
    {
        Debug.Log("StartCount");
        float t = 0;
        float size;
        while (t < duration)
        {
            size = t / duration;

            UpdateHitIndcator(size);
            t += Time.deltaTime;
            yield return null;
        }
        Debug.Log("EndCount");
        EffectEnd();

    }

    private void UpdateHitIndcator(float size)
    {
        decalProjector.material.SetFloat("_Size", size);
    }


    private void RockHit()
    {
        var cols = Physics.OverlapSphere(transform.position, hitRadius);

        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                if (col.TryGetComponent<PlayerCtr>(out PlayerCtr po))
                {
                    Debug.Log("Hit");
                    po.Stun(3f);
                }
            }
        }
    }
}
