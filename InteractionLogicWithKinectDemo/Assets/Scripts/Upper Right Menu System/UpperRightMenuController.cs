using UnityEngine;
using System.Collections;

public class UpperRightMenuController : MonoBehaviour {

	public GameObject displayPanel;
	public UpperRightTierController topLevel;
	public UpperRightTierController currentTier;

	public bool menuOpen = false;

	public float scrollRate = 0.4f;
	private float lastScroll;

	void Start() {
		currentTier = topLevel;
		lastScroll = Time.time;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Tab) || (!menuOpen && Input.GetMouseButtonDown (2))) {
			Enter ();
		}
		else if (Input.GetKeyDown (KeyCode.Return) || Input.GetMouseButtonDown (2)) {
			Accept ();
		} else if (Input.GetKeyDown (KeyCode.Escape)) {
			Leave ();
		} else if (currentTier.IsActive () && !currentTier.IsPaused ()) {
			displayPanel.SetActive (true);
			if (Time.time - lastScroll > scrollRate) {
				if (Input.GetAxis ("Mouse ScrollWheel") != 0.0f) {
					currentTier.Scroll (-Mathf.RoundToInt (Input.GetAxis ("Mouse ScrollWheel")));
					lastScroll = Time.time;
				}
			}
		} else {
			displayPanel.SetActive (false);
		}
	}

	public void Pause() {
		if (currentTier.IsActive ()) {
			if (true) {
			}
		}
	}

	public void Enter() {
		if (currentTier.IsActive ()) {
			currentTier.Pause ();
			menuOpen = !menuOpen;
		} else {
			currentTier.Enter ();
			menuOpen = true;
		}
	}
	
	public void Accept() {
		if (currentTier.IsActive () && !currentTier.IsPaused ()) {
			currentTier.Accept ();
		}
	}
	
	public void Leave() {
		if (currentTier.IsActive () && !currentTier.IsPaused ())
			currentTier.Leave ();
	}

	public void Exit() {
		while (topLevel.IsActive ()) {
			Leave ();
		}
	}
}
