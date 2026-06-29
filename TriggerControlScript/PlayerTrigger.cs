using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    private PanelAnim _panelAnim;

    private GameObject player = null;
    [Header("星星功能：")]
    public GameObject Star;
    private GameObject [] childStars;
    public Text starCount;
    public Text ChildCount;
    //private GameObject destroyChild;

    public bool bonk = false;

    [SerializeField] private int childCount = 0;
    [SerializeField] private int collectCount = 0;

    [Header("时间：")]
    public Text timerText;       // 显示时间的UI文本
    public Text pauseTime;
    private float currentTime = 0f;
    private bool isTiming = false;
    private string timeText;
    public event Action<string> timeNum;

    [Header("通过面板：")]
    //passPanel
    public Text starCountInPass;
    public Text timeInPass;
    public string passTime;

    [Header("交互功能_木箱：")]
    //interactive
    public GameObject woodBoxMove;

    //woodBox
    public GameObject woodBox;
    private bool isWoodBoxMove = false;
    private float startTime;
    public float moveDuration = 2f; // 移动耗时（秒）

    public event Action<bool> boxCollide;

    [Header("交互功能_水：")]
    public GameObject switchButton;
    public event Action<bool> On_Off;
    void Start()
    {
        _panelAnim = FindObjectOfType<PanelAnim>();
        _panelAnim.allowGetTime += GetTime;

        if (player == null)
        {
            if (this.gameObject.name == "Player" || this.gameObject.tag == "Player")
            {
                player = this.gameObject;
            }
            else
            {
                player = GameObject.Find("Player");
            }
        }

        if (Star == null)
        {
            Star = GameObject.Find("Star");
        }

        if (woodBox == null)
        {
            if (this.gameObject.name == "woodBoxr" || this.gameObject.tag == "woodBox")
            {
                woodBox = this.gameObject;
            }
            else
            {
                woodBox = GameObject.Find("woodBox");
            }
        }

        starCount.text = "0";
        
        // 先获取父物体的子物体数量
        childCount = Star.transform.childCount;
        ChildCount.text = childCount.ToString();

        currentTime = 0f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        
        starCountInPass.text = childCount.ToString() + "/" + childCount.ToString();
        UpdateTimerDisplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            //bonk = true;
            if (childCount >= 0)
            {
                collectCount += 1;
                GameObject destroyChild = other.gameObject;
                Debug.Log("已收集" + destroyChild.name + ";" + collectCount + "/" + childCount);
                Destroy(destroyChild);
                starCount.text =collectCount.ToString();
            }
        }
        if (other.gameObject.tag == "woodBoxMoveTrigger")
        {
            woodBoxMove.SetActive(true);
            boxCollide?.Invoke(true);
        }

        if (other.gameObject.tag == "Switch")
        {
            switchButton.SetActive(true);
            On_Off?.Invoke(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "woodBoxMoveTrigger")
        {
            woodBoxMove.SetActive(false);
            boxCollide?.Invoke(false);
        }

        if (other.gameObject.tag == "Switch")
        {
            switchButton.SetActive(false);
            On_Off?.Invoke(false);
        }
    }

    private void UpdateTimerDisplay()
    {
        // 格式化为 分:秒.毫秒
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime % 1) * 1000);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeInPass.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void getTime()
    {
        timeText = timerText.text;
        pauseTime.text = timeText;
    }

    private void GetTime()
    {
        passTime = timeInPass.text;
        timeNum?.Invoke(passTime);
    }
}
