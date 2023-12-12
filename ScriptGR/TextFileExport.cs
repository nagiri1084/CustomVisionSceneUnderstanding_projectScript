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

        //Microsoft HoloLens�� Windows ��ġ ���п� �ִ� ���� ������ �������� ��� ����
        var filePath = @"U:\Users\nagir\AppData\Local\Packages\Template3D_pzq3xp76mxafg\LocalState\_output.txt";
        fileWriter = new StreamWriter(filePath);

        LabelList = GameObject.FindGameObjectsWithTag("Label");
        for (int i = 0; i < LabelList.Length; i++)
        {
            //Background, platform, wall ���� �󺧸� ����
            Debug.Log(LabelList[i]);
            Transform LabelTransform = LabelList[i].transform;

            // Label �̸��� ��ġ ������ Text���Ͽ� �ۼ�
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
        //�� �ۼ��� ���� �ݱ�
        fileWriter.Close();
    }
}