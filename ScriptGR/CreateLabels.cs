using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class CreateLabels : MonoBehaviour
{
    private StreamReader fileReader;
    char separatorChar = ',';
    public GameObject Label;
    //public GameObject suObject;
    // Start is called before the first frame update
    void Start()
    {
        //Microsoft HoloLens의 Windows 장치 포털에 있는 지도 관리자 페이지로 경로 설정
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileReader = new StreamReader(filePath);
        while(fileReader.Peek() >= 0)
        {
            //TextFileData 읽기
            string LineName = fileReader.ReadLine();
            string LineData = fileReader.ReadLine();
            Debug.Log(LineName + ", " + LineData);

            //Label 위치 설정
            float[] LinePos = new float[3];
            List<string> LinePosStr = new List<string>();
            LinePosStr.AddRange(LineData.Split(separatorChar));
            for(int i = 0; i < LinePosStr.Count; i++)
            {
                LinePos[i] = ConvertTofloat(LinePosStr[i]);
            }
            Debug.Log(LinePos[0] + ", " + LinePos[1] + ", " + LinePos[2]);

            //Label 생성 및 이름 설정
            GameObject temp = Instantiate(Label, this.transform.position, Quaternion.identity);
            temp.transform.parent = this.transform;
            temp.name = LineName;
            temp.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
            temp.transform.position = new Vector3(LinePos[0], LinePos[1], LinePos[2]);
        }

    }

    private float ConvertTofloat(string str)
    {
        Regex r = new Regex(@"[0-9]*\.*[0-9]+");
        Match m = r.Match(str);
        return float.Parse(m.Value);
    }
}
