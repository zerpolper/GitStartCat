using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIControl : MonoBehaviour
{

    public GameObject SetPanel;
    public GameObject QualityHeightButton;
    public GameObject QualityButton;
    public GameObject VoiceHeightButton;
    public GameObject VoiceButton;
    public GameObject FunctionHeightButton;
    public GameObject FunctionButton;
    public GameObject MoreHeightButton;
    public GameObject MoreButton;

    public GameObject qualityPanel;
    public GameObject VoicePanel;
    public GameObject FunctionPanel;
    public GameObject MorePanel;
    private void Start()
    {
        //SetPanel.SetActive(false);
        //QualityButton.SetActive(false);
    }
    //显示Panel
    public void ShowPanel()
    {
        SetPanel.SetActive(true);
    }

    //隐藏Panel
    public void HidePanel()
    {
        SetPanel.SetActive(false);
    }

    //QualityHeight and Quality
    public void QualityHeightShow()
    {
        // activeSelf检查当前是否激活，然后取反
        QualityHeightButton.SetActive(true);
        qualityPanel.SetActive(true);
    }
    public void QualityHeightHide()
    {
        QualityHeightButton.SetActive(false);
        qualityPanel.SetActive(false);
    }

    public void QualityShow()
    {
        QualityButton.SetActive(true);
    }
    public void QualityHide()
    {
        QualityButton.SetActive(false);
    }

    //VoiceHeight and Voice
    public void VoiceHeightShow()
    {
        // activeSelf检查当前是否激活，然后取反
        VoiceHeightButton.SetActive(true);
        VoicePanel.SetActive(true);
    }
    public void VoiceHeightHide()
    {
        VoiceHeightButton.SetActive(false);
        VoicePanel.SetActive(false);
    }

    public void VoiceShow()
    {
        VoiceButton.SetActive(true);
    }
    public void VoiceHide()
    {
        VoiceButton.SetActive(false);
    }

    //FunctionHeight and Function
    public void FunctionHeightShow()
    {
        // activeSelf检查当前是否激活，然后取反
        FunctionHeightButton.SetActive(true);
        FunctionPanel.SetActive(true);
    }
    public void FunctionHeightHide()
    {
        FunctionHeightButton.SetActive(false);
        FunctionPanel.SetActive(false);
    }

    public void FunctionShow()
    {
        FunctionButton.SetActive(true);
    }
    public void FunctionHide()
    {
        FunctionButton.SetActive(false);
    }

    //MoreHeight and More
    public void MoreHeightShow()
    {
        // activeSelf检查当前是否激活，然后取反
        MoreHeightButton.SetActive(true);
        MorePanel.SetActive(true);
    }
    public void MoreHeightHide()
    {
        MoreHeightButton.SetActive(false);
        MorePanel.SetActive(false);
    }

    public void MoreShow()
    {
        MoreButton.SetActive(true);
    }
    public void MoreHide()
    {
        MoreButton.SetActive(false);
    }
}
