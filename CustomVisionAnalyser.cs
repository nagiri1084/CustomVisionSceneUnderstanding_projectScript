using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class CustomVisionAnalyser : MonoBehaviour
{
    /// <summary>
    /// Split JsonFile
    /// </summary>
    char separatorChar = '"';
    public string[] tagName = new string[] { "chair", "swivelchair", "laptop", "table" };

    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.02f;

    /// <summary>
    /// Unique instance of this class
    /// </summary>
    public static CustomVisionAnalyser Instance;

    /// <summary>
    /// Insert your prediction key here
    /// </summary>
    private string predictionKey = "616811e1fabf47c6b09180dd83095164";

    /// <summary>
    /// Insert your prediction endpoint here
    /// </summary>
    private string predictionEndpoint = "https://azurecustomvisionproject-prediction.cognitiveservices.azure.com/customvision/v3.0/Prediction/c2ec8fb0-d34a-4abb-975f-b8e3ca5bee35/detect/iterations/Iteration1/image";

    /// <summary>
    /// Bite array of the image to submit for analysis
    /// </summary>
    [HideInInspector] public byte[] imageBytes;

    /// <summary>
    /// Initializes this class
    /// </summary>
    private void Awake()
    {
        // Allows this instance to behave like a singleton
        Instance = this;
    }

    /// <summary>
    /// Call the Computer Vision Service to submit the image.
    /// </summary>
    public IEnumerator AnalyseLastImageCaptured(string imagePath)
    {
        Debug.Log("Analyzing...");
        CheckText.Instance.SetStatus("Analyzing..");

        WWWForm webForm = new WWWForm();

        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(predictionEndpoint, webForm))
        {
            // Gets a byte array out of the saved image
            imageBytes = GetImageAsByteArray(imagePath);

            unityWebRequest.SetRequestHeader("Content-Type", "application/octet-stream");
            unityWebRequest.SetRequestHeader("Prediction-Key", predictionKey);

            // The upload handler will help uploading the byte array with the request
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.uploadHandler.contentType = "application/octet-stream";

            // The download handler will help receiving the analysis from Azure
            unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            CheckText.Instance.SetStatus("DownloadHandlerBuffer");

            //******************************************************************
            // Send the request
            yield return unityWebRequest.SendWebRequest();
            CheckText.Instance.SetStatus("Send the Request");

            string jsonResponse = unityWebRequest.downloadHandler.text;

            Debug.Log("response: " + jsonResponse);
            CheckText.Instance.SetStatus(jsonResponse);
            SplitJsonFile(jsonResponse);
        }
    }

    /// <summary>
    /// Returns the contents of the specified image file as a byte array.
    /// </summary>
    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);

        BinaryReader binaryReader = new BinaryReader(fileStream);

        return binaryReader.ReadBytes((int)fileStream.Length);
    }

    public void SplitJsonFile(string jsonFileData)
    {
        if (jsonFileData != null)
        {
            List<string> textLines = new List<string>();
            List<string> findTagName = new List<string>();
            List<int> tagOrder = new List<int>();
            List<Prediction> predictions = new List<Prediction> { };

            //textLines = jsonFileData.Split(separatorChar, System.StringSplitOptions.RemoveEmptyEntries);
            textLines.AddRange(jsonFileData.Split(separatorChar));
            Debug.Log(textLines);

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
                temp.probability = ConvertTofloat(textLines[tagOrder[i] - 7]);
                temp.boundingBox = new BoundingBox();
                temp.boundingBox.left = ConvertTofloat(textLines[tagOrder[i] + 5]);
                temp.boundingBox.top = ConvertTofloat(textLines[tagOrder[i] + 7]);
                temp.boundingBox.width = ConvertTofloat(textLines[tagOrder[i] + 9]);
                temp.boundingBox.height = ConvertTofloat(textLines[tagOrder[i] + 11]);
                CheckText.Instance.SetStatus(textLines[tagOrder[i] - 7]);
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
            sortedPredictions = predictions.OrderByDescending(p => p.probability).ToList();
            Prediction bestPrediction = new Prediction();
            bestPrediction = sortedPredictions[0];

            for (int i = 0; i < sortedPredictions.Count; i++)
            {
                if (sortedPredictions[i].probability > probabilityThreshold)
                {
                    Debug.Log(sortedPredictions[i].tagName + ", " + sortedPredictions[i].probability);
                }
            }
            CheckText.Instance.SetStatus(sortedPredictions[0].tagName + ", " + sortedPredictions[0].probability);

            if (bestPrediction != null)
            {
                SceneOrganiser.Instance.FinaliseLabel(bestPrediction);
                CheckText.Instance.SetStatus(bestPrediction.tagName+", "+bestPrediction.boundingBox.left);
            }
            else
                CheckText.Instance.SetStatus("analysisRootObject Null");
        }
    }
}