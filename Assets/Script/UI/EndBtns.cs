using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBtns : MonoBehaviour
{
   public void Replay()
    {
        SceneMgr.instance.StartLoadSelectScene();
    }
    public void Menu()
    {
        SceneMgr.instance.LoadMenu();
    }
}
