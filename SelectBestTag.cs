using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBestTag : MonoBehaviour
{
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SelectBestTag Instance;
    private int tagIndex;

    public void ChooseButton()
    {
        tagIndex = int.Parse(this.GetComponent<Text>().text);
        CreateTagList.Instance.tagIndex = tagIndex;
        CreateTagList.Instance.SendChooseTag();
        //return tagIndex;
    }
}
