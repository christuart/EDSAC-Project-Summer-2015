using UnityEngine;
using System.Collections;

public enum RegionType {Highlight, Activate};

public class RegionController : MonoBehaviour {

	public FloorMechanicsController floorMechController;

	public float slowZoneChangeTime = 1.0f;
	
	private float[,] lastEntry;
	private float[,] lastExit;
	private float lastChangeTime;
	private int lastChangeZone;
	private RegionType lastChangeRegion;

	void Start () {

		GameObject gc = GameObject.FindGameObjectWithTag ("FloorMechanicsController");
		if (gc != null) {
			floorMechController = gc.GetComponent<FloorMechanicsController> ();
		}

		lastEntry = new float[Zone.size, sizeof(RegionType)];
		lastExit = new float[Zone.size, sizeof(RegionType)];
		for (int i=0; i < Zone.size; i++)
			for (int j=0; j < sizeof(RegionType); j++) {
				lastEntry [i, j] = 0.0f;
				lastExit [i, j] = 0.0f;
			}
		lastChangeTime = 0.0f;
	}
	
	public static RegionType getRegionTypeFromTag(string regionTag) {
		if (regionTag == "ActivateRegion") {
			return RegionType.Activate;
		} else {
			return RegionType.Highlight;
		}
	}
	public static int getZoneFromName(string name) {
		switch (name) {
		case "Control Zone":
			return Zone.Control;
		case "Overview Zone":
			return Zone.Overview;
		case "Zoom In Zone":
			return Zone.Zoom_In;
		case "Zoom Out Zone":
			return Zone.Zoom_Out;
		case "Orbit Up":
			return Zone.Orbit_Up;
		case "Orbit Down":
			return Zone.Orbit_Down;
		case "Orbit Left":
			return Zone.Orbit_Left;
		case "Orbit Right":
			return Zone.Orbit_Right;
		default:
			return Zone.Error;
		}
	}

	public void RegionEntered(int zone, RegionType regionType) {
		if (regionType == RegionType.Activate) {
			if ((Time.time - lastChangeTime) > slowZoneChangeTime) {
				ConfirmEntry (zone);
			} else {
				StartCoroutine (SlowChangeZone (zone));
			}
		} else if (floorMechController.zonesController.activeZone != zone) {
			floorMechController.HighlightZone (zone);
		}

		lastEntry [zone, (int)regionType] = Time.time;
		lastChangeZone = zone;
		lastChangeRegion = regionType;
	}
	public void RegionExited(int zone, RegionType regionType) {
		if (regionType == RegionType.Highlight)
			floorMechController.StopHighlighting();

		lastExit [zone, (int)regionType] = Time.time;
	}

	IEnumerator SlowChangeZone(int targetZone) {
		Debug.Log ("Entering slowly?");
		yield return new WaitForSeconds (slowZoneChangeTime);
		if (lastChangeZone == targetZone && lastChangeRegion == RegionType.Activate && Time.time - lastChangeTime > 0.9 * slowZoneChangeTime) {
			Debug.Log ("Decided to enter slowly");
			ConfirmEntry(targetZone);
		}
	}

	private void ConfirmEntry(int zone) {
		lastChangeTime = Time.time;
		floorMechController.EnterZone (zone);
	}
}
