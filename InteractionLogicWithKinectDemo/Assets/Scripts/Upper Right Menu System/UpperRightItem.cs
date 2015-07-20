using UnityEngine;
using System.Collections;

public class UpperRightItem : MonoBehaviour {

	public bool isTier;
	public UpperRightTierController tierController;
	public UpperRightAction acceptAction;

	public GameObject displayObject;
	
	public RectTransform rectTransform;

	void Start() {
		displayObject.SetActive (false);
		rectTransform = displayObject.GetComponent<RectTransform> ();
	}

	public void Enter() {
		if (isTier)
			tierController.Enter ();
		if (acceptAction != null)
			acceptAction.Do ();
	}

	public void Accept() {
		if (isTier) {
			tierController.Accept ();
		}
	}

	public void Scroll(int scrollBy) {
		if (isTier) {
			tierController.Scroll (scrollBy);
		}
	}
	
	public void ItemDisplay() {
		displayObject.SetActive (true);
	}
	
	public void ItemHide() {
		displayObject.SetActive (false);
	}
}
