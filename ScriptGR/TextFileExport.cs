    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using TMPro;

public class TextFileExport : MonoBehaviour
{
    private StreamWriter fileWriter;
    private GameObject[] LabelList;
    public Transform Player;
    private GameObject PlayerRightHand;

    public void RecordLabelData()
    {
        //Microsoft HoloLens의 Windows 장치 포털에 있는 지도 관리자 페이지로 경로 설정
        //var filePath = @"U:\Users\nagir\AppData\Local\Packages\Template3D_pzq3xp76mxafg\LocalState\_output.txt";
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileWriter = new StreamWriter(filePath);

        //Player 이름과 위치 기록
        fileWriter.WriteLine("PlayerPosition");
        fileWriter.WriteLine(Player.position.x + "," + Player.position.y + "," + Player.position.z);
        fileWriter.WriteLine(Player.rotation.eulerAngles.x + "," + Player.rotation.eulerAngles.y + "," + Player.rotation.eulerAngles.z);
        Debug.Log("PlayerPosition, " + Player.position.x + "," + Player.position.y + "," + Player.position.z);
        Debug.Log("PlayerRotation, " + Player.rotation.eulerAngles.x + "," + Player.rotation.eulerAngles.y + "," + Player.rotation.eulerAngles.z);

        //Player 오른 손 이름과 위치 기록
        PlayerRightHand = GameObject.Find("R_Wrist");
        if (PlayerRightHand)
        {
            fileWriter.WriteLine("PlayerRightHandPosition");
            fileWriter.WriteLine(PlayerRightHand.transform.position.x + "," + PlayerRightHand.transform.position.y + "," + PlayerRightHand.transform.position.z);
            Debug.Log("PlayerRightHandPosition, " + PlayerRightHand.transform.position.x + "," + PlayerRightHand.transform.position.y + "," + PlayerRightHand.transform.position.z);
        }

        //LabelList = GameObject.FindGameObjectsWithTag("Object");
        LabelList = GameObject.FindGameObjectsWithTag("Label");

        for (int i = 0; i < LabelList.Length; i++)
        {
            //LabelList[i].rectTransform.position = new Vector3 (LabelList2[i].transform.position.x, LabelList2[i].transform.position.y, LabelList2[i].transform.position.z);
            //Background, platform, wall 정보 라벨만 들어옴
            Transform LabelTransform = LabelList[i].transform;

            // Label 이름과 위치 데이터 Text파일에 작성
            //string LabelName = LabelTransform.name;
            string LabelName = LabelTransform.gameObject.GetComponent<TextMeshPro>().text;
            string LabelPosData = LabelTransform.position.x + "," + LabelTransform.position.y + "," + LabelTransform.position.z;
            //string LabelRotData = LabelTransform.rotation.x + "," + LabelTransform.rotation.y + "," + LabelTransform.rotation.z;
            fileWriter.WriteLine(LabelName);
            fileWriter.WriteLine(LabelPosData);
            //fileWriter.WriteLine(LabelRotData);
            Debug.Log(LabelName + ", " + LabelPosData);
        }

        OnDestroy();
    }
    void OnDestroy()
    {
        //다 작성한 파일 닫기
        fileWriter.Close();
    }
}