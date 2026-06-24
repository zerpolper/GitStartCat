using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    private GameObject player = null;
    public GameObject Star;
    private GameObject [] childStars;
    public Text starCount;
    public Text ChildCount;
    //private GameObject destroyChild;

    public bool bonk = false;

    [SerializeField] private int childCount = 0;
    [SerializeField] private int collectCount = 0;

    public Text timerText;       // 显示时间的UI文本
    public Text pauseTime;
    private float currentTime = 0f;
    private bool isTiming = false;
    private string timeText;

    //passPanel
    public Text starCountInPass;
    public Text timeInPass;
    void Start()
    {
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
}
