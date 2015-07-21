using UnityEngine;
using System.Collections;

public class FloorInputController : MonoBehaviour {

	public UserController userController;
	public Camera cam;

	private RaycastHit rcH;

	void Update() {
		if (Input.GetMouseButton (0)) {
			if (pointInRect(cam.ScreenToViewportPoint(Input.mousePosition),new Rect(0,0,1,1))) {
				if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out rcH, 100f,LayerMask.GetMask("Kinect Environment Floor"))) {
					userController.SetUserLocation(rcH.textureCoord.x,rcH.textureCoord.y);
				}
			}
		}
	}

	bool pointInRect(Vector2 point, Rect rect) {
		return (point.x >= rect.xMin &&
			point.x <= rect.xMax &&
			point.y >= rect.yMin &&
			point.y <= rect.yMax);
	}
}
