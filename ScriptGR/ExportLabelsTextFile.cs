﻿    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using System;
    using TMPro;

public class ExportLabelsTextFile : MonoBehaviour
{
    private StreamWriter fileWriter;
    private GameObject[] LabelList;
    //public Transform Player;
    //private GameObject PlayerRightHand;

    public void RecordLabelData()
    {
        //Microsoft HoloLens의 Windows 장치 포털에 있는 지도 관리자 페이지로 경로 설정
        var filePath = @"U:\Users\nagir\AppData\Local\Packages\Template3D_pzq3xp76mxafg\LocalState\_output.txt"; //Real
        //string filePath = Path.Combine(Application.dataPath, "_output.txt"); //Test
        fileWriter = new StreamWriter(filePath);

        //Player 이름과 위치 기록
        //fileWriter.WriteLine("PlayerPosition");
        //fileWriter.WriteLine(Player.position.x + "," + Player.position.y + "," + Player.position.z);
        //Debug.Log("PlayerPosition, " + Player.position.x + "," + Player.position.y + "," + Player.position.z);

        //Player 오른 손 이름과 위치 기록
        //PlayerRightHand = GameObject.Find("R_Wrist");
        //if (PlayerRightHand)
        //{
        //    fileWriter.WriteLine("PlayerRightHandPosition");
        //    fileWriter.WriteLine(PlayerRightHand.transform.position.x + "," + PlayerRightHand.transform.position.y + "," + PlayerRightHand.transform.position.z);
        //    Debug.Log("PlayerRightHandPosition, " + PlayerRightHand.transform.position.x + "," + PlayerRightHand.transform.position.y + "," + PlayerRightHand.transform.position.z);
        //}

        //LabelList = GameObject.FindGameObjectsWithTag("Object");
        LabelList = GameObject.FindGameObjectsWithTag("Label");

        for (int i = 0; i < LabelList.Length; i++)
        {
            Transform LabelTransform = LabelList[i].transform;

            //학습시켜둔 tag Label만 이름과 위치 데이터 Text파일에 작성
            //string LabelName = LabelTransform.name;
            string LabelName = LabelTransform.gameObject.GetComponent<TextMeshPro>().text;
            if (Array.Exists(CustomVisionAnalyser.Instance.tagName, x => x == LabelName)==true)
            {
                string LabelPosData = LabelTransform.position.x + "," + LabelTransform.position.y + "," + LabelTransform.position.z;
                fileWriter.WriteLine(LabelName);
                fileWriter.WriteLine(LabelPosData);
                Debug.Log(LabelName + ", " + LabelPosData);
            }
        }

        OnDestroy();
    }
    void OnDestroy()
    {
        //다 작성한 파일 닫기
        fileWriter.Close();
    }
}