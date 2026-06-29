using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIControl : MonoBehaviour
{
    void Start()
    {

    }
    //异步加载新场景
    IEnumerator LoadGamingAsyncScene(string sceneName)
    {
        // 开始异步加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 等待场景加载完成
        while (!asyncLoad.isDone)
        {
            // 这里的 yield return null 意味着每帧都会检查一次状态
            yield return null;
        }
    }
    void Update()
    {

    }

    public void LoadGame_1Scene()
    {
        StartCoroutine(LoadGamingAsyncScene("GametestWindow"));
    }
    public void LoadGame_2Scene()
    {
        StartCoroutine(LoadGamingAsyncScene("GametestWindow 1"));
    }
    public void LoadGame_3Scene()
    {
        StartCoroutine(LoadGamingAsyncScene("GametestWindow 2"));
    }
}
