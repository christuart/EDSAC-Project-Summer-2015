using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour {

	public float userPosition = 0.5f;

	private GameObject mainCamera;
	private Vector3 startOffset;
	private float calculatedPosition = -1f;

	void Start() {
		mainCamera = GameObject.FindGameObjectWithTag ("ShadowOverlayCamera");
		startOffset = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (calculatedPosition != userPosition) {
			calculatedPosition = userPosition;
//			RaycastHit r;
//
//			if (Physics.Raycast (mainCamera.GetComponent<Camera> ().ScreenPointToRay (new Vector2 (Screen.width * (0.5f + userPosition), 0.0f)), out r)) {
//				Vector3 direction = r.point - mainCamera.transform.position;
//				Vector3 directionProjectedOnto = mainCamera.transform.forward * Vector3.Dot (direction, mainCamera.transform.forward) + mainCamera.transform.right * Vector3.Dot (direction, mainCamera.transform.right);
//				Vector3 onlyRight = new Vector3(1f,0f,0f) * Vector3.Dot(directionProjectedOnto,mainCamera.transform.right);
//				transform.localPosition = startOffset + onlyRight * startOffset.z / Vector3.Dot(directionProjectedOnto,mainCamera.transform.forward);
//			}
			transform.localPosition = startOffset + 2 * mainCamera.transform.right * (userPosition-0.5f) * mainCamera.GetComponent<Camera>().orthographicSize *Screen.width/Screen.height;
		}
	}
	
	private Vector3 makeLiteralFromCamOrientation(Vector3 offset) {
		return 	offset.x * mainCamera.transform.right +
				offset.y * mainCamera.transform.up +
				offset.z * mainCamera.transform.forward;
	}
	private Vector3 makeRelativeToCamOrientation(Vector3 offset) {
		return new Vector3 (Vector3.Dot (offset, mainCamera.transform.right),
		             Vector3.Dot (offset, mainCamera.transform.up),
		             Vector3.Dot (offset, mainCamera.transform.forward));
	}
}
