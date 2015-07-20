using UnityEngine;
using System.Collections;

public class UpperRightTierController : MonoBehaviour {

	public string tierName;
	public UpperRightMenuController menuController;

	public bool isTopTier = false;
	public UpperRightTierController parentTier;

	public UpperRightItem[] items = new UpperRightItem[0];
	public int defaultItem = 1; // starts at 1 to match naming in Hierarchy
	public bool wrapScrolling;
	public float slideRate = 0.3f;
	
	public RectTransform rectTransform;

	private int currentItem;
	private bool tierActivated = false;
	private bool childActivated = false;
	private bool tierPaused = false;

	public Vector3 startPosition;
	public Vector3 targetOffset;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		startPosition = rectTransform.position;
		targetOffset = items [defaultItem-1].displayObject.GetComponent<RectTransform>().localPosition;
		Reset ();
	}
	void Update () {
		rectTransform.position = Vector3.Lerp (rectTransform.position, startPosition + targetOffset, slideRate);
	}

	public void Enter() {
		TierDisplay ();
		tierActivated = true;
		childActivated = false;
	}

	public void JumpTo(int item) {
		if (item != -1)
			currentItem = item;
		menuController.currentTier = this;
		Enter ();
	}
	public void JumpTo() {
		JumpTo (defaultItem);
	}
	public void JumpToWithLastItem() {
		JumpTo(-1);
	}

	public void Leave() {
		tierActivated = false;
		TierHide ();
		if (isTopTier) {
			menuController.menuOpen = false;
		} else {
			menuController.currentTier = parentTier;
			parentTier.Enter();
		}
		Reset ();
	}

	public void Pause() {
		if (tierActivated) {
			if (!tierPaused) {
				tierPaused = true;
				TierHide ();
			} else {
				tierPaused = false;
				TierDisplay ();
			}
		} else {
			Debug.Log ("Tried to pause tier when not active:" + tierName);
		}
	}

	public void Accept() {
		if (!childActivated) { // That means we have to deal with it, so run the Enter method. This will either enter a new tier or run a script 'Do()'.
			if (items [currentItem].isTier) {
				childActivated = true;
				menuController.currentTier = items[currentItem].tierController;
				TierHide ();
			}
			items [currentItem].Enter ();
		} else {
			items [currentItem].Accept (); // Pass for the item to handle the accept request
		}
	}

	public void Scroll(int scrollBy) {
		if (!childActivated) {
			if (scrollBy % items.Length != 0) {
				items [currentItem].ItemHide ();
				currentItem += scrollBy;
				LimitScrolling ();
				items [currentItem].ItemDisplay ();
				rectTransform.position = startPosition + targetOffset;
				targetOffset = -items[currentItem].displayObject.GetComponent<RectTransform>().localPosition;
			}
		} else {
			items [currentItem].Scroll (scrollBy);
		}
	}

	public void Reset() {
		currentItem = defaultItem - 1;
		rectTransform.position = startPosition;
		targetOffset = -items[currentItem].displayObject.GetComponent<RectTransform>().localPosition;
	}
	
	public bool IsActive() {
		return tierActivated;
	}
	public bool IsPaused() {
		return tierPaused;
	}
	
	private void TierDisplay() {
		items [currentItem].ItemDisplay ();
	}
	
	private void TierHide() {
		items [currentItem].ItemHide ();
	}

	private void TierMoveTo (float x) {

	}

	private void LimitScrolling() {
		if (wrapScrolling) {
			while (currentItem < 0) currentItem += items.Length;
			currentItem = currentItem % items.Length;
		} else {
			currentItem = Mathf.Clamp (currentItem, 0, items.Length - 1);
		}
	}

}
