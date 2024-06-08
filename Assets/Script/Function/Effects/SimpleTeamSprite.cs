using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleTeamSprite : TeamEffect
{
    public SpriteRenderer sr;
    internal override void StartEffect(Team team)
    {
        StopEffect();
        switch (team)
        {
            case Team.None:
                sr.color = Color.white;
                break;
            case Team.Blue:
                sr.color = new Color(0,1,1);

                break;
            case Team.Yellow:
                sr.color = new Color(1, 0.95f, 0);
                break;

        }
        
    }

    internal override void StopEffect()
    {
        sr.color = Color.white;


    }
}
