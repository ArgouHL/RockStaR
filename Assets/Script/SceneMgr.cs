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
    public string m_SelectScene_2P;
    public string m_PlayScene_2P;


#if UNITY_EDITOR
    public UnityEditor.SceneAsset Menu;
    public UnityEditor.SceneAsset LoadingScene;
    public UnityEditor.SceneAsset SelectScene;
    public UnityEditor.SceneAsset PlayScene;
    public UnityEditor.SceneAsset SelectScene_2P;
    public UnityEditor.SceneAsset PlayScene_2P;

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
        if (SelectScene_2P != null)
        {
            m_SelectScene_2P = SelectScene_2P.name;
        }
        if (PlayScene != null)
        {
            m_PlayScene_2P = PlayScene_2P.name;
        }
    }
#endif

    internal void StartLoadSelectScene(playMode pm)
    {
        StartCoroutine(LoadSelectIE(pm));

    }

    private IEnumerator LoadSelectIE(playMode pm)
    {
        OnSceneEnd?.Invoke();
        SceneManager.LoadScene(m_LoadingScene);
        yield return null;
        switch (pm)
        {
            case playMode.Two:
                sceneToLoad= SceneManager.LoadSceneAsync(m_SelectScene_2P);
                break;
            case playMode.Four:
                sceneToLoad = SceneManager.LoadSceneAsync(m_SelectScene);
                break;
        }

        sceneToLoad.allowSceneActivation = false;
    }



    internal void EndLoadScene()
    {
        sceneToLoad.allowSceneActivation = true;
    }

    internal void LoadGame(playMode pm)
    {
        switch (pm)
        {
            case playMode.Two:
                SceneManager.LoadScene(m_PlayScene_2P);
                break;
            case playMode.Four:
                SceneManager.LoadScene(m_PlayScene);
                break;
            default:
                break;
        }

     
    }

    internal void LoadMenu()
    {
      
        OnSceneEnd?.Invoke();
        SceneManager.LoadScene(m_Menu);
    }

}

public enum playMode {Two,Four }

