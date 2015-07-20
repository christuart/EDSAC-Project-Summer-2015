using UnityEngine;
using System.Collections;

public class HighlightController : MonoBehaviour {

	private GameObject highlighter;

	void Start() {
		highlighter = GameObject.FindGameObjectWithTag("Highlighter");
	}
	void OnMouseOver() {
		if (Input.GetMouseButton (0)) {
			Highlight();
		}
	}

	public void Highlight() {
		highlighter.SetActive (true);
		MeshFilter meshFilter = highlighter.GetComponent<MeshFilter> ();
		meshFilter.mesh = GetComponent<MeshFilter> ().mesh;
		highlighter.transform.position = transform.position;
		highlighter.transform.rotation = transform.rotation;
	}
}
