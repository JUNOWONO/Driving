using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphDataNum : MonoBehaviour {

	public Data data;
	public Text NumText;
	public Graph graph;
	public int curNum;

	// Use this for initialization
	void Start () {
		data.LoadData ();
		curNum = data.dataList [0].curNum;
		graph.curNum = curNum;
		NumText.text = curNum.ToString();
	}
	


	public void R(){
		if (curNum < data.dataList.Count) {
			curNum++;
			graph.curNum = curNum;
			NumText.text = curNum.ToString ();
		}
	}
	public void L(){
		curNum--;
		if (curNum <= 0)  {curNum = 1;}
		graph.curNum = curNum;
		NumText.text = curNum.ToString();
	}
	public void RR(){
		if (curNum < data.dataList.Count - 10) {
			curNum = curNum + 10;
			graph.curNum = curNum;
			NumText.text = curNum.ToString ();
		} else {
			curNum = data.dataList.Count - 1;
			graph.curNum = curNum;
			NumText.text = curNum.ToString ();
		}
	}
	public void LL(){
		curNum = curNum -10;
		if (curNum <= 0)  {curNum = 1;}
		graph.curNum = curNum;
		NumText.text = curNum.ToString();
	}
	public void goHome() {
		data.SaveData ();
		Application.LoadLevel("Start");
	}
}
