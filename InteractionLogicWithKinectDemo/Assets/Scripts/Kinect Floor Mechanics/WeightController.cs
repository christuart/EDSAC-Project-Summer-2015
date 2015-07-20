using UnityEngine;
using System.Collections;

public class WeightController : MonoBehaviour {

	public GameObject user;
	/*public float thresh;
	public float kP;
	public float kI;
	public float fadeI;*/

	public float force;

	private Rigidbody rb;
	
	private Vector3 error = new Vector3 ();
	private Vector3 integralError = new Vector3 ();
	private bool locked = true;

	private RegionController regionController;

	
	private static int averageOver = 25;
	private Vector3[] userHistory = new Vector3[averageOver];
	private Vector3 average;
	private int last = averageOver - 1;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		GameObject rc = GameObject.FindGameObjectWithTag ("RegionController");
		if (rc != null) {
			regionController = rc.GetComponent<RegionController> ();
		}
		for (int i = 0; i < 5; i++) {
			userHistory [i] = transform.localPosition;
		}
		average = transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/*error = (user.transform.position - transform.position);
		if (locked) {
			integralError *= (1 - fadeI);
			integralError += Mathf.Pow(error.magnitude,.25f) * error.normalized * kI;
			if (integralError.magnitude > 1) {
				locked = false;
				rb.velocity = new Vector3(0, 0, 0);
				integralError = new Vector3();
			}
		}
		if (!locked) {
			if (error.magnitude < thresh) {
				locked = true;
			} else {
				rb.velocity = error * kP;
			}
		}
		transform.position = new Vector3(
			Mathf.Clamp (transform.position.x,-2.5f,2.5f),
			transform.position.y,
			Mathf.Clamp(transform.position.z,-1.0f,2.0f));*/
		last = (last + 1) % averageOver;
		average -= userHistory [last] / averageOver;
		userHistory [last] = user.transform.localPosition;
		average += userHistory [last] / averageOver;
		transform.localPosition = Vector3.Lerp (transform.localPosition, average, 0.15f);
	}
	
	void OnTriggerEnter(Collider other) {
		
		RegionType regionType;
		int zone = Zone.Error;
		
		regionType = RegionController.getRegionTypeFromTag(other.tag);
		zone = RegionController.getZoneFromName (other.transform.parent.gameObject.name);

		if (zone != Zone.Error)
			regionController.RegionEntered (zone, regionType);
		
	}
	void OnTriggerExit(Collider other) {
		
		RegionType regionType;
		int zone = Zone.Error;

		regionType = RegionController.getRegionTypeFromTag(other.tag);
		zone = RegionController.getZoneFromName (other.transform.parent.gameObject.name);
		
		if (zone != Zone.Error)
			regionController.RegionExited (zone, regionType);
		
	}

}
