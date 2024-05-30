using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WarningLine : MonoBehaviour
{
    private RawImage img;
    public Color color1;
    public Color color2;
    public float tillMultiplyer;
    public float offsetSpeed;

    private void OnValidate()
    {
        if(img==null)
        {
            img = GetComponent<RawImage>();
            img.material = Instantiate(Resources.Load<Material>("WarningLine"));
        }

        img.material.SetColor("_Color1", color1);
        img.material.SetColor("_Color2", color2);
        img.material.SetFloat("_TillingSize", tillMultiplyer);
        img.material.SetVector("_Tilling", img.rectTransform.sizeDelta);
        img.material.SetFloat("_OffsetSpeed", offsetSpeed);


    }
}
