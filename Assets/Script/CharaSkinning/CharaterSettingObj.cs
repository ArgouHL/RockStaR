using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Skin", menuName = "CharaterSetting/new Skin")]
public class CharaterSettingObj : ScriptableObject
{
    public CharaterSetting charaterSetting;
}

[Serializable]
public class CharaterSetting
{
    public CharaType charaType;
    public Material[] materials;
    public Sprite winPic;
    public Sprite losePic;

}

public enum CharaType { Fox=0,Duck=1,Raccoon=2,Rabbit=3}
