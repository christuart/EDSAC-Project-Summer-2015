using UnityEngine;
using System.Collections;

public class ViewportController : MonoBehaviour {
	
	public Camera cam;

	private MeshRenderer meshRenderer;
	private RenderTexture renderTexture;

	private ViewportCameraController camController;

	// Use this for initialization
	void Start () {
		meshRenderer = GetComponent<MeshRenderer> ();
		meshRenderer.material.SetTexture ("_MainTex", renderTexture);
	}

	public void ActivateViewport() {
		renderTexture = new RenderTexture (Mathf.RoundToInt(Screen.width*1.5f), Mathf.RoundToInt(Screen.height*1.5f), 24);
		cam.targetTexture = renderTexture;
	}

	public void DeactivateViewport() {
		cam.targetTexture = null;
	}

	public ViewportCameraController GetCameraController() {
		if (camController == null) {
			if (cam != null)
				camController = cam.gameObject.GetComponent<ViewportCameraController> ();
		}
		return camController;
	}

}
