using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : MonoBehaviour
{
    protected  GameObject crystalVisual;

    public abstract void EffectStart();


    protected void DisableVisual()
    {
        crystalVisual.SetActive(false);

    }
}
