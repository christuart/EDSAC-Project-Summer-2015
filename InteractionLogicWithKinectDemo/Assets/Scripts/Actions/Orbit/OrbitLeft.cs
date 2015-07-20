using UnityEngine;
using System.Collections;

public class OrbitLeft : UpperRightAction {

	public ModelRoomGameController gameController;

	public override void Do() {
		gameController.OrbitLeft ();
	}
}
