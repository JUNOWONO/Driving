using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour {


	public Data data;
	public Text curNumText;
	public string str;
	public int curNum;
	// Use this for initialization
	void Start () {
		data.LoadData ();
		if (data.dataList.Count < 1) {
			data.dataList.Add (new Data.DataSet ());
			data.SaveData ();
		}

		//curNum = data.dataList.Count-1;
		curNum = 1;
		curNum = data.dataList [0].curNum;
		str =" "+curNum.ToString();
		curNumText.text = str;


	}

	public void numRBtn(){
		if (curNum < data.dataList.Count) {
			curNum++;
			str = " " + curNum;
			curNumText.text = str;
		}
	}
	public void numLBtn(){
		curNum--;
		if (curNum <= 0)  {curNum = 1;}
		str =" "+curNum;
		curNumText.text = str;
	}
	public void numRRBtn(){
		if (curNum < data.dataList.Count - 10) {
			curNum = curNum + 10;
			str = " " + curNum;
			curNumText.text = str;
		} else {
			curNum = data.dataList.Count - 1;
			str = " " + curNum;
			curNumText.text = str;
		}
	}
	public void numLLBtn(){
		curNum = curNum -10;
		if (curNum <= 0)  {curNum = 1;}
		str =" "+curNum;
		curNumText.text = str;
	}

	public void StartButton() {
		data.dataList [0].curNum = curNum; //set dataList[0]'s curNum as current curNum which is slected

		if(data.dataList.Count -1 >= curNum){ 
			
			data.dataList [curNum].x.Clear();
			data.dataList [curNum].y.Clear();
			data.dataList [curNum].z.Clear();

			data.SaveData ();
			Application.LoadLevel("main"); 	
		} else if (data.dataList.Count-1 < curNum){ // if there not exist data
			//for (int i = 0; i <= curNum - data.dataList.Count + 1; i++) {
				data.dataList.Add (new Data.DataSet());
			//}
			data.SaveData ();
			Application.LoadLevel("main"); 
		}

	}

	public void GraphButton(){  //stop&save btn
		data.dataList [0].curNum = curNum; //set dataList[0]'s curNum as current curNum which is slected

		if(data.dataList.Count -1 >= curNum){ 
			data.SaveData ();
			Application.LoadLevel("Graph"); 	
		}
	

	}



	public void goHome() {
		data.SaveData ();
		Application.LoadLevel("Start");
	}

	public void Exit() {
		Application.Quit ();
	}

	void OnApplicationQuit()
	{
		Application.CancelQuit();
		#if !UNITY_EDITOR
		System.Diagnostics.Process.GetCurrentProcess().Kill();
		#endif
	}
}
