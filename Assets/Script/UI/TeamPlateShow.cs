using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPlateShow : MonoBehaviour
{
    [SerializeField] private Image plate;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Sprite cyan;
    [SerializeField] private Sprite gray;


    private void Start()
    {
        
    }

    internal void ChangeTeam(Team team)
    {
        switch (team)
        {
            case Team.Blue:
                plate.sprite = cyan;
                break;
            case Team.Yellow:
                plate.sprite = yellow;
                break;
            default:
                plate.sprite = gray;
                break;

        }
    }

}
