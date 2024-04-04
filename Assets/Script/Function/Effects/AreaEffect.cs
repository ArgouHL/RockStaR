using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaEffect : MonoBehaviour
{
    protected Coroutine effectCountDownCoro;
    [SerializeField] protected float duration;
    [SerializeField] protected GameObject visualGO;
    protected Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }
    protected virtual void EffectStartCount()
    {
      
        if (effectCountDownCoro == null)
            effectCountDownCoro = StartCoroutine(EffectCountDownIE());
      
    }

    internal abstract void EffectStart();


    protected abstract void EffectEnd();
  


    protected virtual void VisualShow()
    {
        Debug.Log("showVisual");
        visualGO.SetActive(true);
    }

    protected virtual void VisualHide()
    {
        Debug.Log("showVisual");
        visualGO.SetActive(false);
    }

    protected virtual IEnumerator EffectCountDownIE()
    {
        Debug.Log("StartCount");
        float t = 0;
        while (t < duration)
        {
            
            t += Time.deltaTime;
            yield return null;
        }
        Debug.Log("EndCount");
        EffectEnd();

    }

  
}
