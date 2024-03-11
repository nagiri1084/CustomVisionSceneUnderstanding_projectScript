/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSelectObject : MonoBehaviour
{
    public static CreateSelectObject Instance;
    public GameObject[] detectObject = new GameObject[1];
    public Transform clonePos;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }

    public void InstantiateObject(Prediction bestPrediction)
    {
        for(int i = 0; i < detectObject.Length; i++) {
            if (detectObject[i].gameObject.name == bestPrediction.tagName)
            {
                Instantiate(detectObject[i], clonePos.position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/