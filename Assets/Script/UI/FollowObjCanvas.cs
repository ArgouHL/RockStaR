using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowObjCanvas : CanvasCtr
{
    [SerializeField] private Transform follewTarget;
    //[SerializeField] private bool smoothfollow;

    private void Update()
    {
        if (!IsFollowActive())
            return;

        transform.position = Camera.main.WorldToScreenPoint(follewTarget.position);
        ShowCanvas();
    }


    private bool IsFollowActive()
    {
        if (follewTarget.gameObject.activeInHierarchy)
            return true;
        else
        {
            canvas.alpha = 0;
            return false;
        }
    }


    private void ShowCanvas()
    {

        if (isShown == canvas.alpha > 0 && canvas.alpha == showAlpha)
            return;
        if (isShown)
        {
            canvas.alpha = showAlpha;
        }
        else
        {
            canvas.alpha = 0;
        }

    }



}
