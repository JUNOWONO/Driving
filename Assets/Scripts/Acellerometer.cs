using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Acellerometer : MonoBehaviour {


	public float g=9.8f;
	public float x,y,z;
	Vector3 vec3;
	Vector3 ori; //original position of the ball
	Vector3 cal; 
	Vector3 accVec3;
	float yz1; //for Calculating
	float yz2; //for Calculating

	public Sprite sprite1; //GreenBall
	public Sprite sprite2; //YellowBall
	public Sprite sprite3; //RedBall
	public Data data;
	public int curNum; //current Number of Data


	//for testing
	public Text output1;
	public Text output2;
	public Text output3;
	private string str1;
	private string str2;
	private string str3;

	private float[] x_ = new float[4];  //buffer for x axis, ignore [0]
	private float[] y_ = new float[4];  
	private float[] z_ = new float[4];  

	public float dt = 0.075f; //시간 변화량
	public float k;//필터링 민감도
	private float resultX; //결과
	private float resultY; //결과
	private float resultZ; //결과
	private int count = 1; //1 2 3 loop count
	private bool plag = false;



	// Use this for initialization
	void Start () {

		data.LoadData ();
		curNum = data.dataList [0].curNum;
		//data.dataList [curNum].x.Clear();
		//data.dataList [curNum].y.Clear();
		//data.dataList [curNum].z.Clear();

		vec3 = Input.acceleration; //initial acceleration
		ori = transform.position;

		//dirVec3 = new Vector3(vec3.x/vec3.magnitude, vec3.y/vec3.magnitude , vec3.z/vec3.magnitude);

		yz1 = vec3.y /Mathf.Sqrt(vec3.y * vec3.y + vec3.z * vec3.z);
		yz2 = vec3.z /Mathf.Sqrt(vec3.y * vec3.y + vec3.z * vec3.z);
		k = (float)PlayerPrefs.GetInt("filter") / 1000.0f;
	}

	void FixedUpdate() {
		
		cal = Input.acceleration; //initially get the acceleration
		//yz2*cal.y - yz1*cal.z, cal.x, yz1*cal.y+ yz2*cal.z -1


		float magnitude=0;

		if (float.IsNaN (vec3.x) == false && float.IsNaN (vec3.y) == false && float.IsNaN (vec3.z) == false) {
			accVec3 = new Vector3 (-cal.x, yz2 * cal.y - yz1 * cal.z, yz1 * cal.y + yz2 * cal.z - 1); //xyz (-x)

			x_ [count] = accVec3.x;
			y_ [count] = accVec3.y;
			z_ [count] = accVec3.z;

			if (count == 1) {
				resultX = dt / (dt + k) * x_ [1] + k / (dt + k) * (dt * (x_ [2] - x_ [3]) + x_ [3]);
				resultY = dt / (dt + k) * y_ [1] + k / (dt + k) * (dt * (y_ [2] - y_ [3]) + y_ [3]);
				resultZ = dt / (dt + k) * z_ [1] + k / (dt + k) * (dt * (z_ [2] - z_ [3]) + z_ [3]);
			} else if (count == 2) {
				resultX = dt / (dt + k) * x_ [2] + k / (dt + k) * (dt * (x_ [3] - x_ [1]) + x_ [1]);
				resultY = dt / (dt + k) * y_ [2] + k / (dt + k) * (dt * (y_ [3] - y_ [1]) + y_ [1]);
				resultZ = dt / (dt + k) * z_ [2] + k / (dt + k) * (dt * (z_ [3] - z_ [1]) + z_ [1]);
			} else if (count == 3) {
				resultX = dt / (dt + k) * x_ [3] + k / (dt + k) * (dt * (x_ [1] - x_ [2]) + x_ [2]);
				resultY = dt / (dt + k) * y_ [3] + k / (dt + k) * (dt * (y_ [1] - y_ [2]) + y_ [2]);
				resultZ = dt / (dt + k) * z_ [3] + k / (dt + k) * (dt * (z_ [1] - z_ [2]) + z_ [2]);
			}
			accVec3 = new Vector3 (resultX,resultY,resultZ);

			transform.position = accVec3*(50); //set the position
			//transform.position = new Vector3(z+ori.x,x+ori.y,y+ori.z);
			magnitude = accVec3.sqrMagnitude; 

			/*
			필터링

				x1 = 전전
				x2 = 전
				x3 = 지금
				dt  = 0.075 //시간 변화량
				k  = 0.005 //필터링 민감도

				Result = dt / (dt + k) * x1 + k / (dt+k) * (dt * (x3 - x2) + x2)

				이거를 xyz축에 모두 적용 
			*/
			//-------------------------//
			if (count >= 3) {
				count = 1;
				plag = true;
			}
			count++;
			//--------------------------//
		} else {
			accVec3 = new Vector3 (0,0,0);
			transform.position = accVec3*(50);
			magnitude = accVec3.sqrMagnitude;
		}

		transform.Translate (ori.x, ori.y, ori.z); //set position to the original ball posiotion


		if (plag == true) { // do not play first 3times for fulfilling buffer
			if (magnitude <= 0.4) {
				GetComponent<Image> ().sprite = sprite1;
			
			} else if (magnitude <= 1.0) {
				GetComponent<Image> ().sprite = sprite2;
			
			} else {
				GetComponent<Image> ().sprite = sprite3;
			}

			x = accVec3.x;
			y = accVec3.y;
			z = accVec3.z;

			//put xyz accelerometer data into dataList
			data.dataList [curNum].x.Add (x);
			data.dataList [curNum].y.Add (y);
			data.dataList [curNum].z.Add (z);

			x = Mathf.Round (x / .01f) * .01f; //cut float number until seconde number under the point
			y = Mathf.Round (y / .01f) * .01f;
			z = Mathf.Round (z / .01f) * .01f;

			str1 = "x: " + x;
			str2 = "y: " + y;
			str3 = "z: " + z;
			output1.text = str1;
			output2.text = str2;
			output3.text = str3;
		}
		//data.dataList[curNum]


	}

	public void onClickSave() {

		data.SaveData ();
	}



	public void onClick() { //set the centor of acceleration with current state of phone
		vec3 = Input.acceleration; //initial acceleration

		yz1 = vec3.y /Mathf.Sqrt(vec3.y * vec3.y + vec3.z * vec3.z);
		yz2 = vec3.z /Mathf.Sqrt(vec3.y * vec3.y + vec3.z * vec3.z);
	}
}
