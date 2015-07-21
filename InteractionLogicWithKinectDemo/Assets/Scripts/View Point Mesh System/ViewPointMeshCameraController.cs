﻿using UnityEngine;
using System.Collections;

public class ViewPointMeshCameraController : MonoBehaviour {

	public bool usingViewPointMeshSystem = true;

	public ViewPointMeshVertex startingVertex;

	public float slideRate = 0.3f;
	public float slideThresh = 0.02f;

	private ViewPointMeshVertex vert;
	
	private Vector3 targetPosition;
	private Quaternion targetRotation;

	void Start() {
		if (startingVertex != null && vert == null) {
			GoToVertex(startingVertex);
		}
	}

	void Update () {
		if ((transform.position-targetPosition).magnitude > slideThresh) {
			transform.position = Vector3.Lerp(transform.position,targetPosition,slideRate);
			transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,slideRate);
		}
	}

	public void GoToVertex(ViewPointMeshVertex target) {
		vert = target;
		targetPosition = target.transform.position;
		targetRotation = target.transform.rotation;
		Debug.Log ("Going to: " + targetPosition.ToString());
	}

}
