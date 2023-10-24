using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTagList : MonoBehaviour
{
    public static CreateTagList Instance;
    private float yValue = -10;
    private int tagIndex = 0;
    [SerializeField] public RectTransform tagItem;
    [SerializeField] private ScrollRect scroll;
    private GameObject tagItemButton;
    List<Prediction> selectPredictions = new List<Prediction>();

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }
    public void AddTagList(List<Prediction> TagName)
    {
        if (TagName != null)
        {
            selectPredictions = TagName;
            while (TagName[tagIndex] != null)
            {
                var addTagItem = Instantiate(tagItem, scroll.content);
                addTagItem.anchoredPosition = new Vector2(0, yValue);
                GameObject tagItemName = addTagItem.transform.GetChild(0).gameObject;
                tagItemButton = addTagItem.transform.GetChild(1).GetChild(0).gameObject;

                tagItemName.GetComponent<Text>().text = TagName[tagIndex].tagName + ": " + TagName[tagIndex].probability;
                tagItemButton.GetComponent<Text>().text = tagIndex.ToString();
                Debug.Log(yValue);
                yValue -= 20;
                tagIndex++;
                //yValue -= addTagItem.sizeDelta.y; ;
            }
        }
    }
    //
    //public void SelectBestTag()
    //{
    //    if (selectPredictions != null)
    //    {
    //        int i = int.Parse(tagItemButton.GetComponent<Text>().text);
    //        SceneOrganiser.Instance.FinaliseLabel(selectPredictions[i]);
    //    }
    //}
}
