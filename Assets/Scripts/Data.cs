using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Data.dataArr[i].변수이름  <-이렇게 사용. (다른 스크립트에서 Data data; 로 선언 한 경우 data.dataArr[i].변수이름
//값이 변경 될 경우 반드시 Data.SaveData();을 붙여 줄 것 (데이터 수정 및 저장)
//값을 불러오기 전에는 반드시 Data.LoadData();를 먼저 불러 줄 것 
// Save롸 Load는 한 스크립트에서 한번 씩만 콜 해주면 됨 (LoadData() 는 시작 부분에 , SaveData는 값의 변경이 끝나는 부분에)
public class Data : MonoBehaviour {

	[Serializable]
	public class DataSet
	{
		public List<float> x = new List<float>();
		public List<float> y = new List<float>();
		public List<float> z = new List<float>();

		public int curNum = 1; //[0]에서만 사용. 현재 넘버

	}

	public List<DataSet> dataList = new List<DataSet>();

	public Text textField;

	//인덱스가 0일때는 항상 현재의 데이터(버퍼처럼)
	//public DataSet[] dataArr = new DataSet[31];

	public void SaveData()
	{
		var binaryFormatter = new BinaryFormatter ();
		var memoryStream = new MemoryStream ();

		binaryFormatter.Serialize (memoryStream, dataList); //dataArr을 바이너리 배열로 변환해서 저장

		//바이너리값을 다시 문자열 값으로 변환해서 "Data"라는 키값으로 플레이어프리펩에 저장
		PlayerPrefs.SetString ("Data01", Convert.ToBase64String (memoryStream.GetBuffer()));

	}	

	public void LoadData()
	{	
		var data = PlayerPrefs.GetString ("Data01");

		if (!string.IsNullOrEmpty (data)) {
			var binaryFormatter = new BinaryFormatter ();
			var memoryStream = new MemoryStream (Convert.FromBase64String (data));

			//가져온 데이터를 다시 바이너리 배열로 변환 후 DataSet클래스 타입 어레이로 다시 캐스팅
			dataList = (List<DataSet>)binaryFormatter.Deserialize (memoryStream);

		}
	}

	// Use this for initialization
	void Start ()
	{	
		var data = PlayerPrefs.GetString ("Data01");

		if (dataList.Count==0) {
			dataList.Add (new DataSet ());
		}

		if (!string.IsNullOrEmpty (data))
		{
			var binaryFormatter = new BinaryFormatter();
			var memoryStream = new MemoryStream(Convert.FromBase64String(data));

			//가져온 데이터를 다시 바이너리 배열로 변환 후 DataSet클래스 타입 어레이로 다시 캐스팅
			dataList = (List<DataSet>)binaryFormatter.Deserialize(memoryStream);

		}


	}

	public void changeFilterSensitivity(){
		int tmpInt;
		if (int.TryParse (textField.text, out tmpInt)){
			PlayerPrefs.SetInt("filter", tmpInt);

		} 
	}


}
