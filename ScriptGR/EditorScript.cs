using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorScript : EditorWindow
{
    static private List<GameObject> Label;
    static private GameObject[] Labels;
    static private GameObject[] Objects;

    #region Editor Label GUI


    [MenuItem("Window/3D Object Data")]
    static void RunLabelGenerator()
    {
        Labels = GameObject.FindGameObjectsWithTag("Label");
        Debug.Log(Labels[0].name);
        Label.AddRange(GameObject.FindGameObjectsWithTag("Label"));
        Debug.Log(Label[0].name);
        Objects[0] = GameObject.Find("table");
        Objects[1] = GameObject.Find("laptop");

        for (int i = 0; i < Label.Count; i++)
        {
            for(int j =0; j < Objects.Length; j++)
            {
                if(Label[i].name == Objects[j].name)
                    Instantiate(Objects[j], Label[i].transform.position, Quaternion.identity);
                    Debug.Log(Objects[j].name);
            }
        }
        Debug.Log("Editor Loading...");
    }
    #endregion

}
