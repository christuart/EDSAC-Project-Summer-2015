using UnityEngine;
using System.Collections;
using System;

public enum Scene { None, Setup, Model_Room };

public class GestureHandler : MonoBehaviour, KinectGestures.GestureListenerInterface {
	
	public SetupGameController setupGameController;
	public float timeBetweenContinuousGestures;

	public ModelRoomGameController gameController;
	
	private bool[] gestureInProgress;
	private float[] lastGestureTime;

	private Scene currentRoom;

	void Start() {
		currentRoom = Scene.Setup;
		gestureInProgress = new bool[Tools.IntPow(2,1+sizeof(KinectGestures.Gestures))];
		lastGestureTime = new float[Tools.IntPow(2,1+sizeof(KinectGestures.Gestures))];
		Debug.Log (Tools.IntPow(2,sizeof(KinectGestures.Gestures)));
		foreach (KinectGestures.Gestures gesture in Enum.GetValues(typeof(KinectGestures.Gestures))) {
			Debug.Log ((int)gesture);
			gestureInProgress[(int)gesture] = false;
			lastGestureTime[(int)gesture] = 0;
		}
	}

	void OnLevelWasLoaded () {
		
		switch (Application.loadedLevelName) {
		case "Setup":
			currentRoom = Scene.Setup;
			break;
		case "ModelRoomWithViewportSystem":
			foreach (GameObject controller in GameObject.FindGameObjectsWithTag("GameController")) {
				if (controller.GetComponent<ModelRoomGameController>() != null)
					gameController = controller.GetComponent<ModelRoomGameController>();
			}
			currentRoom = Scene.Model_Room;
			break;
		default:
			Debug.Log ("Room not found...");
			currentRoom = Scene.None;
			break;
		}
		
	}

	public void UserDetected(uint userId, int userIndex)
	{
	}
	
	public void UserLost(uint userId, int userIndex)
	{
	}
	
	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
		if (GestureIsContinuous(gesture) && progress > 0.3f) {
			if (!gestureInProgress[(int)gesture] || Time.time - lastGestureTime[(int)gesture] >= timeBetweenContinuousGestures) {
				gestureInProgress[(int)gesture] = true;
				lastGestureTime[(int)gesture] = Time.time;
				switch (currentRoom) {
				case Scene.Setup:
					currentRoom = Scene.Setup;
					setupGameController.ReceiveGesture (gesture);
					break;
				case Scene.Model_Room:
					currentRoom = Scene.Model_Room;
					gameController.ReceiveGesture (gesture);
					break;
				}
			}
		}
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
		Debug.Log ("A gesture!");
		switch (currentRoom) {
		case Scene.Setup:
			Debug.Log ("A gesture in setup no less.");
			currentRoom = Scene.Setup;
			setupGameController.ReceiveGesture (gesture);
			break;
		case Scene.Model_Room:
			currentRoom = Scene.Model_Room;
			gameController.ReceiveGesture (gesture);
			break;
		}
		return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint) {
		gestureInProgress[(int)gesture] = false;
		return true;
	}

	private bool GestureIsContinuous(KinectGestures.Gestures gesture) {
		return
			gesture == KinectGestures.Gestures.ZoomIn ||
			gesture == KinectGestures.Gestures.ZoomOut;
	}
}
