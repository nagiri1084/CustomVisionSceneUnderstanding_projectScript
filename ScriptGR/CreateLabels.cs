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
    //public string[] tagName = new string[] { "chair", "swivelchair", "laptop", "table" };
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
                //List<string> suObjectRot = new List<string>();
                //suObjectRot.AddRange(fileReader.ReadLine().Split(separatorChar));
                this.transform.position = new Vector3(float.Parse(suObjectPos[0]), float.Parse(suObjectPos[1]), float.Parse(suObjectPos[2]));
            //this.transform.rotation = Quaternion.Euler(float.Parse(suObjectRot[0]), float.Parse(suObjectRot[1]), float.Parse(suObjectRot[2]));
            while (fileReader.Peek() >= 0)
            {
                //TextFileData �б�
                string LineName = fileReader.ReadLine();
                string LinePosData = fileReader.ReadLine();
                //string LineRotData = fileReader.ReadLine();
                Debug.Log(LineName + ", " + LinePosData);

                for (int i = 0; i < tagObjects.Length; i++)
                {
                    if (tagObjects[i].name == LineName)
                    {
                        //Label ��ġ ����
                        //float[] LinePos = new float[3];
                        List<string> LinePos = new List<string>();
                        LinePos.AddRange(LinePosData.Split(separatorChar));
                        //List<string> LineRot = new List<string>();
                        //LineRot.AddRange(LinePosData.Split(separatorChar));

                        //Label ���� �� �̸� ����
                        GameObject temp1 = Instantiate(Label, this.transform.position, Quaternion.identity);
                        temp1.transform.parent = this.transform;
                        temp1.name = LineName;
                        temp1.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
                        temp1.transform.position = new Vector3(float.Parse(LinePos[0]), float.Parse(LinePos[1]), float.Parse(LinePos[2]));
                        //temp.transform.rotation = new Quaternion(float.Parse(LineRot[0]), float.Parse(LineRot[1]), float.Parse(LineRot[2]), 0);

                        //3D ������Ʈ ����
                        GameObject temp2 = Instantiate(tagObjects[i], temp1.transform.position, Quaternion.identity);
                        temp2.transform.parent = temp1.transform;
                    }
                }
            }
        }
    }
}
