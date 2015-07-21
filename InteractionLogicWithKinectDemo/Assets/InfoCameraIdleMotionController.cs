using UnityEngine;
using System.Collections;

public class InfoCameraIdleMotionController : MonoBehaviour {

	private Quaternion initialRotation;
	private Quaternion rotationOffset;

	// Use this for initialization
	void Start () {
		initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		//transform.rotation = initialRotation + new Quaternion(
	}
}
