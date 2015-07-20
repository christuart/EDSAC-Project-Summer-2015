using UnityEngine;
using System.Collections;

public class ZoomMax : UpperRightAction {

	public CameraController[] cams;
	
	public override void Do() {
		foreach (CameraController cam in cams) {
			cam.ZoomMax();
		}
	}
}
