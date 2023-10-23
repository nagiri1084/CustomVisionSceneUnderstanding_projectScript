using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckText : MonoBehaviour
{
    public static CheckText Instance;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }
    public void SetStatus(string statusText)
    {
        if (statusText!=null)
        {
            GameObject display = this.gameObject;
            display.transform.localPosition = new Vector3(0, 0, 1);
            display.SetActive(true);
            display.transform.localScale = new Vector3(0.03f,0.03f,1.0f);
            display.transform.rotation = new Quaternion();
            TextMeshPro textMesh = display.GetComponent<TextMeshPro>();
            textMesh.GetComponent<TextMeshPro>().text = statusText;
            Debug.Log(statusText);
        }
    }
}
