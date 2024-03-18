using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorCtr : MonoBehaviour
{
    [SerializeField] private StriageIndcator ropeRangeIndacator;

    private void Start()
    {
        Debug.Log(ropeRangeIndacator.transform.forward);
    }

    internal void ShowRopeRangeIndacator(float maxRange )
    {
        ropeRangeIndacator.StartRange(maxRange);
        ropeRangeIndacator.GetComponent<CanvasGroup>().alpha = 1;
    }

    internal void HideRopeRangeIndacator()
    {
        ropeRangeIndacator.GetComponent<CanvasGroup>().alpha = 0;
    }


    internal void UpdateRopeRangeIndacator(float range, Vector2 currentWatchDir)
    {
        ropeRangeIndacator.UpdateRange(range);
        transform.forward = new Vector3(currentWatchDir.x, 0, currentWatchDir.y);
    }
}
