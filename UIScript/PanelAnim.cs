using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PanelAnim : MonoBehaviour
{
    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    public float animationSpeed;
    public GameObject panel;
    public GameObject passPanel;

    public GameObject Star;

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPosition;
    private float panelHeight;

    [SerializeField]
    private bool hasPause = false;

    private RectTransform rectTransformPass;
    private Vector2 originalAnchoredPositionPass;
    private float passPanelHeight;
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
        // 保存pause原始位置
        originalAnchoredPositionPass = rectTransformPass.anchoredPosition;
        // 获取pause面板高度
        passPanelHeight = rectTransformPass.rect.height;

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

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        // 等待加载完成
        while (!asyncLoad.isDone)
        {
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
        hasPause = true;
        passPanel.SetActive(true);
        StartCoroutine(ShowPanel(rectTransformPass, passPanel, originalAnchoredPositionPass,passPanelHeight));
    }

    public void ReStartInPass()
    {
        StartCoroutine(LoadSceneAsync());
    }
}
