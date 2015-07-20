using UnityEngine;
using System.Collections;

public class ZoomIn : UpperRightAction {

	public CameraController[] cams;

	public override void Do() {
		foreach (CameraController cam in cams) {
			cam.ZoomIn();
		}
	}
}
