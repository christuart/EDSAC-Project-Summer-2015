using UnityEngine;
using System.Collections;

public class GestureHandler : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public SetupGameController gameController;

	public void UserDetected(uint userId, int userIndex)
	{
	}
	
	public void UserLost(uint userId, int userIndex)
	{
	}
	
	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos) {
		gameController.receiveGesture (gesture);
		return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		return true;
	}
}
