using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SkinColour", menuName = "CharaterSetting/new SkinColour")]
public class CharaterColorSettingObj : ScriptableObject
{
    public CharaterColorSetting charaterColorSetting;
}

[Serializable]
public class CharaterColorSetting
{
    public CharaType charaType;
    public Material[] materials;  
   
}

public enum CharaType { fox=0,duck=1,rokoon=2,rubbit=3}
