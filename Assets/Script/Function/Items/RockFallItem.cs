using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFallItem : ItemEffect
{
    public override void EffectStart()
    {
        DisableVisual();
        Debug.Log(transform.position);
    }
}
