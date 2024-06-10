using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance;
    public static AsyncOperation sceneToLoad;
    public delegate void ResetScene();
    public static ResetScene OnSceneEnd;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public string m_Menu;
    public string m_LoadingScene;
    public string m_SelectScene;
    public string m_PlayScene;


#if UNITY_EDITOR
    public UnityEditor.SceneAsset Menu;
    public UnityEditor.SceneAsset LoadingScene;
    public UnityEditor.SceneAsset SelectScene;
    public UnityEditor.SceneAsset PlayScene;

    private void OnValidate()
    {
        if (Menu != null)
        {
            m_Menu = Menu.name;
        }
        if (LoadingScene != null)
        {
            m_LoadingScene = LoadingScene.name;
        }
        if (SelectScene != null)
        {
            m_SelectScene = SelectScene.name;
        }
        if (PlayScene != null)
        {
            m_PlayScene = PlayScene.name;
        }
    }
#endif

    internal void StartLoadSelectScene()
    {
        StartCoroutine(LoadSelectIE());

    }

    private IEnumerator LoadSelectIE()
    {
        OnSceneEnd?.Invoke();
        SceneManager.LoadScene(m_LoadingScene);
        yield return null;

        sceneToLoad = SceneManager.LoadSceneAsync(m_SelectScene);

        sceneToLoad.allowSceneActivation = false;
    }



    internal void EndLoadScene()
    {
        sceneToLoad.allowSceneActivation = true;
    }

    internal void LoadGame()
    {
        
       
        SceneManager.LoadScene(m_PlayScene);
    }

    internal void LoadMenu()
    {
      
        OnSceneEnd?.Invoke();
        SceneManager.LoadScene(m_Menu);
    }

}


