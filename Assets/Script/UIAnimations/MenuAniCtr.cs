using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAniCtr : MonoBehaviour
{
    public RectTransform[] buttons;
    public float startDelay=1;
    public float delay = 0.5f;

    public RectTransform logoReflect;
    public GameObject logoStart;

    private void Start()
    {
        float _delay = startDelay;

        LeanTween.delayedCall(_delay, () => LeanTween.moveX(logoReflect, 2546, 0.8f));
        LeanTween.delayedCall(_delay + 0.6f, () => LeanTween.value(logoStart, (float val) => logoStart.transform.localScale = Vector3.one * val, 0, 2, 0.2f).setOnComplete(()=> LeanTween.value(logoStart, (float val) => logoStart.transform.localScale = Vector3.one * val, 2, 0, 0.2f)));
        for (int i=0;i<buttons.Length;i++)
        {
            RectTransform button = buttons[i];
            LeanTween.delayedCall(_delay, () => LeanTween.moveX(button, 72, 0.3f).setEase(LeanTweenType.easeOutQuart).setOnComplete(() => button.GetComponent<UIFloating>().StartFloat()));
            _delay += delay;
        }
     

    }


}
