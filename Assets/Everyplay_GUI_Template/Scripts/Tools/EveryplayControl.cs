using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EveryplayControl : MonoBehaviour 
{
	//To automatically turn on FaceCam 
	//when recording permission is granted
	private bool startFaceCamWhenPermissionGranted = false;
	private bool isEveryplayPanelShow = false;
	private bool isFaceCameOn = false;
	public bool isDebugLogOn = true;
	
	public CanvasGroup everyplayPanelCanvasGroup;

	public Toggle ToogleRecord;
	public Button ButtonWatchReplays;
	public Button ButtonFaceCam;
	public Button ButtonShare;
	
	void Awake ()
	{
		Everyplay.RecordingStarted += RecordingStarted;
		Everyplay.RecordingStopped += RecordingStopped;
		Everyplay.ReadyForRecording += ReadyForRecording;
		Everyplay.FaceCamRecordingPermission += FaceCamRecordingPermission;

		Everyplay.FaceCamSetPreviewOrigin(Everyplay.FaceCamPreviewOrigin.TopLeft);
		//Everyplay.FaceCamSetPreviewBorderWidth(0);
		Everyplay.FaceCamSetPreviewSideWidth(60);
		Everyplay.FaceCamSetPreviewPositionY(85);

	}

	void DebugLog(string s)
	{
		if (isDebugLogOn) 
		{
			Debug.Log(s);
		}
	}

	void Start()
	{
		DebugLog("Everplay Control Start()!");

		if (!Everyplay.IsRecordingSupported())
		{
			ToogleRecord.interactable = false;
		}
		
		if (!Everyplay.FaceCamIsVideoRecordingSupported())
		{
			ButtonFaceCam.interactable = false;
		}
	}

	void Destroy()
	{
		DebugLog("Everyplay Destroy!");

		Everyplay.RecordingStarted -= RecordingStarted;
		Everyplay.RecordingStopped -= RecordingStopped;
		Everyplay.ReadyForRecording -= ReadyForRecording;
		Everyplay.FaceCamRecordingPermission -= FaceCamRecordingPermission;
	}

	public void ShowEveryplayPanel()
	{
		DebugLog("Show Everyplay Panel!");

		isEveryplayPanelShow = !isEveryplayPanelShow;

		everyplayPanelCanvasGroup.interactable = isEveryplayPanelShow;
		everyplayPanelCanvasGroup.blocksRaycasts = isEveryplayPanelShow;
		everyplayPanelCanvasGroup.alpha = isEveryplayPanelShow ? 1 : 0;
	}
	
	private void ReadyForRecording(bool enabled)
	{
		DebugLog("ReadyForRecording Event");

		ToogleRecord.interactable = enabled;
	}

	private void RecordingStarted()
	{
		DebugLog("RecordingStarted Event");

		if (ButtonShare != null) 
		{
			ButtonShare.interactable = false;
		}
		
		if (ButtonFaceCam != null) 
		{
			ButtonFaceCam.interactable = false;
		}
	}
	
	private void RecordingStopped()
	{
		DebugLog("RecordingStopped Event");

		if (ButtonShare != null) 
		{
			ButtonShare.interactable = true;
		}

		if (ButtonFaceCam != null) 
		{
			ButtonFaceCam.interactable = true;
		}
	}

	public void RecordToggle()
	{
		DebugLog ("Record Toggle!");

		if (ToogleRecord.isOn)
		{
			StartRecording();
		}
		else
		{
			StopRecording();
			ShareVideo();
		}
	}


	public void StartRecording()
	{
		DebugLog("StartRecording!");

		Everyplay.StartRecording();
	}

	public void StopRecording()
	{
		DebugLog("StopRecording!");
			
		Everyplay.StopRecording();
	}

	/// <summary>
	///Called after the Everyplay.FaceCamRequestRecordingPermission() call, 
	///when program has the result of a permission request
	/// </summary>
	private void FaceCamRecordingPermission(bool granted)
	{
		DebugLog ("FaceCamRecordingPermission Event");

		if (startFaceCamWhenPermissionGranted)
		{
			isFaceCameOn = granted;
			Everyplay.FaceCamStartSession();
			Everyplay.FaceCamSetAudioOnly (true); //added
			if (Everyplay.FaceCamIsSessionRunning())
			{
				startFaceCamWhenPermissionGranted = false;
			}
		}
	}
	
	public void FaceCamToggle()
	{
		DebugLog ("Face Cam Toggle!");

		if (Everyplay.FaceCamIsRecordingPermissionGranted())
		{
			isFaceCameOn = !isFaceCameOn;
			
			if (isFaceCameOn)
			{
				if (!Everyplay.FaceCamIsSessionRunning())
				{
					Everyplay.FaceCamStartSession();
					Everyplay.FaceCamSetAudioOnly (true); //added
				}
			}
			else
			{
				if (Everyplay.FaceCamIsSessionRunning())
				{
					Everyplay.FaceCamStopSession();
				}
			}
		}
		else //For the first time in the game we want to turn on the camera
		{
			Everyplay.FaceCamRequestRecordingPermission();
			startFaceCamWhenPermissionGranted = true;
		}
	}
	
	public void OpenEveryplay()
	{
		DebugLog ("Everplay Show!");

		Everyplay.Show();
	}

	public void SetMetadata(string key, object val)
	{
		DebugLog (string.Format ("Set Meta Data: {0}, {1}", key, val));
		
		Everyplay.SetMetadata(key, val);
	}

	private void EditVideo()
	{
		DebugLog ("Edit Video!");
		Everyplay.PlayLastRecording();
	}
	
	public void ShareVideo()
	{
		DebugLog ("Share Video!");
		Everyplay.ShowSharingModal();
	}

}