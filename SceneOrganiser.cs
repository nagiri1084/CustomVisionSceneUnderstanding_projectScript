using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneOrganiser : MonoBehaviour
{
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static SceneOrganiser Instance;

    /// <summary>
    /// The cursor object attached to the Main Camera
    /// </summary>
    internal GameObject cursor;

    /// <summary>
    /// The label used to display the analysis on the objects in the real world
    /// </summary>
    public GameObject label;

    /// <summary>
    /// Object providing the current status of the camera.
    /// </summary>
    internal TextMesh cameraStatusIndicator;


    /// <summary>
    /// Reference to the last Label positioned
    /// </summary>
    internal Transform lastLabelPlaced;

    /// <summary>
    /// Reference to the last Label positioned
    /// </summary>
    internal TextMesh lastLabelPlacedText;

    /// <summary>
    /// Current threshold accepted for displaying the label
    /// Reduce this value to display the recognition more often
    /// </summary>
    internal float probabilityThreshold = 0.02f;

    /// <summary>
    /// The quad object hosting the imposed image captured
    /// </summary>
    private GameObject quad;

    /// <summary>
    /// Renderer of the quad object
    /// </summary>
    internal Renderer quadRenderer;

    /// <summary>
    /// Called on initialization
    /// </summary>
    private void Awake()
    {
        // Use this class instance as singleton
        Instance = this;

        // Add the ImageCapture class to this Gameobject
        gameObject.AddComponent<ImageCapture>();

        // Add the CustomVisionAnalyser class to this Gameobject
        gameObject.AddComponent<CustomVisionAnalyser>();

        // Add the CustomVisionObjects class to this Gameobject
        gameObject.AddComponent<CustomVisionObjects>();

        // Create the camera status indicator label, and place it above where predictions
        // and training UI will appear.
        cameraStatusIndicator = CreateTrainingUI("Status Indicator", 0.02f, -0.2f, 3, true);

        // Set camera status indicator to loading.
        SetCameraStatus("Loading");
    }

    /// <summary>
    /// Set the camera status to a provided string. Will be coloured if it matches a keyword.
    /// </summary>
    /// <param name="statusText">Input string</param>
    public void SetCameraStatus(string statusText)
    {
        if (string.IsNullOrEmpty(statusText) == false)
        {
            string message = "white";

            switch (statusText.ToLower())
            {
                case "loading":
                    message = "yellow";
                    break;

                case "ready":
                    message = "green";
                    break;

                case "uploading image":
                    message = "red";
                    break;

                case "looping capture":
                    message = "yellow";
                    break;

                case "analysis":
                    message = "red";
                    break;
            }

            cameraStatusIndicator.GetComponent<TextMesh>().text = $"Camera Status:\n<color={message}>{statusText}..</color>";
        }
    }

    /// <summary>
    /// Create a 3D Text Mesh in scene, with various parameters.
    /// </summary>
    /// <param name="name">name of object</param>
    /// <param name="scale">scale of object (i.e. 0.04f)</param>
    /// <param name="yPos">height above the cursor (i.e. 0.3f</param>
    /// <param name="zPos">distance from the camera</param>
    /// <param name="setActive">whether the text mesh should be visible when it has been created</param>
    /// <returns>Returns a 3D text mesh within the scene</returns>
    internal TextMesh CreateTrainingUI(string name, float scale, float yPos, float zPos, bool setActive)
    {
        GameObject display = new GameObject(name, typeof(TextMesh));
        display.transform.parent = Camera.main.transform;
        display.transform.localPosition = new Vector3(0, yPos, zPos);
        display.SetActive(setActive);
        display.transform.localScale = new Vector3(scale, scale, scale);
        display.transform.rotation = new Quaternion();
        TextMesh textMesh = display.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        return textMesh;
    }

    /// <summary>
    /// Instantiate a Label in the appropriate location relative to the Main Camera.
    /// </summary>
    public void PlaceAnalysisLabel()
    {
        lastLabelPlaced = Instantiate(label.transform, cursor.transform.position, transform.rotation);
        lastLabelPlacedText = lastLabelPlaced.GetComponent<TextMesh>();
        lastLabelPlaced.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

        // Create a GameObject to which the texture can be applied
        quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        Material m = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
        quadRenderer.material = m;

        // Here you can set the transparency of the quad. Useful for debugging
        float transparency = 0f;
        quadRenderer.material.color = new Color(1, 1, 1, transparency);

        // Set the position and scale of the quad depending on user position
        quad.transform.parent = transform;
        quad.transform.rotation = transform.rotation;

        // The quad is positioned slightly forward in font of the user
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);

        // The quad scale as been set with the following value following experimentation,  
        // to allow the image on the quad to be as precisely imposed to the real world as possible
        quad.transform.localScale = new Vector3(3f, 1.65f, 1f);
        quad.transform.parent = null;
    }


    /// <summary>
    /// Set the Tags as Text of the last label created. 
    /// </summary>
    public void FinaliseLabel(Prediction bestPrediction)
    {
        Debug.Log("FinaliseLabel:"+bestPrediction.tagName);
        CheckText.Instance.SetStatus("FinaliseLabel1");
        if (bestPrediction != null)
        {
            CheckText.Instance.SetStatus("FinaliseLabel2");
            lastLabelPlacedText = lastLabelPlaced.GetComponent<TextMesh>();

            CheckText.Instance.SetStatus("quadRenderer");
            quadRenderer = quad.GetComponent<Renderer>() as Renderer;
            Bounds quadBounds = quadRenderer.bounds;

            // Position the label as close as possible to the Bounding Box of the prediction 
            // At this point it will not consider depth
            CheckText.Instance.SetStatus("Bounding Box of the prediction ");
            lastLabelPlaced.transform.parent = quad.transform;
            lastLabelPlaced.transform.localPosition = CalculateBoundingBoxPosition(quadBounds, bestPrediction.boundingBox);

            // Set the tag text
            if (bestPrediction.tagName != null)
            {
                lastLabelPlacedText.text = bestPrediction.tagName;
                CheckText.Instance.SetStatus(bestPrediction.tagName + "Exist!");
                Debug.Log(bestPrediction.tagName + "Exist!");
            }
            else
                CheckText.Instance.SetStatus("bestPrediction.tagName Null");

            // Cast a ray from the user's head to the currently placed label, it should hit the object detected by the Service.
            // At that point it will reposition the label where the ray HL sensor collides with the object,
            // (using the HL spatial tracking)
            CheckText.Instance.SetStatus("FinaliseLabel4");
            Vector3 headPosition = Camera.main.transform.position;
            RaycastHit objHitInfo;
            Vector3 objDirection = lastLabelPlaced.position;
        }
        // Reset the color of the cursor
        cursor.GetComponent<Renderer>().material.color = Color.green;

        // Stop the analysis process
        ImageCapture.Instance.ResetImageCapture();
    }

    /// <summary>
    /// This method hosts a series of calculations to determine the position 
    /// of the Bounding Box on the quad created in the real world
    /// by using the Bounding Box received back alongside the Best Prediction
    /// </summary>
    public Vector3 CalculateBoundingBoxPosition(Bounds b, BoundingBox boundingBox)
    {
        Debug.Log($"BB: left {boundingBox.left}, top {boundingBox.top}, width {boundingBox.width}, height {boundingBox.height}");

        float centerFromLeft = boundingBox.left + (boundingBox.width / 2);
        float centerFromTop = boundingBox.top + (boundingBox.height / 2);
        Debug.Log($"BB CenterFromLeft {centerFromLeft}, CenterFromTop {centerFromTop}");

        float quadWidth = b.size.normalized.x;
        float quadHeight = b.size.normalized.y;
        Debug.Log($"Quad Width {b.size.normalized.x}, Quad Height {b.size.normalized.y}");

        float normalisedPos_X = (quadWidth * centerFromLeft) - (quadWidth / 2);
        float normalisedPos_Y = (quadHeight * centerFromTop) - (quadHeight / 2);

        return new Vector3((float)normalisedPos_X, (float)normalisedPos_Y, 0);
    }
}
