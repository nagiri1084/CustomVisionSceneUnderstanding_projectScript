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
    private bool loadFileState = true;
    void Start()
    {
        //Microsoft HoloLens�� Windows ��ġ ���п� �ִ� ���� ������ �������� ��� ����
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileReader = new StreamReader(filePath);
        if (fileReader != null)
        {
            if (loadFileState == true)
            {
                while (fileReader.Peek() >= 0)
                {
                    //TextFileData �б�
                    string LineName = fileReader.ReadLine();
                    string LinePosData = fileReader.ReadLine();
                    //string LineRotData = fileReader.ReadLine();
                    //Debug.Log(LineName + ", " + LinePosData + ", " + LineRotData);

                    //Label ��ġ ����
                    //float[] LinePos = new float[3];
                    List<string> LinePos = new List<string>();
                    LinePos.AddRange(LinePosData.Split(separatorChar));
                    //List<string> LineRot = new List<string>();
                    //LineRot.AddRange(LinePosData.Split(separatorChar));

                    //Label ���� �� �̸� ����
                    GameObject temp = Instantiate(Label, this.transform.position, Quaternion.identity);
                    //GameObject temp = new GameObject();
                    temp.transform.parent = this.transform;
                    temp.name = LineName;
                    temp.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
                    //temp.transform.position = new Vector3(LinePos[0], LinePos[1], LinePos[2]);
                    temp.transform.position = new Vector3(float.Parse(LinePos[0]), float.Parse(LinePos[1]), float.Parse(LinePos[2]));
                    //temp.transform.rotation = new Quaternion(float.Parse(LineRot[0]), float.Parse(LineRot[1]), float.Parse(LineRot[2]), 0);
                }
                loadFileState = false;
            }
        }

    }
}
