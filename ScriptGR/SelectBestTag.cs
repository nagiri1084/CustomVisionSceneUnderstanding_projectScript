using UnityEngine;
using UnityEngine.UI;

public class SelectBestTag : MonoBehaviour
{
    private int tagIndex;

    public void ChooseButton()
    {
        tagIndex = int.Parse(this.GetComponent<Text>().text);
        CreateTagList.Instance.tagIndex = tagIndex;
        CreateTagList.Instance.SendChooseTag();
        //return tagIndex;
    }

}
