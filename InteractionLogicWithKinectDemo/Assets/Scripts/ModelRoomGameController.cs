using UnityEngine;
using System.Collections;

public class ModelRoomGameController : MonoBehaviour {

	public int users = 1;

	public GameObject[] interestPoints;
	
	public ViewportController leftViewport;
	public ViewportController rightViewport;
	public Camera finalViewCamera;

	public ViewPointMesh[] viewPointMeshes;
	public int startViewPointMesh = -1;

	public ZonesController zonesController;

	public GameObject topRightIndicator;

	public UpperRightMenuController menuController;
	
	private int currentViewPointMesh;
	private ViewPointMeshVertex currentVertex;
	
	private GameObject[] userCameras;

	private bool splittingCameras = false;
	private bool viewPortModeActive = false;

	// Use this for initialization
	void Start () {

		userCameras = new GameObject[] { leftViewport.GetCameraController().gameObject, rightViewport.GetCameraController().gameObject };

		if (startViewPointMesh != -1) {
			ChangeActiveViewPointMesh(startViewPointMesh);
		}

		LeaveViewportMode ();

		zonesController.UpdateRegions();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Semicolon)) {
			users = 2;
			InitiateCameraSplit();
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			ViewPointMeshGoLeft();
		} else if (Input.GetKeyDown(KeyCode.D)) {
			ViewPointMeshGoRight();
		} else if (Input.GetKeyDown(KeyCode.W)) {
			ViewPointMeshGoUp();
		} else if (Input.GetKeyDown(KeyCode.S)) {
			ViewPointMeshGoDown();
		}
		if (Input.GetKeyDown(KeyCode.V)) {
			users = 2;
			if (viewPortModeActive) {
				LeaveViewportMode();
			} else {
				EnterViewportMode();
			}
		}
		if (splittingCameras) {
			if (!leftViewport.GetCameraController().IsSplitting() && !rightViewport.GetCameraController().IsSplitting()) {
				EndCameraSplit();
			}
		}
	}

	/*****************************
	// PUBLIC GAME EVENTS		*/
	public void OnOrbitLeft() {
		ViewPointMeshGoLeft();
	}
	public void OnOrbitRight() {
		ViewPointMeshGoRight();
	}
	public void OnOrbitUp() {
		ViewPointMeshGoUp();
	}
	public void OnOrbitDown() {
		ViewPointMeshGoDown();
	}
	public void OnPointTopRight() {
		menuController.Enter();
		topRightIndicator.SetActive(true);
	}
	public void OnStopPointingTopRight() {
		topRightIndicator.SetActive(false);
	}
	
	/*****************************
	// CAMERA SPLIT				*/
	
	private void InitiateCameraSplit() {
		//splittingCameras = true;
						//finalViewCamera.
		//EnterViewportMode();
		leftViewport.GetCameraController().ToggleSplit();
		rightViewport.GetCameraController().ToggleSplit();
	}
	private void EndCameraSplit() {
		splittingCameras = false;
		foreach (GameObject cam in userCameras) {
			cam.GetComponent<UnityStandardAssets.ImageEffects.DepthOfField>().enabled = false;
		}
		LeaveViewportMode();

	}



	/*****************************
	// VIEWPORTS 				*/

	private void EnterViewportMode() {
		if (users == 2) {
			leftViewport.ActivateViewport ();
			rightViewport.ActivateViewport ();
			finalViewCamera.gameObject.SetActive (true);
			viewPortModeActive = true;
		} else {
			Debug.Log ("Cancelled request for viewport mode as only 1 user present.");
		}
	}
	private void LeaveViewportMode() {
		leftViewport.DeactivateViewport ();
		rightViewport.DeactivateViewport ();
		finalViewCamera.gameObject.SetActive (false);
		viewPortModeActive = false;
	}

	/*****************************
	// VIEW POINT MESHES		*/
	
	private void ChangeActiveViewPointMesh(int newMeshIndex) {
		if (newMeshIndex < 0 || newMeshIndex >= viewPointMeshes.Length) {
			Debug.Log("Invalid mesh index supplied: " + newMeshIndex);
			return;
		}
		ChangeActiveViewPointMesh(newMeshIndex, viewPointMeshes[newMeshIndex].defaultVertex);
	}
	private void ChangeActiveViewPointMesh(int newMeshIndex, ViewPointMeshVertex startVertex) {
		if (newMeshIndex < 0 || newMeshIndex >= viewPointMeshes.Length) {
			Debug.Log("Invalid mesh index supplied: " + newMeshIndex);
			return;
		}

		ActivateViewPointMeshVertex (startVertex);

		currentViewPointMesh = newMeshIndex;
	}
	private void ActivateViewPointMeshVertex(ViewPointMeshVertex vert) {
		
		foreach (GameObject cam in userCameras) {
			cam.GetComponent<ViewPointMeshCameraController>().GoToVertex(vert);
		}

		currentVertex = vert;

	}
	private void ViewPointMeshGoLeft() {
		ActivateViewPointMeshVertex(currentVertex.Left ());
	}
	private void ViewPointMeshGoRight() {
		ActivateViewPointMeshVertex(currentVertex.Right ());
	}
	private void ViewPointMeshGoUp() {
		ActivateViewPointMeshVertex(currentVertex.Up ());
	}
	private void ViewPointMeshGoDown() {
		ActivateViewPointMeshVertex(currentVertex.Down ());
	}



}
