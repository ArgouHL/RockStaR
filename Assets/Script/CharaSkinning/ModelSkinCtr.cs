using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSkinCtr : MonoBehaviour
{
    public CharaType charaType;
    public SkinnedMeshRenderer[] meshRenderers;

    public void SetColor(int colorIndex)
    {
        Material _material = SkinSystem.instance.GetSkinColors(charaType).materials[colorIndex];
        foreach( var meshRenderer in meshRenderers)
        {
            meshRenderer.material = _material;
        }
    }
}

