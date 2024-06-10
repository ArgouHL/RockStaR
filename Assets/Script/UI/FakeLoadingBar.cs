using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoadingBar : MonoBehaviour
{
    public Slider slider;
    public float loadTime;

    private void Start()
    {
        StartCoroutine(Loading());
        slider.value = 0;
    }

    private IEnumerator Loading()
    {
        float _time = 0;

        while (_time < loadTime)
        {
            slider.value = Mathf.Lerp(0, 0.97f, _time / loadTime);
            _time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        slider.value = 1;
        SceneMgr.instance.EndLoadScene();
    }
}
