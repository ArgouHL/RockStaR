using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Chara",menuName = "CharaterSetting/new Chara")]
public class CharaterSettingObj : ScriptableObject
{
    public CharaterSetting charaterSetting;
}

[Serializable]
public class CharaterSetting
{ 
    public Mesh bodyMesh;
    public Material material;
    public AnimatorOverrideController controller;

   
}
