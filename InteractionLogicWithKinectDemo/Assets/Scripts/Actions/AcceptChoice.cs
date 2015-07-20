using UnityEngine;
using System.Collections;

public class AcceptChoice : UpperRightAction {

	public bool closeMenu = true;

	public override void Do() {
		Debug.Log ("Choice made");
		if (closeMenu) {
			transform.gameObject.GetComponentInParent<UpperRightMenuController>().Exit ();
		}
	}
}
