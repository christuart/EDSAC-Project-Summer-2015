using UnityEngine;
using System.Collections;

public class ZoomOut : UpperRightAction {
	
	public CameraController[] cams;
	
	public override void Do() {
		foreach (CameraController cam in cams) {
			cam.ZoomOut();
		}
	}
}
