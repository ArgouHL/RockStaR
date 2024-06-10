using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleTmpAni : MonoBehaviour
{
    public TMP_Text tMP_Text;

    public string[] texts;
    public float duration = 0.3f;
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                tMP_Text.text = texts[i];
                yield return new WaitForSeconds(duration);
            }

        }
    }
}
