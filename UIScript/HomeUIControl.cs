using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //异步加载新场景
    IEnumerator LoadLevelAsyncScene(string sceneName)
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
    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene()
    {
        StartCoroutine(LoadLevelAsyncScene("LevelScene"));
    }
}
