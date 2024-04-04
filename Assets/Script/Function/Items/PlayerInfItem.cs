using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfItem : ItemEffect
{
    public override void EffectStart()
    {
        DisableVisual();
        //addbuff
        Debug.Log(transform.position);
    }
}
