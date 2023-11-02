using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTagList : MonoBehaviour
{
    public static CreateTagList Instance;
    private float yValue = -10;
    private int i = 0;
    public int tagIndex = 0;
    [SerializeField] public RectTransform tagItem;
    [SerializeField] private ScrollRect scroll;
    private List<Prediction> selectPredictions = new List<Prediction>();

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }

    public void AddTagList(List<Prediction> TagList)
    {
        //CheckText.Instance.SetStatus("AddTagList");
        if (TagList != null)
        {
            selectPredictions = TagList;
            for (int i = 0; i < TagList.Count; i++) 
            {
                var addTagItem = Instantiate(tagItem, scroll.content);
                addTagItem.anchoredPosition = new Vector2(0, yValue);
                GameObject tagItemName = addTagItem.transform.GetChild(0).gameObject;
                GameObject tagItemButtonTexts = addTagItem.transform.GetChild(1).GetChild(0).gameObject;

                tagItemName.GetComponent<Text>().text = TagList[i].tagName + ": " + TagList[i].probability;
                tagItemButtonTexts.GetComponent<Text>().text = i.ToString();
                //Debug.Log(yValue);
                yValue -= 20;
                //yValue -= addTagItem.sizeDelta.y; ;
            }
        }
    }
    
    public void SendChooseTag()
    {
        CheckText.Instance.SetStatus("SendChooseTag");
        Debug.Log(tagIndex);
        if (selectPredictions != null && tagIndex != 0)
        {
            Debug.Log(selectPredictions[tagIndex].tagName);
            SceneOrganiser.Instance.FinaliseLabel(selectPredictions[tagIndex]);
            CheckText.Instance.SetStatus(selectPredictions[tagIndex].tagName+", "+selectPredictions[tagIndex].probability.ToString());
        }
    }
}
