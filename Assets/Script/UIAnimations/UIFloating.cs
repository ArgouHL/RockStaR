
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIFloating : MonoBehaviour
{
    public bool floatOnAwake;

    private delegate void UpdateEvent();
    private UpdateEvent OnUpdate;
    private Vector3 orgPostion;
    float timeX;
    float timeY;
    public float xSpeed;
    public float ySpeed;
    public float xDistance;
    public float yDistance;
    public UnityEvent OnStart;


    private void Start()
    {
        if (!floatOnAwake)
            return;
        StartFloat();
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }    

    private void OnDisable()
    {
        OnUpdate = null;
    }

    internal void StartFloat()
    {
        OnStart?.Invoke();
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        timeX = (float)Random.Range(0, Mathf.Deg2Rad * 360);
        Random.InitState(System.Guid.NewGuid().GetHashCode());
        timeY = (float)Random.Range(0, Mathf.Deg2Rad * 360);

        orgPostion = transform.position;
        OnUpdate += FloatOnUpdate;
    }

    internal void FloatOnUpdate()
    {
        timeX += Time.deltaTime;
        timeY += Time.deltaTime;
        float x = Mathf.Sin(timeX * xSpeed)* xDistance;
        float y = Mathf.Sin(timeY * ySpeed) * yDistance;
        transform.position = orgPostion + new Vector3(x, y, 0);
    }
}
