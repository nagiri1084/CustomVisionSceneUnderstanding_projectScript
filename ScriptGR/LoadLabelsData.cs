using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class LoadLabelsData : MonoBehaviour
{
    private StreamReader fileReader;
    char separatorChar = ',';
    public GameObject Label;
    public GameObject[] tagObjects;

    void Start()
    {
        //Microsoft HoloLens의 Windows 장치 포털에 있는 지도 관리자 페이지로 경로 설정
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileReader = new StreamReader(filePath);
        if (fileReader != null)
        {
            this.name = fileReader.ReadLine();
            List<string> suObjectPos = new List<string>();
            suObjectPos.AddRange(fileReader.ReadLine().Split(separatorChar));
            this.transform.position = new Vector3(float.Parse(suObjectPos[0]), float.Parse(suObjectPos[1]), float.Parse(suObjectPos[2]));

            while (fileReader.Peek() >= 0)
            {
                //TextFileData 읽기
                string LineName = fileReader.ReadLine();
                string LinePosData = fileReader.ReadLine();
                Debug.Log(LineName + ", " + LinePosData);

                //Label 위치 설정
                List<string> LinePos = new List<string>();
                LinePos.AddRange(LinePosData.Split(separatorChar));

                //Label 생성 및 이름 설정
                GameObject temp1 = Instantiate(Label, this.transform.position, Quaternion.identity);
                temp1.transform.parent = this.transform;
                temp1.name = LineName;
                temp1.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
                temp1.transform.position = new Vector3(float.Parse(LinePos[0]), float.Parse(LinePos[1]), float.Parse(LinePos[2]));

                for (int i = 0; i < tagObjects.Length; i++)
                {
                    if (tagObjects[i].name == LineName)
                    {
                        //3D 오브젝트 생성
                        GameObject temp2 = Instantiate(tagObjects[i], temp1.transform.position, Quaternion.identity);
                        temp2.transform.parent = temp1.transform;
                    }
                }
            }
        }
    }
}
