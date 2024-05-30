using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkinSystem : MonoBehaviour
{
    public static SkinSystem instance;
    private void SetInstance()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] internal CharaterColorSettingObj[] charaterColorSettingObjs;
    private Dictionary<CharaType, CharaterColorSetting> charaterColorSettingDict;
    private void SetCharaDictionary()
    {
        charaterColorSettingDict = new Dictionary<CharaType, CharaterColorSetting>();
        foreach( var obj in charaterColorSettingObjs)
        {
            charaterColorSettingDict.Add(obj.charaterColorSetting.charaType, obj.charaterColorSetting);
        }
    }

    private void Awake()
    {
        SetInstance();
        SetCharaDictionary();
    }

    internal CharaterColorSetting GetSkinColors(CharaType type)
    {
        return charaterColorSettingDict[type];
    }

}
