using UnityEngine;
using System.Collections;

public class ViewPointMeshCameraController : MonoBehaviour {

	public bool usingViewPointMeshSystem = true;

	public ViewPointMeshVertex startingVertex;

	public float slideRate = 0.3f;

	private ViewPointMeshVertex vert;
	
	private Vector3 targetPosition;
	private Quaternion targetRotation;

	void Start() {
		if (startingVertex != null) {
			GoToVertex(startingVertex);
		}
	}

	void Update () {
		transform.position = Vector3.Lerp(transform.position,targetPosition,slideRate);
		transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,slideRate);
	}

	public void GoToVertex(ViewPointMeshVertex vert) {
		targetPosition = vert.transform.position;
		targetRotation = vert.transform.rotation;
	}

}
