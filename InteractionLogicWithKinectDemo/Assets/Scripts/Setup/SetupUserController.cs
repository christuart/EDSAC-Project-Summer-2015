using UnityEngine;
using System.Collections;

public class SetupUserController : MonoBehaviour {

	public Transform topLeft;
	public Transform bottomRight;
	
	public float leftMostRealWorldX = -0.5f;
	public float rightMostRealWorldX = 0.5f;
	public float closestRealWorldZ = 1f;
	public float furthestRealWorldZ = 3f;

	public Vector2[] realWorldQuad = new Vector2[4];
	public Vector2 realWorldPosition = new Vector2 (0f, 0f);
	public Vector2 projectedPosition = new Vector2 (0f, 0f);
	
	private Vector2[] normals = new Vector2[4];

	private bool started = false;
	private bool quadSuppliedOnInstantiation = false;
	private bool usingQuadForLocation = false;

	private KinectManager kinectManager;

	void Start() {

		started = true;
		topLeft = GameObject.FindGameObjectWithTag ("TopLeftCorner").transform;
		bottomRight = GameObject.FindGameObjectWithTag ("BottomRightCorner").transform;

		if (quadSuppliedOnInstantiation) {
			SetRealWorldSpace (realWorldQuad);
		} else {
			for (int i = 0; i < 4; i++) {
				realWorldQuad [i] = new Vector2 (0, 0);
			}
			SetRealWorldSpace( new Vector2[] {topLeft.position, bottomRight.position});
		}

	}
	
	void Update() {

		kinectManager = KinectManager.Instance;

		uint playerID = kinectManager != null ? kinectManager.GetPlayer1ID() : 0;

		Vector3 realWorldUserPosition = kinectManager.GetUserPosition(playerID);
		Vector2 inGameUserPosition;

		if (!usingQuadForLocation) {
			inGameUserPosition = new Vector2 (
				0.5f - Mathf.InverseLerp (leftMostRealWorldX, rightMostRealWorldX, realWorldUserPosition.x),
				Mathf.InverseLerp (closestRealWorldZ, furthestRealWorldZ, realWorldUserPosition.z));
		} else {
			inGameUserPosition = ProjectRealWorldOntoSquare (new Vector2 (realWorldUserPosition.x, realWorldUserPosition.z));
		}

		SetUserLocation (inGameUserPosition);

	}
	
	public void SetUserLocation(float u, float v) {
		SetUserLocation (new Vector2 (u, v));
	}

	public void SetUserLocation(Vector2 uv) {

		projectedPosition = uv;

		Vector3 newPosition = new Vector3 (0, transform.position.y, 0); 
		newPosition.x = Mathf.Lerp (topLeft.position.x, bottomRight.position.x, uv.x);
		newPosition.z = Mathf.Lerp (topLeft.position.z, bottomRight.position.z, uv.y);
		
		transform.position = newPosition;
		
	}

	public void SetRealWorldSpace(Vector2[] quad) {
		if (quad.Length == 2) {

			usingQuadForLocation = false;
			leftMostRealWorldX = quad [0].x;
			rightMostRealWorldX = quad [1].x;
			closestRealWorldZ = quad [0].y;
			furthestRealWorldZ = quad [1].y;
			if (leftMostRealWorldX > rightMostRealWorldX) {
				rightMostRealWorldX = leftMostRealWorldX;
				leftMostRealWorldX = quad [1].x;
			}
			if (closestRealWorldZ > furthestRealWorldZ) {
				furthestRealWorldZ = closestRealWorldZ;
				closestRealWorldZ = quad [1].y;
			}

		} else if (quad.Length == 4) {

			if (!started) {
				realWorldQuad = quad;
				quadSuppliedOnInstantiation = true;
			} else {
				usingQuadForLocation = true;
				realWorldQuad = quad;
				BuildProjectionModel (quad);
			}

		} else {
			Debug.Log ("SetRealWorldSpace called with invalid Vector2 array.");
		}
	}

	
	/* This elegant solution was found at:
	 * http://math.stackexchange.com/questions/13404/mapping-irregular-quadrilateral-to-a-rectangle#answer-104595
	 */
	private void BuildProjectionModel(Vector2[] quad) {
		
		normals[0] = Perp(quad [3] - quad [0]).normalized;
		normals[1] = Perp(quad [0] - quad [1]).normalized;
		normals[2] = Perp(quad [1] - quad [2]).normalized;
		normals[3] = Perp(quad [2] - quad [3]).normalized;
		
	}
	private Vector2 ProjectRealWorldOntoSquare(Vector2 realWorld) {
		
		float u, uA, uB, v, vA, vB;
		
		uA = Vector2.Dot (realWorld - realWorldQuad [0], normals [0]);
		uB = Vector2.Dot (realWorld - realWorldQuad [2], normals [2]);
		u = uA / (uA + uB);
		vA = Vector2.Dot (realWorld - realWorldQuad [1], normals [1]);
		vB = Vector2.Dot (realWorld - realWorldQuad [3], normals [3]);
		v = vA / (vA + vB);
		
		return new Vector2 (u, v);
		
	}


	private Vector2 Perp(Vector2 v) {
		return new Vector2(v.y, -v.x);
	}
}
