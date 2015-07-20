using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float targetFieldOfView = 60f;
	public float zoomRate = 0.1f;
	
	public float defaultFieldOfView = 60;
	public float zoomIncrement = 10;
	public float maxZoom = 20;
	public float minZoom = 100;
	public float zoomThreshold = 2f;
	public float linearZoomRate = 0.1f;

	private Camera cam;
	private bool zooming = false;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (zooming) {
			cam.fieldOfView = Tools.LerpPlus(cam.fieldOfView, targetFieldOfView, zoomRate, zoomThreshold, linearZoomRate, ref zooming);
		}
	}
	
	public void ZoomMax() {
		targetFieldOfView = maxZoom;
		zooming = true;
	}
	public float ZoomIn() {
		targetFieldOfView = Mathf.Clamp (targetFieldOfView - zoomIncrement, maxZoom, minZoom);
		zooming = true;
		return targetFieldOfView;
	}
	public void ZoomReset() {
		targetFieldOfView = defaultFieldOfView;
		zooming = true;
	}
	public float ZoomOut() {
		targetFieldOfView = Mathf.Clamp (targetFieldOfView + zoomIncrement, maxZoom, minZoom);
		zooming = true;
		return targetFieldOfView;
	}
	public void ZoomMin() {
		targetFieldOfView = minZoom;
		zooming = true;
	}

}
