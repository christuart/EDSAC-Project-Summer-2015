using UnityEngine;
using System.Collections;

public class SetupGameController : MonoBehaviour {

	public string postSetupLevelName = "ModelRoomWithViewportSystem";
	public GUIText demoInstructionText;
	public GameObject calibrationIndicatorPrefab;
	public LineRenderer linePrefab;
	public GameObject userPrefab;
	
	public Transform topLeft;
	public Transform bottomRight;

	public float deepGap = 0.15f;
	public float wideGap = 0.1f;

	public string calibrationStartPointInstruction;
	public string calibrationPointInProgressInstruction;
	public string calibrationCompleteInstruction;

	private enum calibrationProgress { None, One_Started, One_Done, Two_Started, Two_Done, Three_Started, Three_Done, Four_Started, Done, Left_Calibration };
	private calibrationProgress progress;
	private GameObject calibrationIndicator;

	private Vector2 realWorldCalPointSum;
	private float calTimeStart;
	
	private Vector2[] calibrationPoints = new Vector2[4];
	
	private KinectManager kinectManager;
	
	public LineRenderer[] lines;

	// Use this for initialization
	void Start () {

		lines = new LineRenderer[6];

		if (linePrefab) {

			for(int i = 0; i < lines.Length; i++) {
				lines[i] = Instantiate(linePrefab) as LineRenderer;
				lines[i].transform.parent = transform;
				lines[i].gameObject.SetActive(false);
			}

		}
		
		calibrationPoints[0] = new Vector2 (0, 1);
		calibrationPoints[1] = new Vector2 (1, 1);
		calibrationPoints[2] = new Vector2 (1, 0);
		calibrationPoints[3] = new Vector2 (0, 0);

		calibrationStartPointInstruction = calibrationStartPointInstruction.Replace("\\n", "\n");
		calibrationPointInProgressInstruction = calibrationPointInProgressInstruction.Replace("\\n", "\n");
		calibrationCompleteInstruction = calibrationCompleteInstruction.Replace("\\n", "\n");

		demoInstructionText.text = calibrationStartPointInstruction;
		calibrationIndicator = createCalibrationTarget (new Vector2 (wideGap, 1-deepGap));
		progress = calibrationProgress.None;

	}
	
	void OnLevelWasLoaded () {
		
		if (Application.loadedLevelName == postSetupLevelName) {
			UserController userController = GameObject.FindGameObjectWithTag ("User").GetComponent<UserController>();
			userController.SetRealWorldSpace(calibrationPoints);
			Destroy (this);
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			calibrationPoints = new Vector2[] { new Vector2(0.6950065f,1.96167f), new Vector2(-0.8923265f,2.011135f), new Vector2(-1.026978f,3.05445f), new Vector2(1.254533f,3.204662f) };
			demoInstructionText.text = "";
			Application.LoadLevel (postSetupLevelName);
			progress = calibrationProgress.Left_Calibration;
		}

		kinectManager = KinectManager.Instance;
		uint playerID = kinectManager != null ? kinectManager.GetPlayer1ID() : 0;

		switch (progress) {
		case calibrationProgress.One_Started:
		case calibrationProgress.Two_Started:
		case calibrationProgress.Three_Started:
		case calibrationProgress.Four_Started:
			{
				Vector3 realWorldUserPosition = kinectManager.GetUserPosition(playerID);
				realWorldCalPointSum += Time.deltaTime * (new Vector2(realWorldUserPosition.x,realWorldUserPosition.z));
			}
			break;
		}

	}

	public void receiveGesture(KinectGestures.Gestures gesture) {
		switch (progress) {
		case calibrationProgress.None:
		case calibrationProgress.One_Done:
		case calibrationProgress.Two_Done:
		case calibrationProgress.Three_Done:
		{
			if (gesture == KinectGestures.Gestures.SwipeDown) {
				Destroy (calibrationIndicator);
				demoInstructionText.text = calibrationPointInProgressInstruction;
				realWorldCalPointSum = new Vector2();
				calTimeStart = Time.time;
				progress = (calibrationProgress)((int)progress + 1);
			}
		}
		break;
		case calibrationProgress.One_Started:
		case calibrationProgress.Two_Started:
		case calibrationProgress.Three_Started:
		case calibrationProgress.Four_Started:
		{
			if (gesture == KinectGestures.Gestures.SwipeUp) {
				switch (progress) {
				case calibrationProgress.One_Started:
					calibrationPoints[3] = realWorldCalPointSum / (Time.time - calTimeStart);
					calibrationIndicator = createCalibrationTarget (new Vector2 (1-wideGap, 1-deepGap));
					demoInstructionText.text = calibrationStartPointInstruction;
					break;
				case calibrationProgress.Two_Started:
					calibrationPoints[2] = realWorldCalPointSum / (Time.time - calTimeStart);
					calibrationIndicator = createCalibrationTarget (new Vector2 (1-wideGap, deepGap));
					demoInstructionText.text = calibrationStartPointInstruction;
					break;
				case calibrationProgress.Three_Started:
					calibrationPoints[1] = realWorldCalPointSum / (Time.time - calTimeStart);
					calibrationIndicator = createCalibrationTarget (new Vector2 (wideGap, deepGap));
					demoInstructionText.text = calibrationStartPointInstruction;
					break;
				case calibrationProgress.Four_Started:
					calibrationPoints[0] = realWorldCalPointSum / (Time.time - calTimeStart);
					if (linePrefab) {
						lines[0].SetPosition(0,new Vector3(	-2.5f	,3.8f	,2f	));
						lines[0].SetPosition(1,new Vector3(	2.5f	,3.8f	,2f	));
						lines[1].SetPosition(0,new Vector3(	0f		,2.3f	,2f	));
						lines[1].SetPosition(1,new Vector3(	0f		,5.3f		,2f	));
						lines[2].SetPosition(0,new Vector3(	calibrationPoints[0].x, 2.3f + calibrationPoints[0].y, 2f ));
						lines[2].SetPosition(1,new Vector3(	calibrationPoints[1].x, 2.3f + calibrationPoints[1].y, 2f ));
						lines[3].SetPosition(0,new Vector3(	calibrationPoints[1].x, 2.3f + calibrationPoints[1].y, 2f ));
						lines[3].SetPosition(1,new Vector3(	calibrationPoints[2].x, 2.3f + calibrationPoints[2].y, 2f ));
						lines[4].SetPosition(0,new Vector3(	calibrationPoints[2].x, 2.3f + calibrationPoints[2].y, 2f ));
						lines[4].SetPosition(1,new Vector3(	calibrationPoints[3].x, 2.3f + calibrationPoints[3].y, 2f ));
						lines[5].SetPosition(0,new Vector3(	calibrationPoints[3].x, 2.3f + calibrationPoints[3].y, 2f ));
						lines[5].SetPosition(1,new Vector3(	calibrationPoints[0].x, 2.3f + calibrationPoints[0].y, 2f ));
						for(int i = 0; i < lines.Length; i++)
							lines[i].gameObject.SetActive(true);
					}
					demoInstructionText.text = calibrationCompleteInstruction;
					break;
				}
				progress = (calibrationProgress)((int)progress + 1);
			}
		}
		break;
		case calibrationProgress.Done:
		{
			if (gesture == KinectGestures.Gestures.SwipeDown || gesture == KinectGestures.Gestures.SwipeUp) {
				for(int i = 0; i < lines.Length; i++)
					lines[i].gameObject.SetActive(false);
				demoInstructionText.text = "";
				Application.LoadLevel(postSetupLevelName);
				progress = calibrationProgress.Left_Calibration;
			}
		}
		break;
		default:
		break;
		}
	}

	private GameObject createCalibrationTarget(Vector2 uv) {

		Vector3 targetPosition = new Vector3 (0f, 0f, 0f);
		Quaternion targetRotation = new Quaternion();
		targetPosition.x = Mathf.Lerp (topLeft.position.x, bottomRight.position.x, uv.x);
		targetPosition.z = Mathf.Lerp (topLeft.position.z, bottomRight.position.z, uv.y);
		return Instantiate (calibrationIndicatorPrefab, targetPosition, targetRotation) as GameObject;

	}

}
/*
newPosition.x = Mathf.Lerp (topLeft.position.x, bottomRight.position.x, uv.x);
newPosition.z = Mathf.Lerp (topLeft.position.z, bottomRight.position.z, uv.y);
*/