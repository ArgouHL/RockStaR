using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOffset : MonoBehaviour
{
    public float speedX = 2;
    public float speedY = 2;
    public Material mat;
    float offsetX = 0;
    float offsetY = 0;
    private void Start()
    {
        mat.mainTextureOffset = new Vector2(0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        offsetX += speedX * Time.deltaTime;
        offsetY += speedY * Time.deltaTime;
        mat.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
