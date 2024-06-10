using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtns : MonoBehaviour
{
  public void LoadGame()
    {
        SceneMgr.instance.StartLoadSelectScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
