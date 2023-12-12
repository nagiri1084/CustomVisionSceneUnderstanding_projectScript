    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using TMPro;

public class TextFileExport : MonoBehaviour
{
    private StreamWriter fileWriter;
    private GameObject[] LabelList;

    public void RecordLabelData()
    {

        //Microsoft HoloLens의 Windows 장치 포털에 있는 지도 관리자 페이지로 경로 설정
        var filePath = @"U:\Users\nagir\AppData\Local\Packages\Template3D_pzq3xp76mxafg\LocalState\_output.txt";
        fileWriter = new StreamWriter(filePath);

        LabelList = GameObject.FindGameObjectsWithTag("Label");
        for (int i = 0; i < LabelList.Length; i++)
        {
            //Background, platform, wall 정보 라벨만 들어옴
            Debug.Log(LabelList[i]);
            Transform LabelTransform = LabelList[i].transform;

            // Label 이름과 위치 데이터 Text파일에 작성
            string LabelName = LabelTransform.gameObject.GetComponent<TextMeshPro>().text;
            string LabelPosData = LabelTransform.position.x + "," + LabelTransform.position.y + "," + LabelTransform.position.z;
            fileWriter.WriteLine(LabelName);
            fileWriter.WriteLine(LabelPosData);
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