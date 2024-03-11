using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class ImageCapture : MonoBehaviour
{
    /// <summary>
    /// Allows this class to behave like a singleton
    /// </summary>
    public static ImageCapture Instance;

    /// <summary>
    /// Keep counts of the taps for image renaming
    /// </summary>
    private int captureCount = 0;

    /// <summary>
    /// Photo Capture object
    /// </summary>
    private PhotoCapture photoCaptureObject = null;

    /// <summary>
    /// Flagging if the capture loop is running
    /// </summary>
    internal bool captureIsActive = false;

    /// <summary>
    /// File path of current analysed photo
    /// </summary>
    internal string filePath = string.Empty;

    /// <summary>
    /// Called on initialization
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Runs at initialization right after Awake method
    /// </summary>
    void Start()
    {
        // Clean up the LocalState folder of this application from all photos stored
        DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            try
            {
                file.Delete();
            }
            catch (Exception)
            {
                Debug.LogFormat("Cannot delete file: ", file.Name);
            }
        }
        SceneOrganiser.Instance.SetCameraStatus("Ready");
        CheckText.Instance.SetStatus("Ready");
    }

    /// <summary>
    /// Respond to Tap Input.
    /// </summary>
    public void TapHandler()
    {
        CreateTagList.Instance.ResetTagList();
        if (!captureIsActive)
        {
            captureIsActive = true;

            // Set the cursor color to red
            SceneOrganiser.Instance.cursor.GetComponent<Renderer>().material.color = Color.red;

            // Update camera status to looping capture.
            SceneOrganiser.Instance.SetCameraStatus("Looping Capture");

            // Begin the capture loop
            Invoke("ExecuteImageCaptureAndAnalysis", 0);
        }
        else
        {
            // The user tapped while the app was analyzing 
            // therefore stop the analysis process
            ResetImageCapture();
        }
    }

    /// <summary>
    /// Begin process of image capturing and send to Azure Custom Vision Service.
    /// </summary>
    private void ExecuteImageCaptureAndAnalysis()
    {
        // Update camera status to analysis.
        SceneOrganiser.Instance.SetCameraStatus("Analysis");
        CheckText.Instance.SetStatus("ExecuteImageCaptureAndAnalysis");

        // Create a label in world space using the ResultsLabel class 
        // Invisible at this point but correctly positioned where the image was taken
        SceneOrganiser.Instance.PlaceAnalysisLabel();
        CheckText.Instance.SetStatus("PlaceAnalysisLabel");

        // Set the camera resolution to be the highest possible
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending
            ((res) => res.width * res.height).First();
        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Begin capture process, set the image format
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            CheckText.Instance.SetStatus("PhotoCapture");
            photoCaptureObject = captureObject;

            CameraParameters camParameters = new CameraParameters
            {
                hologramOpacity = 1.0f,
                cameraResolutionWidth = targetTexture.width,
                cameraResolutionHeight = targetTexture.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };

            // Capture the image from the camera and save it in the App internal folder
            captureObject.StartPhotoModeAsync(camParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                string filename = string.Format(@"CapturedImage{0}.jpg", captureCount);
                filePath = Path.Combine(Application.persistentDataPath, filename);
                captureCount++;
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
            });
        });
        CheckText.Instance.SetStatus("finish capture process, set the image format");
    }

    /// <summary>
    /// Register the full execution of the Photo Capture. 
    /// </summary>
    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        try
        {
            // Call StopPhotoMode once the image has successfully captured
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
        catch (Exception e)
        {
            Debug.LogFormat("Exception capturing photo to disk: {0}", e.Message);
        }
    }

    /// <summary>
    /// The camera photo mode has stopped after the capture.
    /// Begin the image analysis process.
    /// </summary>
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.LogFormat("Stopped Photo Mode");

        // Dispose from the object in memory and request the image analysis 
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        // Call the image analysis
        StartCoroutine(CustomVisionAnalyser.Instance.AnalyseLastImageCaptured(filePath));
        CheckText.Instance.SetStatus(filePath);
        CheckText.Instance.SetStatus("AnalyseLastImageCaptured");
    }

    /// <summary>
    /// Stops all capture pending actions
    /// </summary>
    internal void ResetImageCapture()
    {
        captureIsActive = false;

        // Set the cursor color to green
        SceneOrganiser.Instance.cursor.GetComponent<Renderer>().material.color = Color.green;

        // Update camera status to ready.
        SceneOrganiser.Instance.SetCameraStatus("Ready");

        // Stop the capture loop if active
        //CancelInvoke();
    }
}
