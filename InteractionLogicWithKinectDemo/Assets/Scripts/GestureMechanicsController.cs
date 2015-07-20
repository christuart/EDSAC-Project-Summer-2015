using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GestureMechanicsController : MonoBehaviour {

	public Text trackedJointsText;

	private KinectManager km;
	private KinectWrapper.NuiSkeletonPositionIndex rightHand = KinectWrapper.NuiSkeletonPositionIndex.HandLeft; // remember, body is flipped as facing away from kinect
	private KinectWrapper.NuiSkeletonPositionIndex centreHip = KinectWrapper.NuiSkeletonPositionIndex.HipCenter;
	private KinectWrapper.NuiSkeletonPositionIndex centreShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
	private KinectWrapper.NuiSkeletonPositionIndex rightShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderLeft;
	private KinectWrapper.NuiSkeletonPositionIndex leftShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;
	private KinectWrapper.NuiSkeletonPositionIndex rightWrist = KinectWrapper.NuiSkeletonPositionIndex.WristLeft;
	
	private Vector3 wristVector;
	private Vector3 shouldersVector;
	private Vector3 backVector;

	void Start() {
		wristVector = new Vector3 (0.1f, -1f, 0f);
		shouldersVector = new Vector3 (1f, 0f, 0f);
		backVector = new Vector3 (0f, 1f, 0f);
	}

	// Update is called once per frame
	void Update () {
		km = KinectManager.Instance;
		string trackedJointsString = "Tracked Joints:";
		if (km != null) {
			if (km.IsUserDetected()) {
				uint userId = km.GetPlayer1ID();
				/*trackedJointsString += "\nchecking joints";
				if(km.IsJointTracked(userId, (int)rightHand))
					trackedJointsString += "\nRight hand";
				if(km.IsJointTracked(userId, (int)rightWrist))
					trackedJointsString += "\nRight wrist";
				if(km.IsJointTracked(userId, (int)centreHip))
					trackedJointsString += "\nCentre Hip";
				if(km.IsJointTracked(userId, (int)leftShoulder))
					trackedJointsString += "\nLeft shoulder";
				if(km.IsJointTracked(userId, (int)centreShoulder))
					trackedJointsString += "\nCentre shoulder";
				if(km.IsJointTracked(userId, (int)rightShoulder))
					trackedJointsString += "\nRight shoulder";
				trackedJointsString += "\ndone checking joints";*/

				if (km.IsJointTracked(userId, (int)rightHand) && km.IsJointTracked(userId, (int)rightShoulder))
					wristVector = km.GetJointPosition(userId, (int)rightHand) - km.GetJointPosition(userId, (int)rightShoulder);
				if (km.IsJointTracked(userId, (int)rightShoulder) && km.IsJointTracked(userId, (int)leftShoulder))
					shouldersVector = km.GetJointPosition(userId, (int)rightShoulder) - km.GetJointPosition(userId, (int)leftShoulder);
				if (km.IsJointTracked(userId, (int)centreShoulder) && km.IsJointTracked(userId, (int)centreHip))
					backVector = km.GetJointPosition(userId, (int)centreShoulder) - km.GetJointPosition(userId, (int)centreHip);

				//GramSchmidt Orthonormal Space
				Vector3 e2 = backVector.normalized;
				Vector3 e1 = (shouldersVector - Vector3.Dot (shouldersVector,e2) * e2).normalized;

				Vector2 wristVectorInPlane = new Vector2(Vector3.Dot(e1, wristVector), Vector3.Dot(e2, wristVector));
				if (wristVectorInPlane.y > 0 && wristVectorInPlane.y > (wristVectorInPlane.x / 2f))
					trackedJointsString += "\nPointing up right: " + Mathf.Rad2Deg * Mathf.Atan2 (wristVectorInPlane.y,wristVectorInPlane.x) + "degrees";

			}
		}
		trackedJointsText.text = trackedJointsString;
	}

}