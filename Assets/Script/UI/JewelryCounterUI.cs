using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelryCounterUI : MonoBehaviour
{
    public static JewelryCounterUI instance;
    [SerializeField] private Sprite[] yellowJew;
    [SerializeField] private Sprite[] blueJew;
    [SerializeField] private Sprite emptyJew;
    [SerializeField] private float maxSize = 2f;
    [SerializeField] private Image image;
    private FollowObjCanvas canvas;
    private int counter = 0;
    private Team nowTeam = Team.None;
    private void Awake()
    {
        instance = this;
        canvas = GetComponentInChildren<FollowObjCanvas>();
    }

    internal void ShowCount(Team nowTeam, int count)
    {
        image.color = new Color(1, 1, 1, 1);
        image.transform.localScale = Vector3.one;
        switch (nowTeam)
        {
            case Team.Blue:
                image.sprite = count <= blueJew.Length ? blueJew[count - 1] : blueJew[blueJew.Length - 1];
                break;
            case Team.Yellow:
                image.sprite = count <= yellowJew.Length ? yellowJew[count - 1] : yellowJew[yellowJew.Length - 1];
                break;

        }
        Debug.Log("Co");
    }

    internal void ShowEnd(Team nowTeam)
    {
        image.color = new Color(1, 1, 1, 1);
        switch (nowTeam)
        {
            case Team.Blue:
                image.sprite = blueJew[blueJew.Length - 1];
                break;
            case Team.Yellow:
                image.sprite = yellowJew[yellowJew.Length - 1];
                break;

        }
        LeanTween.value(1, maxSize, 0.3f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float val) =>
            image.transform.localScale = Vector3.one * val);
        LeanTween.delayedCall(0.1f, () => LeanTween.value(1, 0, 0.2f).setOnUpdate((float val) =>
              image.color = new Color(1, 1, 1, val)));

    }


}

