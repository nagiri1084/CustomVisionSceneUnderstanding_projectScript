using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateTagList : MonoBehaviour
{
    public static CreateTagList Instance;
    private float yValue = -10;
    private int i = 0;
    [SerializeField] public int tagIndex = 0;
    private GameObject rayObject;
    public TextMeshPro Label;
    [SerializeField] private RaycastHit hit;
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
    private void Update()
    {
        rayObject = GameObject.Find("CursorPress");
        if (rayObject)
        {
            if (Physics.Raycast(rayObject.transform.position, rayObject.transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "Label")
                {
                    Label.text = hit.transform.gameObject.GetComponent<TextMeshPro>().text;
                    Debug.Log(Label.text);
                }
            }
        }
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
            hit.transform.gameObject.GetComponent<TextMeshPro>().text = selectPredictions[tagIndex].tagName;
            CheckText.Instance.SetStatus(hit.transform.gameObject.GetComponent<TextMeshPro>().text);
            Debug.Log(hit.transform.gameObject.GetComponent<TextMeshPro>().text);
            SceneOrganiser.Instance.FinaliseLabel(selectPredictions[tagIndex]);
            CheckText.Instance.SetStatus(selectPredictions[tagIndex].tagName+", "+selectPredictions[tagIndex].probability.ToString());
        }
    }
}
