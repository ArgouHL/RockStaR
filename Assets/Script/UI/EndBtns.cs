using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBtns : MonoBehaviour
{
   public void Replay()
    {
        switch(InputConnect.instance.playerCount)
        {
            case 2:
                SceneMgr.instance.StartLoadSelectScene(playMode.Two);
                break;
            case 4:
                SceneMgr.instance.StartLoadSelectScene(playMode.Four);
                break;
        }
    
    }
    public void Menu()
    {
        SceneMgr.instance.LoadMenu();
    }
}
