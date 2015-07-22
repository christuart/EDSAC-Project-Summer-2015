using UnityEngine;
using System.Collections;

public class ViewPointMeshBuilder : MonoBehaviour {

	public ModelRoomGameController gameController;

	public GameObject viewPointMeshVertex;
	public int levels = 3;
	public int columns = 5;
	public float angleTheta = 360f; // angle in floor plane
	public float anglePhiMax = 37f; // angle above horizontal at top level
	public float anglePhiMin = 10f; // angle above horizontal at bottom level
	public bool placeColumnAtBothEnds = false; // i.e. |...|...|...| or |..|..|..|...
	public bool loopAround = true;
	public bool thetaCentredAtOrigin = false;

	public bool enterDefaultVertexOnStart = true;
	public int[] defaultVertex = new int[] {2,3}; // starting at 1,1 and finishing at levels,columns

	public ViewPointMeshCameraController[] targets;
	
	private Vector3[,] camPositions;
	private Quaternion[,] camRotations;
	private GameObject[,] builtMesh;
	private ViewPointMeshVertex[,] builtMeshVertices;

	// Use this for initialization
	void Start () {
		
		camPositions = new Vector3[levels,columns];
		camRotations = new Quaternion[levels,columns];
		builtMesh = new GameObject[levels,columns];
		builtMeshVertices = new ViewPointMeshVertex[levels,columns];
		
		float phi0 = anglePhiMin;
		float dPhi = (anglePhiMax-anglePhiMin)/(levels-1);
		float theta0 = transform.localRotation.eulerAngles.y + (thetaCentredAtOrigin ? -angleTheta/2f : 0f);
		float dTheta = angleTheta / (placeColumnAtBothEnds ? columns-1 : columns);
		
		for (int i = 0; i < levels; i++) {
			for (int j = 0; j < columns; j ++) {
				camPositions[i,j] = new Vector3(
					Mathf.Cos (Mathf.Deg2Rad * (theta0 + j*dTheta)) * Mathf.Cos (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.x,
					Mathf.Sin (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.y,
					Mathf.Sin (Mathf.Deg2Rad * (theta0 + j*dTheta)) * Mathf.Cos (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.z);
				camRotations[i,j].eulerAngles = transform.localRotation.eulerAngles + (new Vector3(phi0 + i*dPhi,90f - j*dTheta,0));
				builtMesh[i,j] = GameObject.Instantiate(viewPointMeshVertex,transform.position+camPositions[i,j],camRotations[i,j]) as GameObject;
			}
		}
		for (int i = 0; i < levels; i++) {
			for (int j = 0; j < columns; j ++) {
				builtMeshVertices[i,j] = builtMesh[i,j].GetComponent<ViewPointMeshVertex>();
				if (i > 0) {
					builtMeshVertices[i,j].down = builtMeshVertices[i-1,j];
					builtMeshVertices[i-1,j].up = builtMeshVertices[i,j];
				}
				if (j > 0) {
					builtMeshVertices[i,j].left = builtMeshVertices[i,j-1];
					builtMeshVertices[i,j-1].right = builtMeshVertices[i,j];
				}
			}
			if (loopAround) {
				builtMeshVertices[i,0].left = builtMeshVertices[i,columns-1];
				builtMeshVertices[i,columns-1].right = builtMeshVertices[i,0];
			}
		}

		gameController.OnBuildMeshToEnter(this);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnDrawGizmosSelected() {
		
		camPositions = new Vector3[levels,columns];
		
		float phi0 = anglePhiMin;
		float dPhi = (anglePhiMax-anglePhiMin)/(levels-1);
		float theta0 = transform.localRotation.eulerAngles.y + (thetaCentredAtOrigin ? -angleTheta/2f : 0f);
		float dTheta = angleTheta / (placeColumnAtBothEnds ? columns-1 : columns);
		
		for (int i = 0; i < levels; i++) {
			for (int j = 0; j < columns; j ++) {
				camPositions[i,j] = new Vector3(
					Mathf.Cos (Mathf.Deg2Rad * (theta0 + j*dTheta)) * Mathf.Cos (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.x,
					Mathf.Sin (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.y,
					Mathf.Sin (Mathf.Deg2Rad * (theta0 + j*dTheta)) * Mathf.Cos (Mathf.Deg2Rad * (phi0 + i*dPhi)) * transform.localScale.z);
				//Gizmos.DrawWireSphere(transform.position+camPositions[i,j],.01f);
				DrawCameraGizmo(camPositions[i,j]);
			}
		}

		Gizmos.color = Color.yellow;

		for (int i = 0; i < levels; i++) {
			for (int j = 0; j < columns; j ++) {
				if (i < levels-1)
					Gizmos.DrawLine(transform.position+camPositions[i,j],transform.position+camPositions[i+1,j]);
				if ((j < columns-1) || (loopAround))
					Gizmos.DrawLine(transform.position+camPositions[i,j],transform.position+camPositions[i,(j+1) % columns]);
			}
		}
		for (int i = 0; i < levels; i++) {
			for (int j = 0; j < columns; j ++) {
				DrawCameraGizmo(camPositions[i,j]);
			}
		}

		Gizmos.color = new Color(1f,1f,1f,0.3f);

		int x = levels/2;
		for (int j = 0; j < columns; j ++) {
			Gizmos.DrawLine(transform.position,transform.position+camPositions[x,j]);
		}
		
	}
	void DrawCameraGizmo(Vector3 pos) {

		Vector3 forward = (-pos).normalized;
		Vector3 right = Vector3.Cross(forward,new Vector3(0,1,0)).normalized;
		Vector3 up = Vector3.Cross(right,forward).normalized;
		
		Gizmos.color = Color.red;
		float scale = 0.02f;
		float arg1, arg2;

		for (int i = 0; i < 4; i++) {
			arg1 = Mathf.PI*(0.5f+i)/2f;
			arg2 = Mathf.PI*(1.5f+i)/2f;
			Gizmos.DrawLine (transform.position + pos + right * 2 * scale * Mathf.Sin (arg1) + up * scale * Mathf.Cos (arg1),
			                 transform.position + pos + right * 2 * scale * Mathf.Sin (arg2) + up * scale * Mathf.Cos (arg2));
			Gizmos.DrawLine (transform.position + pos + right * 4 * scale * Mathf.Sin (arg1) + up * 2 * scale * Mathf.Cos (arg1) + forward * 5 * scale,
			                 transform.position + pos + right * 4 * scale * Mathf.Sin (arg2) + up * 2 * scale * Mathf.Cos (arg2) + forward * 5 * scale);
			Gizmos.DrawLine (transform.position + pos + right * 2 * scale * Mathf.Sin (arg1) + up * scale * Mathf.Cos (arg1),
			                 transform.position + pos + right * 4 * scale * Mathf.Sin (arg1) + up * 2 * scale * Mathf.Cos (arg1) + forward * 5 * scale);
			
		}

	}

	public ViewPointMeshVertex GetDefaultVertex() {
		if (enterDefaultVertexOnStart) {
			return builtMesh[defaultVertex[0]-1,defaultVertex[1]-1].GetComponent<ViewPointMeshVertex>();
		} else {
			return transform.parent.GetComponent<ViewPointMesh>().defaultVertex;
		}
	}
}
