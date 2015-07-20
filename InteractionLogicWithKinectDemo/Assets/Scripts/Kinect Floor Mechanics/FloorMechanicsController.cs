using UnityEngine;
using System.Collections;

public class FloorMechanicsController : MonoBehaviour {
	
	public ZonesController zonesController;

	public GUIText activeRegionText;
	public GUIText highlightRegionText;


	void Start() {
		activeRegionText.text = "Active Region: none";
		highlightRegionText.text = "Highlight Region: none";
	}

	public void EnterZone(int zone) {
		Debug.Log("Entered zone " + zone);
		StopHighlighting ();
		zonesController.doEntryAction(zone);
		switch (zone) {
		case Zone.Control:
			activeRegionText.text = "Active Region: Fine Control";
			break;
		case Zone.Overview:
			activeRegionText.text = "Active Region: Overview";
			break;
		case Zone.Error:
			activeRegionText.text = "Active Region: error";
			break;
		}
		zonesController.activeZone = zone;
		zonesController.UpdateRegions ();
	}

	public void HighlightZone(int zone) {
		StopHighlighting ();
		switch (zone) {
		case Zone.Control:
			highlightRegionText.text = "Highlight Region: Fine Control";
			SetZoneActivateRegionHeight(zone,0.15f);
			break;
		case Zone.Overview:
			highlightRegionText.text = "Highlight Region: Overview";
			SetZoneActivateRegionHeight(zone,0.15f);
			break;
		case Zone.Error:
			highlightRegionText.text = "Highlight Region: error";
			break;
		}
	}
	
	public void StopHighlighting() {
		highlightRegionText.text = "Highlight Region: none";
		for(int i = 0; i < Zone.size; i++)
			SetZoneActivateRegionHeight(i,0.0f);
	}
	
	private void SetZoneActivateRegionHeight(int zone, float height) {
		if (zonesController.zoneControllers[zone] != null)
			zonesController.zoneControllers[zone].activateRegion.transform.position = new Vector3(
				zonesController.zoneControllers[zone].activateRegion.transform.position.x,
				zonesController.zoneControllers[zone].initialHeight + height,
				zonesController.zoneControllers[zone].activateRegion.transform.position.z);
	}
	
}
