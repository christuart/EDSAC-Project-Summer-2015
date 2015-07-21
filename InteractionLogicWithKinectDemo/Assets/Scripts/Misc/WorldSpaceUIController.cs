using UnityEngine;
using System.Collections;

public class WorldSpaceUIController : MonoBehaviour {

	public GameObject camera;

	public float initialAngle;
	public float initialCameraAngle;
	public float angleOffset;
	public Vector3 positionOffset;
	public float angle;

	// Use this for initialization
	void Start () {
		initialAngle = transform.eulerAngles.y;
		initialCameraAngle = camera.transform.eulerAngles.y;
		positionOffset = camera.transform.position - transform.position;
		initialCameraAngle = Mathf.Atan2(positionOffset.x,positionOffset.z) * Mathf.Rad2Deg;
		angleOffset = initialAngle - initialCameraAngle;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newEulers = transform.eulerAngles;
		newEulers.y = camera.transform.eulerAngles.y + angleOffset;
		positionOffset = camera.transform.position - transform.position;
		angle = Mathf.Atan2(positionOffset.x,positionOffset.z) * Mathf.Rad2Deg;
		newEulers.y = angleOffset + angle;
		transform.eulerAngles = newEulers;
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position,0.02f);
	}
}
