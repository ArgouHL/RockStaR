using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtns : MonoBehaviour
{
  public void LoadGame()
    {

        SceneMgr.instance.StartLoadSelectScene(playMode.Four);
    }

    public void LoadGame2P()
    {

        SceneMgr.instance.StartLoadSelectScene(playMode.Two);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
