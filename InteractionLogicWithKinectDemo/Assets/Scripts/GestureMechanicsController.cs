using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GestureMechanicsController : MonoBehaviour {

	public Text trackedJointsText;

	private KinectManager km;
	private KinectWrapper.NuiSkeletonPositionIndex rightHand = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
	private KinectWrapper.NuiSkeletonPositionIndex centreHip = KinectWrapper.NuiSkeletonPositionIndex.HipCenter;
	private KinectWrapper.NuiSkeletonPositionIndex centreShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderCenter;
	private KinectWrapper.NuiSkeletonPositionIndex rightShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderRight;
	private KinectWrapper.NuiSkeletonPositionIndex leftShoulder = KinectWrapper.NuiSkeletonPositionIndex.ShoulderLeft;
	private KinectWrapper.NuiSkeletonPositionIndex rightWrist = KinectWrapper.NuiSkeletonPositionIndex.WristRight;
	
	// Use this for initialization
	void Awake () {
		km = KinectManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		string trackedJointsString = "Tracked Joints:";
		if (km != null) {
			if (km.IsUserDetected()) {
				Debug.Log ("checking joints");
				uint userId = km.GetPlayer1ID();
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
				Debug.Log ("cone checking joints");

				
				Vector3 wristVector = km.GetRawSkeletonJointPos(userId, (int)rightWrist) - km.GetRawSkeletonJointPos(userId, (int)rightShoulder);
				Vector3 shouldersVector = km.GetRawSkeletonJointPos(userId, (int)rightShoulder) - km.GetRawSkeletonJointPos(userId, (int)leftShoulder);
				Vector3 backVector = km.GetRawSkeletonJointPos(userId, (int)centreShoulder) - km.GetRawSkeletonJointPos(userId, (int)centreHip);

				//GramSchmidt Orthonormal Space
				Vector3 e1 = shouldersVector.normalized;
				Vector3 e2 = (backVector - Vector3.Dot (backVector,e1) * e1).normalized;

				Vector2 wristVectorInPlane = new Vector2(Vector3.Dot(e1, wristVector), Vector3.Dot(e2, wristVector));
				if (wristVectorInPlane.y > 0 && wristVectorInPlane.y > (wristVectorInPlane.x / 2f))
					trackedJointsString += "\nPointing up right: " + Mathf.Rad2Deg * Mathf.Atan2 (wristVectorInPlane.y,wristVectorInPlane.x) + "degrees";

			}
			trackedJointsText.text = trackedJointsString;
		}
	}

}