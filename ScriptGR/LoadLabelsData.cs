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
        //Microsoft HoloLens�� Windows ��ġ ���п� �ִ� ���� ������ �������� ��� ����
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
                //TextFileData �б�
                string LineName = fileReader.ReadLine();
                string LinePosData = fileReader.ReadLine();
                Debug.Log(LineName + ", " + LinePosData);

                //Label ��ġ ����
                List<string> LinePos = new List<string>();
                LinePos.AddRange(LinePosData.Split(separatorChar));

                //Label ���� �� �̸� ����
                GameObject temp1 = Instantiate(Label, this.transform.position, Quaternion.identity);
                temp1.transform.parent = this.transform;
                temp1.name = LineName;
                temp1.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
                temp1.transform.position = new Vector3(float.Parse(LinePos[0]), float.Parse(LinePos[1]), float.Parse(LinePos[2]));

                for (int i = 0; i < tagObjects.Length; i++)
                {
                    if (tagObjects[i].name == LineName)
                    {
                        //3D ������Ʈ ����
                        GameObject temp2 = Instantiate(tagObjects[i], temp1.transform.position, Quaternion.identity);
                        temp2.transform.parent = temp1.transform;
                    }
                }
            }
        }
    }
}
