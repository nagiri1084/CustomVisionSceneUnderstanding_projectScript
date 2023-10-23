using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTagName : MonoBehaviour
{
    public static SelectTagName Instance;
    private int yValue = -10;
    public Image tagItem;
    [SerializeField] private ScrollRect scroll;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }
    public void AddTagList(Prediction TagName)
    {
        var addTagItem = Instantiate(tagItem, scroll.content);
        GameObject tagItemName = addTagItem.transform.GetChild(0).gameObject;

        tagItemName.GetComponent<Text>().text = TagName.tagName+": "+TagName.probability;
        Debug.Log(yValue);
        yValue -= 20;
    }
}
