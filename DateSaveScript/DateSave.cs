using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OfficeOpenXml;

public class DateSave : MonoBehaviour
{
    PlayerTrigger _playerTrigger;

    string savePath = "D:/unity/StartCat/Assets/Data/timedata.xlsx";
    //string folderPath = Application.persistentDataPath + "/ExcelFiles";
    //string filePath;
    FileInfo fileInfo;
    private string currenttime;
    private void Start()
    {
        
        _playerTrigger = FindObjectOfType<PlayerTrigger>();
        _playerTrigger.timeNum += GetTime;

        //if (!Directory.Exists(folderPath))
        //{
        //    Directory.CreateDirectory(folderPath);  // 创建文件夹
        //}
        //filePath = Path.Combine(folderPath, "timedata.xlsx");
        fileInfo = new FileInfo(savePath);
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
        if (fileInfo.Exists)
        {
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet_0 = package.Workbook.Worksheets.Add("Sheet1");
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[1, 1].Value = time;
                package.Save();
            }
        }
        
        yield return null;
    }
}
