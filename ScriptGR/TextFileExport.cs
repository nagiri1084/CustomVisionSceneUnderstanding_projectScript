using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextFileExport : MonoBehaviour
{
    private StreamWriter fileWriter;

    [Header("Recoding")]
    public float recordingDuration = 2.0f;
    public float recordingInterval = 0.1f;
    public string recStartKey;

    [Header("RigidBody Parts")]
    public Transform rigidbody_Hand;
    public Transform rigidbody_Elbow;
    public Transform rigidbody_Shoulder;
    public Transform[] rigidbodyParts_L;

    [Header("Avatar Parts")]
    public Transform avatar_Hand;
    public Transform avatar_Elbow;
    public Transform avatar_Shoulder;
    public Transform[] avatarParts_L;

    [Header("Parts Error")]
    public Transform[] PartsError_L;

    // Start is called before the first frame update
    void Start()
    {
        //경로 수정
        string filePath = Path.Combine(Application.dataPath, "_output.txt");
        fileWriter = new StreamWriter(filePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(recStartKey))
        {
            StartRecording();
        }
    }

    public void StartRecording()
    {
        Debug.Log("Recoding Start");
        fileWriter.WriteLine("Recoding Start at " + Time.time);

        InvokeRepeating("RecordTransformData", 0f, recordingInterval);
        Invoke("StopRecording", recordingDuration);

        Debug.Log("Recoding Finished");
    }

    void RecordTransformData()
    {
        float testValue = rigidbody_Hand.transform.position.x - avatar_Hand.transform.position.x;
        float testAngle = rigidbody_Hand.transform.rotation.x - avatar_Hand.transform.rotation.x;

        // 이 부분 응용
        string transformData = Time.time + "\t" + testValue + "\t" + testAngle;
        fileWriter.WriteLine(transformData);
    }
    void StopRecording()
    {
        fileWriter.Close();
        CancelInvoke("RecordTransformData");
    }

    void OnDestroy()
    {
        fileWriter.Close();
    }
}
