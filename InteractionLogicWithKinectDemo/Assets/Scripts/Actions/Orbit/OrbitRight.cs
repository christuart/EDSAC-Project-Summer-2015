using UnityEngine;
using System.Collections;

public class OrbitRight : UpperRightAction {
	
	public ModelRoomGameController gameController;
	
	public override void Do() {
		gameController.OrbitRight ();
	}
}
