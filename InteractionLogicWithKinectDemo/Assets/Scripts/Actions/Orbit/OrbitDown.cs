using UnityEngine;
using System.Collections;

public class OrbitDown : UpperRightAction {
	
	public ModelRoomGameController gameController;
	
	public override void Do() {
		gameController.OrbitDown ();
	}
}
