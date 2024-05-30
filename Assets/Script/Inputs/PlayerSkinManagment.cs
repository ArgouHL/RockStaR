using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerSkinManagment : MonoBehaviour
{

    [SerializeField] internal ModelSkinCtr[] modelSkinCtrs;

    internal void SetColor(int colorIndex)
    {
        GetComponentInChildren<ModelSkinCtr>().SetColor(colorIndex);
    }

    internal void SetModel(int charaIndex)
    {
        for(int i=0;i< modelSkinCtrs.Length;i++)
        {
            if(i== charaIndex)
            {
                modelSkinCtrs[i].gameObject.SetActive(true);
            }
            else
                modelSkinCtrs[i].gameObject.SetActive(false);
        }
        GetComponentInChildren<ModelSkinCtr>().SetColor(0);
        GetComponent<PlayerCtr>().ReGetAnimator();

    }

    internal Vector2Int GetSkinID()
    {
        var config = GetComponent<PlayerCtr>().playerConfig;
        return new Vector2Int(config.CharaterIndex, config.CharaterColorIndex);
    }
}
