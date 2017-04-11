using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class nnn : MonoBehaviour {

	public Text textfield;


	// Update is called once per frame
	void Update () {
		this.textfield.text = ((float)PlayerPrefs.GetInt("filter") / 1000.0f).ToString();
	}
}
