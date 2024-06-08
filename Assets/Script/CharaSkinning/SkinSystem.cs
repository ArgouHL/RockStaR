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

    [SerializeField] internal CharaterSettingObj[] charaterColorSettingObjs;
    private Dictionary<CharaType, CharaterSetting> charaterSettingDict;
    private void SetCharaDictionary()
    {
        charaterSettingDict = new Dictionary<CharaType, CharaterSetting>();
        foreach( var obj in charaterColorSettingObjs)
        {
            charaterSettingDict.Add(obj.charaterSetting.charaType, obj.charaterSetting);
        }
    }

    private void Awake()
    {
        SetInstance();
        SetCharaDictionary();
    }

    internal CharaterSetting GetCharaData(CharaType type)
    {
        return charaterSettingDict[type];
    }

  

}
