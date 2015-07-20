using UnityEngine;
using System.Collections;

public class TextClearOnStart : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<GUIText> ().text = "";
	}

}
