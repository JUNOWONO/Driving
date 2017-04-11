using UnityEngine;
using System.Collections;

public class dataTestScript : MonoBehaviour {

	public Data data;

	
	// Update is called once per frame
	void Update () {
		data.LoadData ();
		print (data.dataList[0].curNum);
	
	}
}
