using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public WebCamTexture mCamera = null;
	public GameObject plane;
	public RawImage rawImage;

	// Use this for initialization
	void Start () {
		/*
		plane = GameObject.FindWithTag ("Player"); //태그로 찾기 


		mCamera = new WebCamTexture (1280, 720);
		plane.GetComponent<Renderer>().material.mainTexture = mCamera;
		mCamera.Play ();
		*/
		WebCamTexture webcamTexture = new WebCamTexture();
		rawImage.texture = webcamTexture;
		rawImage.material.mainTexture = webcamTexture;
		webcamTexture.Play();

	}

	// Update is called once per frame
	void Update () {
	
	}
}
