using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchControl : MonoBehaviour
{
    private PlayerTrigger _playerTrigger;

    [Header("开关：")]
    [SerializeField]
    private GameObject switch_water;
    public Button switch_waterButton;

    [SerializeField]
    private bool isSwitch = false;
    [SerializeField]
    private bool hasSwitch = false;

    [Header("水")]
    public GameObject water;

    public float downSpeed = 1f;
    private Vector3 downPoint;
    private void Awake()
    {
        _playerTrigger = FindObjectOfType<PlayerTrigger>();
        _playerTrigger.On_Off += GetIsSwitch;
    }
    void Start()
    {
        if (switch_water == null)
        {
            if (this.gameObject.name == "switch" || this.gameObject.tag == "Switch")
            {
                switch_water = this.gameObject;
            }
            else
            {
                switch_water = GameObject.Find("switch");
            }
        }

        if (water == null)
        {
            water = GameObject.Find("StylizedWater");
        }
        downPoint = water.transform.position +new Vector3(0,-2.5f,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwitch && Input.GetKeyDown(KeyCode.F))
        {
            toRotate();
            
        }
        if (hasSwitch)
        {
            switch_waterButton.interactable = false;
        }

        if (!isSwitch && hasSwitch)
        {
            WaterToDown();
        }
    }

    public void UISwitch()
    {
        if (isSwitch)
        {
            toRotate();
        }
    }
    public void toRotate()
    {
        switch_water.transform.Rotate(90, 0, 0);
        switch_water.transform.Rotate(Vector3.right * 90);
        hasSwitch = true;
        isSwitch = false;
    }

    private void GetIsSwitch(bool toSwitch)
    {
        if (!hasSwitch)
        {
            isSwitch = toSwitch;
        }
    }

    private void WaterToDown()
    {
        //Debug.Log("水下降");
        float step = downSpeed * Time.deltaTime;
        water.transform.position = Vector3.Lerp(water.transform.position, downPoint, step);
    }
}
