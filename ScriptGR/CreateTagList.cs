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
    private GameObject storeHit;
    public TextMeshPro LabelUiTitle;
    public GameObject LabelPrefeb;
    //private Transform LabelCreatePos; //Real
    public Transform LabelCreatePos; //Test
    [SerializeField] private RaycastHit hit;
    [SerializeField] public RectTransform tagItem;
    [SerializeField] private ScrollRect scroll;
    private List<Prediction> selectPredictions = new List<Prediction>();

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton //Real
        Instance = this;
    }
    private void Start()
    {
        //LabelCreatePos = SceneOrganiser.Instance.cursor.transform;
    }
    private void Update()
    {
        rayObject = GameObject.Find("Right_ShellHandRayPointer(Clone)");
        if (rayObject)
        {
            if (Physics.Raycast(rayObject.transform.position, rayObject.transform.forward, out hit))
            {
                if (hit.transform.gameObject.tag == "Label")
                {
                    LabelUiTitle.text = hit.transform.gameObject.GetComponent<TextMeshPro>().text;
                    storeHit = hit.transform.gameObject;
                    Debug.Log(LabelUiTitle.text);
                }
            }
            Debug.DrawRay(rayObject.transform.position, rayObject.transform.forward, Color.red);
        }
    }

    public void AddTagList(List<Prediction> TagList)
    {
        CheckText.Instance.SetStatus("AddTagList"); //Real
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
                tagItemButtonTexts.GetComponent<Text>().text = (i+1).ToString();
                //Debug.Log(yValue);
                yValue -= 20;
                //yValue -= addTagItem.sizeDelta.y; ;
            }
        }
    }
    
    public void SendChooseTag()
    {
        CheckText.Instance.SetStatus("SendChooseTag"); //Real
        //Debug.Log(tagIndex-1);
        if (selectPredictions != null && tagIndex != 0)
        {
            storeHit.GetComponent<TextMeshPro>().text = selectPredictions[tagIndex-1].tagName;
            CheckText.Instance.SetStatus(storeHit.transform.gameObject.GetComponent<TextMeshPro>().text);
            SceneOrganiser.Instance.FinaliseLabel(selectPredictions[tagIndex - 1]);
            CreateSelectObject.Instance.InstantiateObject(selectPredictions[tagIndex - 1]);
            CheckText.Instance.SetStatus(selectPredictions[tagIndex-1].tagName+", "+selectPredictions[tagIndex-1].probability.ToString());
        }
    }

    public void CreateSelectedObjectLabel()
    {
        CheckText.Instance.SetStatus("CreateSelectedObjectLabel"); //Real
        if (selectPredictions != null && tagIndex != 0)
        {
            GameObject newLabel = Instantiate(LabelPrefeb, LabelCreatePos.position, Quaternion.identity);
            newLabel.GetComponent<TextMeshPro>().text = selectPredictions[tagIndex - 1].tagName;
        }
    }
}
