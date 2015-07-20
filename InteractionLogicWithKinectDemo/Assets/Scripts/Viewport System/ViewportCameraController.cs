using UnityEngine;
using System.Collections;

public class ViewportCameraController : MonoBehaviour {

	public bool split = false;
	public bool right = false;
	
	public float viewportSmoothness = 0.02f;
	public float viewportThreshold = 0.03f;

	private float targetViewportW = 1.0f;
	private Camera cam;
	private bool splitting = true;

	void Start() {
		cam = GetComponent<Camera> ();
	}

	// Update is called once per frame
	void Update() {
		if (splitting) {
			if (right) {
				cam.rect = new Rect (Mathf.Lerp (cam.rect.x, 1 - targetViewportW, viewportSmoothness),
				                     0f,
				                     Mathf.Lerp (cam.rect.width, targetViewportW, viewportSmoothness),
				                     1f);
			} else {
				cam.rect = new Rect (0f,
				                     0f,
				                     Mathf.Lerp (cam.rect.width, targetViewportW, viewportSmoothness),
				                     1f);
			}
			if (Mathf.Abs (targetViewportW - cam.rect.width) < viewportThreshold) {
				if (right) {
					cam.rect = new Rect (1 - targetViewportW,
					                     0f,
					                     targetViewportW,
					                     1f);
				} else {
					cam.rect = new Rect (0f,
					                     0f,
					                     targetViewportW,
					                     1f);
				}
				splitting = false;
			}
		}
	}

	public void ToggleSplit() {

		if (split) {
			targetViewportW = 1.0f;
			split = false;
			splitting = true;
		} else {
			targetViewportW = 0.51f;
			split = true;
			splitting = true;
		}
	}

	public bool IsSplitting() {
		return splitting;
	}

}
