using UnityEngine;
using System.Collections;

public class ZoomReset : UpperRightAction {
	
	public CameraController[] cams;
	
	void Start() {
	}
	
	public override void Do() {
		foreach (CameraController cam in cams) {
			cam.ZoomReset();
		}
	}
}
