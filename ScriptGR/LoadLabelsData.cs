using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class LoadLabelsData : MonoBehaviour
{
    public bool CreateLabel;
    private StreamReader fileReader;
    char separatorChar = ',';
    public GameObject Label;
    public GameObject suObject;
    private List<string> tagLabel = new List<string>();
    //public GameObject[] tagObjects;

    void Start()
    {
        //Microsoft HoloLens�� Windows ��ġ ���п� �ִ� ���� ������ �������� ��� ����
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileReader = new StreamReader(filePath);
        if (fileReader != null && CreateLabel == true)
        {
            while (fileReader.Peek() >= 0)
            {
                //TextFileData �б�
                string LineName = fileReader.ReadLine();
                string LinePosData = fileReader.ReadLine();
                Debug.Log(LineName + ", " + LinePosData);

                //3d ������Ʈ�� ���� tag Label ã��(�ߺ�����)
                if (tagLabel.Contains(LineName) == false) tagLabel.Add(LineName);


                //Label ��ġ ����
                List<string> LinePos = new List<string>();
                LinePos.AddRange(LinePosData.Split(separatorChar));

                //Label ���� �� �̸� ����
                GameObject temp1 = Instantiate(Label, this.transform.position, Quaternion.identity);
                temp1.transform.parent = this.transform;
                temp1.name = LineName;
                temp1.transform.gameObject.GetComponent<TextMeshPro>().text = LineName;
                temp1.transform.position = new Vector3(float.Parse(LinePos[0]), float.Parse(LinePos[1]), float.Parse(LinePos[2]));

                //for (int i = 0; i < tagObjects.Length; i++)
                //{
                //    if (tagObjects[i].name == LineName)
                //    {
                //        //3D ������Ʈ ����
                //        GameObject temp2 = Instantiate(tagObjects[i], temp1.transform.position, Quaternion.identity);
                //        temp2.transform.parent = PrefabRoot.transform;
                //    }
                //}
                for (int i = 0; i < tagLabel.Count; i++)
                {
                    Debug.Log(tagLabel[i]);
                    if (tagLabel[i] == LineName)
                    {
                        //3D ������Ʈ ����(Resources>Prefabs ���Ͽ� �±� �̸��� ������ ������Object�� �־�� ���� ������)
                        Debug.Log(tagLabel[i]);
                        GameObject temp2 = Instantiate(Resources.Load("Prefabs/"+tagLabel[i], typeof(GameObject)), temp1.transform.position, Quaternion.identity) as GameObject;
                        temp2.transform.parent = this.transform;
                    }
                }
            }
        }
    }
}
