using UnityEngine;
using System.Collections;

public class EveryplayTest : MonoBehaviour
{
    public bool showUploadStatus = true;
    private bool isRecording = false;
    private bool isPaused = false;
    private bool isRecordingFinished = false;
    private GUIText uploadStatusLabel;

    void Awake()
    {
        if (enabled && showUploadStatus)
        {
            CreateUploadStatusLabel();
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (uploadStatusLabel != null)
        {
            Everyplay.UploadDidStart += UploadDidStart;
            Everyplay.UploadDidProgress += UploadDidProgress;
            Everyplay.UploadDidComplete += UploadDidComplete;
        }

        Everyplay.RecordingStarted += RecordingStarted;
        Everyplay.RecordingStopped += RecordingStopped;
    }

    void Destroy()
    {
        if (uploadStatusLabel != null)
        {
            Everyplay.UploadDidStart -= UploadDidStart;
            Everyplay.UploadDidProgress -= UploadDidProgress;
            Everyplay.UploadDidComplete -= UploadDidComplete;
        }

        Everyplay.RecordingStarted -= RecordingStarted;
        Everyplay.RecordingStopped -= RecordingStopped;
    }

    private void RecordingStarted()
    {
        isRecording = true;
        isPaused = false;
        isRecordingFinished = false;
    }

    private void RecordingStopped()
    {
        isRecording = false;
        isRecordingFinished = true;
    }

    private void CreateUploadStatusLabel()
    {
        GameObject uploadStatusLabelObj = new GameObject("UploadStatus", typeof(GUIText));

        if (uploadStatusLabelObj)
        {
            uploadStatusLabelObj.transform.parent = transform;
            uploadStatusLabel = uploadStatusLabelObj.GetComponent<GUIText>();

            if (uploadStatusLabel != null)
            {
                uploadStatusLabel.anchor = TextAnchor.LowerLeft;
                uploadStatusLabel.alignment = TextAlignment.Left;
                uploadStatusLabel.text = "Not uploading";
            }
        }
    }

    private void UploadDidStart(int videoId)
    {
        uploadStatusLabel.text = "Upload " + videoId + " started.";
    }

    private void UploadDidProgress(int videoId, float progress)
    {
        uploadStatusLabel.text = "Upload " + videoId + " is " + Mathf.RoundToInt((float) progress * 100) + "% completed.";
    }

    private void UploadDidComplete(int videoId)
    {
        uploadStatusLabel.text = "Upload " + videoId + " completed.";

        StartCoroutine(ResetUploadStatusAfterDelay(2.0f));
    }

    private IEnumerator ResetUploadStatusAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);
        uploadStatusLabel.text = "Not uploading";
    }

    void OnGUI()
    {
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
		myButtonStyle.fontSize = 30;

		if (GUI.Button(new Rect(10*3, 10*3, 138*3, 48*3), "Everyplay",myButtonStyle))
        {
            Everyplay.Show();
            #if UNITY_EDITOR
            Debug.Log("Everyplay view is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }

		if (isRecording && GUI.Button(new Rect(10*3, 64*3, 138*3, 48*3), "Stop Recording"))
        {
            Everyplay.StopRecording();
            #if UNITY_EDITOR
            Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }
		else if (!isRecording && GUI.Button(new Rect(10*3, 64*3, 138*3, 48*3), "Start Recording",myButtonStyle))
        {
            Everyplay.StartRecording();
            #if UNITY_EDITOR
            Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }

        if (isRecording)
        {
			if (!isPaused && GUI.Button(new Rect(10*3 + 150*3, 64*3, 138*3, 48*3), "Pause Recording",myButtonStyle))
            {
                Everyplay.PauseRecording();
                isPaused = true;
                #if UNITY_EDITOR
                Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
                #endif
            }
			else if (isPaused && GUI.Button(new Rect(10*3 + 150*3, 64*3, 138*3, 48*3), "Resume Recording",myButtonStyle))
            {
                Everyplay.ResumeRecording();
                isPaused = false;
                #if UNITY_EDITOR
                Debug.Log("The video recording is not available in the Unity editor. Please compile and run on a device.");
                #endif
            }
        }

		if (isRecordingFinished && GUI.Button(new Rect(10*3, 118*3, 138*3, 48*3), "Play Last Recording",myButtonStyle))
        {
            Everyplay.PlayLastRecording();
            #if UNITY_EDITOR
            Debug.Log("The video playback is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }

		if (isRecording && GUI.Button(new Rect(10*3, 118*3, 138*3, 48*3), "Take Thumbnail",myButtonStyle))
        {
            Everyplay.TakeThumbnail();
            #if UNITY_EDITOR
            Debug.Log("Everyplay take thumbnail is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }

		if (isRecordingFinished && GUI.Button(new Rect(10*3, 172*3, 138*3, 48*3), "Show sharing modal",myButtonStyle))
        {
            Everyplay.ShowSharingModal();
            #if UNITY_EDITOR
            Debug.Log("The sharing modal is not available in the Unity editor. Please compile and run on a device.");
            #endif
        }
    }
}
