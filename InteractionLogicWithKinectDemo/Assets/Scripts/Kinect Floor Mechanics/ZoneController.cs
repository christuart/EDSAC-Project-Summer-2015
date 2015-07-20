using UnityEngine;
using System.Collections;

public class ZoneController : MonoBehaviour {

	public GameObject highlightRegion;
	public GameObject activateRegion;

	public Material activeActivateMaterial;
	public Material inactiveActivateMaterial;
	
	public UpperRightAction entryAction;
	public UpperRightAction exitAction;

	public float initialHeight;

	void Start() {
		initialHeight = transform.position.y;
	}

	public void HideHighlightRegion () {
		highlightRegion.SetActive (false);
	}
	public void ShowHighlightRegion () {
		highlightRegion.SetActive (true);
	}
	
	public void ShowActivateRegionActive() {
		activateRegion.GetComponent<MeshRenderer>().material = activeActivateMaterial;
	}
	public void ShowActivateRegionInactive() {
		activateRegion.GetComponent<MeshRenderer>().material = inactiveActivateMaterial;
	}
	
	public void doEntryAction() {
		if (entryAction != null)
			entryAction.Do ();
	}
	public void doExitAction() {
		if (exitAction != null)
			exitAction.Do ();
	}
}
