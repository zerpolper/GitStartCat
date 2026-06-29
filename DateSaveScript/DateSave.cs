using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OfficeOpenXml;
using System;
using UnityEngine.UI;
using static UnityEngine.Rendering.ReloadAttribute;

public class DateSave : MonoBehaviour
{
    PlayerTrigger _playerTrigger;
    string savePath = "E:/testSavrData/timedata.xlsx";
    //string savePath = "D:/unity/StartCat/Assets/Data/timedata.xlsx";
    //string folderPath = Application.persistentDataPath + "/ExcelFiles";
    //string filePath;
    FileInfo fileInfo;
    private string currenttime;

    public GameObject newimage;
    private void Start()
    {
        
        _playerTrigger = FindObjectOfType<PlayerTrigger>();
        _playerTrigger.timeNum += GetTime;

        fileInfo = new FileInfo(savePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            package.Save();
        }
    }

    void Update()
    {
         
    }
    
    private void Save()
    {
        
    }

    private void GetTime(string passTime)
    {
        currenttime = passTime;
        StartCoroutine(SaveData(currenttime));
    }

    IEnumerator SaveData(string time)
    {
        //if (!File.Exists(savePath))
        //{

        //}
        if (fileInfo.Exists)
        {
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {

                ExcelWorksheet readWorksheet = package.Workbook.Worksheets[1];
                string getTime = readWorksheet.Cells[1, 1].Value.ToString();
                Debug.Log("表中数据:" + getTime);
                TimeSpan t1 = TimeSpan.Parse("00:" + getTime);  // "00:00:07"
                TimeSpan t2 = TimeSpan.Parse("00:" + time);  // "00:00:18"
                if (t1 > t2)
                {
                    ExcelWorksheet writeWorksheet = package.Workbook.Worksheets[1];
                    writeWorksheet.Cells[1, 1].Value = time;
                    newimage.SetActive(true);
                }

                //ExcelWorksheet writeWorksheet = package.Workbook.Worksheets[1];
                //writeWorksheet.Cells[1, 1].Value = time;
                //newimage.SetActive(true);
                package.Save();

            }
        }
        
        yield return null;
    }
}
