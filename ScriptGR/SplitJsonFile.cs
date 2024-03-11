using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;

//Test용 Script
public class SplitJsonFile : MonoBehaviour
{
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SplitJsonFile Instance;

    /// <summary>
    /// Split JsonFile
    /// </summary>
    char separatorChar = '"';
    //태그를 받아오는 파일에서 가져올 수는 없을까..?
    //Interior
    public string[] tagName = new string[] { "bed", "chair", "closet", "door", "dressingTable", "hanger", "shelve", "table", "window"}; 
    //public string[] tagName = new string[] { "chair", "shelf", "table", "shelf", "board" }; //StudyRoom

    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.02f; //가져올 최소 정확도

    void Start()
    {
        FindTagName(this.gameObject.GetComponent<TextMesh>().text);
        Debug.Log(this.gameObject.GetComponent<TextMesh>().text);
    }

    public void FindTagName(string jsonFileData)
    {
        if (jsonFileData != null)
        {
            List<string> textLines = new List<string>();
            List<int> tagOrder = new List<int>();
            List<Prediction> predictions = new List<Prediction> { };

            //textLines = jsonFileData.Split(separatorChar, System.StringSplitOptions.RemoveEmptyEntries);
            textLines.AddRange(jsonFileData.Split(separatorChar));
            //CheckText.Instance.SetStatus(jsonFileData);

            for (int i = 0; i < textLines.Count; i++)
            {
                for (int j = 0; j < tagName.Length; j++)
                {
                    if (textLines[i] == tagName[j])
                    {
                        tagOrder.Add(i);
                    }
                }
            }

            for (int i = 0; i < tagOrder.Count; i++)
            {
                Prediction temp = new Prediction();
                temp.tagName = textLines[tagOrder[i]];
                temp.probability = ConvertTofloat(textLines[tagOrder[i] + 2]);
                Debug.Log(textLines[tagOrder[i] + 5]);
                Debug.Log(ConvertTofloat(textLines[tagOrder[i] + 5]));
                temp.boundingBox = new BoundingBox();
                temp.boundingBox.left = ConvertTofloat(textLines[tagOrder[i] + 5]);
                temp.boundingBox.top = ConvertTofloat(textLines[tagOrder[i] + 7]);
                temp.boundingBox.width = ConvertTofloat(textLines[tagOrder[i] + 9]);
                temp.boundingBox.height = ConvertTofloat(textLines[tagOrder[i] + 11]);
                Debug.Log(temp.boundingBox);
                predictions.Add(temp);
            }
            FindBestTag(predictions);
        }
    }

    private float ConvertTofloat(string str)
    {
        Regex r = new Regex(@"[0-9]*\.*[0-9]+");
        Match m = r.Match(str);
        return float.Parse(m.Value);
    }

    /// <summary>
    /// Set the Tags as Text of the last label created. 
    /// </summary>
    public void FindBestTag(List<Prediction> predictions)
    {
        if (predictions != null)
        {
            // Sort the predictions to locate the highest one
            List<Prediction> sortedPredictions = new List<Prediction>();
            sortedPredictions = predictions.OrderBy(p => p.probability).ToList();
            Prediction bestPrediction = new Prediction();
            bestPrediction = sortedPredictions[0];
            CreateTagList.Instance.AddTagList(sortedPredictions);

            for (int i = 0; i < sortedPredictions.Count; i++)
            {
                if (sortedPredictions[i].probability > probabilityThreshold)
                {
                    Debug.Log(sortedPredictions[i].tagName + ", " + sortedPredictions[i].probability);
                }
            }

            if (bestPrediction != null)
            {
                //CreateLabel.Instance.FinaliseLabel(bestPrediction);
                Debug.Log(bestPrediction.tagName);
            }
            else
                Debug.Log("analysisRootObject Null");
        }
    }
}
