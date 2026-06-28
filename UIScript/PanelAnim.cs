using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class PanelAnim : MonoBehaviour
{
    private WoodBoxControl _woodBoxControl; 

    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject panel;
    public GameObject passPanel;

    public GameObject waitImage;
    public Image wait;

    public GameObject touchImage;
    public Image touch;

    public GameObject Star;

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private float panelHeight;

    [SerializeField]
    private bool hasPause = false;

    private RectTransform rectTransformPass;
    private Vector2 originalAnchoredPositionPass;
    private float passPanelHeight;

    public bool allowStartpassPanel = false;
    public event Action allowGetTime;

    //请稍等
    private RectTransform waitTransform;
    private Vector2 originalAnchoredPositionWait;
    private float waitImageTime = 0;
    private bool startWaitTime = false;

    //木箱接触水
    private RectTransform touchTransform;
    private Vector2 originalAnchoredPositionTouch;
    private float touchImageTime = 0;
    private bool startTouchTime = false;
    private void Awake()
    {
        _woodBoxControl = FindObjectOfType<WoodBoxControl>();
        _woodBoxControl.woodBovMoving += ShowWait;
        _woodBoxControl.boxTouchWater += ShowTouch;
    }
    void Start()
    {

        //PausePanel
        rectTransform = panel.GetComponent<RectTransform>();
        // 保存pause原始位置
        originalAnchoredPosition = rectTransform.anchoredPosition;
        // 获取pause面板高度
        panelHeight = rectTransform.rect.height;

        //PassPanel
        rectTransformPass = passPanel.GetComponent<RectTransform>();
        // 保存pass原始位置
        originalAnchoredPositionPass = rectTransformPass.anchoredPosition;
        // 获取pass面板高度
        passPanelHeight = rectTransformPass.rect.height;

        //waitImage
        waitTransform = waitImage.GetComponent<RectTransform>();
        // 保存pass原始位置
        originalAnchoredPositionWait = waitTransform.anchoredPosition;

        //touchImage
        touchTransform = touchImage.GetComponent<RectTransform>();
        // 保存pass原始位置
        originalAnchoredPositionTouch = touchTransform.anchoredPosition;


        if (Star == null)
        {
            Star = GameObject.Find("Star");
        }
    }

    IEnumerator ShowPanel(RectTransform rectTran,GameObject gameObject, Vector2 rPos, float height)
    {
        float timer = 0f;
        // 起始位置：在屏幕上方（偏移量为负高度）
        Vector2 startPos = new Vector2(rPos.x, -height);
        // 结束位置：原始位置
        Vector2 endPos = new Vector2(rPos.x,0); ;

        while (timer <= 1f)
        {
            // 根据曲线计算插值进度
            float progress = showCurve.Evaluate(timer);
            // 在起始和结束位置之间插值
            rectTran.anchoredPosition = Vector2.Lerp(startPos, endPos, progress);
            timer += Time.unscaledDeltaTime * animationSpeed;
            yield return null;
        }

        // 确保最终位置准确
        rectTran.anchoredPosition = endPos;
    }

    IEnumerator HidePanel(RectTransform rectTran, GameObject gameObject, Vector2 rPos, float height)
    {
        float timer = 0f;

        Vector2 startPos = new Vector2(rPos.x, 0);

        Vector2 endPos = rPos;

        while (timer <= 1f)
        {
            float progress = hideCurve.Evaluate(timer);
            rectTran.anchoredPosition = Vector2.Lerp(endPos, startPos, progress);
            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }

        // 确保最终位置准确
        rectTran.anchoredPosition = endPos;
    }

    IEnumerator HideImage(RectTransform rectTran, GameObject gameObject, Vector2 rPos)
    {
        float timer = 0f;
        Color color = wait.color;
        Vector2 startPos = rPos;

        Vector2 endPos = new Vector2(rPos.x, rPos.y + 25);

        while (timer <= 1f)
        {
            float progress = hideCurve.Evaluate(timer);
            rectTran.anchoredPosition = Vector2.Lerp(endPos, startPos, progress);

            color.a = Mathf.Lerp(100, 0, timer);
            wait.color = color;

            timer += Time.deltaTime * animationSpeed;
            yield return null;
        }

        // 确保最终位置准确
        rectTran.anchoredPosition = endPos;
        color.a = 0;
        wait.color = color;
        gameObject.SetActive(false);
    }

    //异步重新加载场景
    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // 等待加载完成
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    //异步加载新场景
    IEnumerator LoadHomeAsyncScene(string sceneName)
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
        if (hasPause)
        {
            Time.timeScale = 0f;
        }
        if (!hasPause)
        {
            Time.timeScale = 1.0f;
        }
        //Debug.Log(Time.timeScale);
        if (Star.transform.childCount == 0)
        {
            ShowPassPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showPausePanel();
        }
        if (startWaitTime)
        {
            waitImageTime += Time.deltaTime;
            if (waitImageTime >= 0.8f)
            {
                HideWait();
            }
        }
        if (startTouchTime)
        {
            touchImageTime += Time.deltaTime;
            if (touchImageTime >= 0.8f)
            {
                HideTouch();
            }
        }
    }

    public void showPausePanel()
    {
        hasPause = true;
        panel.SetActive(true);
        StartCoroutine(ShowPanel(rectTransform,panel, originalAnchoredPosition, panelHeight));
    }

    public void hidePausePanel()
    {
        hasPause = false;
        Time.timeScale = 1.0f;
        StartCoroutine(HidePanel(rectTransform, panel,originalAnchoredPosition, panelHeight));
    }

    public void reStart()
    {
        hidePausePanel();
        StartCoroutine(LoadSceneAsync());
    }

    public void ShowPassPanel()
    {
        allowGetTime?.Invoke();

        hasPause = true;
        passPanel.SetActive(true);
        StartCoroutine(ShowPanel(rectTransformPass, passPanel, originalAnchoredPositionPass,passPanelHeight));

    }

    public void ReStartInPass()
    {
        StartCoroutine(LoadSceneAsync());
    }

    public void LoadScene()
    {
        StartCoroutine(LoadHomeAsyncScene("SampleScene"));
    }
    public void NextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string SceneName = currentScene.name;
        if (SceneName == "GametestWindow")
        {
            StartCoroutine(LoadHomeAsyncScene("GametestWindow 1"));
        }
        if (SceneName == "GametestWindow 1")
        {
            StartCoroutine(LoadHomeAsyncScene("GametestWindow 2"));
        }
        //if (SceneName == "GametestWindow")
        //{
        //    StartCoroutine(LoadHomeAsyncScene("GametestWindow 1"));
        //}

    }
    public void ShowWait()
    {
        startWaitTime = true;
        waitImage.SetActive(true);
    }
    public void HideWait()
    {
        startWaitTime = false;
        StartCoroutine(HideImage(waitTransform, waitImage, originalAnchoredPositionWait));
        //waitImage.SetActive(false);
    }

    public void ShowTouch()
    {
        startTouchTime = true;
        touchImage.SetActive(true);
    }
    public void HideTouch()
    {
        startTouchTime = false;
        StartCoroutine(HideImage(touchTransform, touchImage, originalAnchoredPositionTouch));
        //touchImage.SetActive(false);
    }
}

