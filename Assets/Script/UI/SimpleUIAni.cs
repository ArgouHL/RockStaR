using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIAni : MonoBehaviour
{
    public Image img;
    public Sprite[] sprites;
    public float duration = 0.3f;
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                img.sprite = sprites[i];
                yield return new WaitForSeconds(duration);
            }

        }
    }
}
