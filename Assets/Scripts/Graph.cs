using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Graph : MonoBehaviour {

	public Data data;
	public int curNum;
	public int lastCurNum;
	public Vector3 vec3;

	public GameObject canvas;
	public GameObject image;
	public Text NumText;


	public Sprite sprite1; //GreenBall
	public Sprite sprite2; //YellowBall
	public Sprite sprite3; //RedBall
	// Use this for initialization
	private bool plag;
	private int count;

	void Start () {
		data.LoadData ();
		curNum = data.dataList [0].curNum;
		lastCurNum = curNum;
		NumText.text = curNum.ToString ();
		plag = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (plag == false) {
			lastCurNum = curNum;
			count = data.dataList [curNum].x.Count;
			for (int i = 0; i < count; i++) {
				GameObject newImage = Instantiate (image) as GameObject;
				newImage.transform.SetParent (canvas.transform, false);
				vec3 = new Vector3 (data.dataList [curNum].x [i], data.dataList [curNum].y [i], data.dataList [curNum].z [i]);

				float magnitude = 0;
				if (float.IsNaN (vec3.x) == false && float.IsNaN (vec3.y) == false && float.IsNaN (vec3.z) == false) {
					newImage.transform.position = vec3 * (300); // G -> -2~2  x->-1280~1280 y-> -720~720 Center is (o,o)
					newImage.transform.Translate(1280, 720,0);
					magnitude = vec3.sqrMagnitude;
				} 

				if (magnitude  <= 0.4) {
					newImage.GetComponent<Image> ().sprite = sprite1;

				} else if (magnitude <= 1.0) {
					newImage.GetComponent<Image> ().sprite = sprite2;

				} else {
					newImage.GetComponent<Image> ().sprite = sprite3;
				} 
		
				// if the value of var3 is over the background image, delete the circle.
				if(newImage.transform.position.x <= 1280-600 || newImage.transform.position.x >= 1280+600 || newImage.transform.position.y <= 720-600 || newImage.transform.position.y >= 720+300){
					Destroy (newImage);
				}
			}

			plag = true;
		}else if (lastCurNum != curNum) {
			plag = false;
			data.dataList [0].curNum = curNum;
			data.SaveData ();
			NumText.text = curNum.ToString ();
			Application.LoadLevel(Application.loadedLevel);
		}

	}

	public void goHome() {
		data.SaveData ();
		Application.LoadLevel("Start");
	}
}
