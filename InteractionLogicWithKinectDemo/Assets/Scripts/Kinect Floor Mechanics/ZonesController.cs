using UnityEngine;
using System.Collections;

public struct Zone 
{ 
	public const int Error = 0; 
	public const int Overview = 1; 
	public const int Control = 2; 
	public const int Zoom_In = 3; 
	public const int Zoom_Out = 4; 
	public const int Orbit_Up = 5; 
	public const int Orbit_Down = 6; 
	public const int Orbit_Left = 7; 
	public const int Orbit_Right = 8; 
	public const int size = 9;
}; 

public class ZonesController : MonoBehaviour {
	
	public ZoneController OverviewZone;
	public ZoneController ControlZone;
	public ZoneController ZoomInZone;
	public ZoneController ZoomOutZone;
	public ZoneController OrbitUpZone;
	public ZoneController OrbitDownZone;
	public ZoneController OrbitLeftZone;
	public ZoneController OrbitRightZone;
	public ZoneController[] zoneControllers;

	public int activeZone;

	private bool updateRegions;

	// Use this for initialization
	void Start () {

		zoneControllers = new ZoneController[Zone.size];
		for (int i=0; i < zoneControllers.Length; i++)
			zoneControllers[i] = new ZoneController();
		zoneControllers [Zone.Error] = null;
		zoneControllers [Zone.Overview] = OverviewZone;
		zoneControllers [Zone.Control] = ControlZone;
		zoneControllers [Zone.Zoom_In] = ZoomInZone;
		zoneControllers [Zone.Zoom_Out] = ZoomOutZone;
		zoneControllers [Zone.Orbit_Up] = OrbitUpZone;
		zoneControllers [Zone.Orbit_Down] = OrbitDownZone;
		zoneControllers [Zone.Orbit_Left] = OrbitLeftZone;
		zoneControllers [Zone.Orbit_Right] = OrbitRightZone;

	}

	void Update() {
		if (updateRegions) {
			UpdateRegions(true);
			updateRegions = false;
		}
	}

	private void UpdateRegions(bool initiatedWithinController) {
		for (int i = 0; i < Zone.size; i++) {
			Debug.Log (i);
			if (zoneControllers[i] != null) {
				if (i == activeZone) {
					zoneControllers [i].HideHighlightRegion ();
					zoneControllers [i].ShowActivateRegionActive();
				} else {
					zoneControllers [i].ShowHighlightRegion ();
					zoneControllers [i].ShowActivateRegionInactive();
				}
			}
		}
	}

	public void UpdateRegions() {
		updateRegions = true;
	}

	public void doEntryAction(int zone) {
		if (zoneControllers[zone] != null)
			zoneControllers[zone].doEntryAction();
	}
}
