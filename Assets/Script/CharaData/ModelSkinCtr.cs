using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSkinCtr : MonoBehaviour
{
    public CharaType charaType;
    [SerializeField] private SkinGroup[] skinGroups;
}

[Serializable]
public struct SkinGroup
{
    public SkinnedMeshRenderer[] skins;
}