using UnityEngine;
using System.Collections;

public class OrbitUp : UpperRightAction {
	
	public ModelRoomGameController gameController;
	
	public override void Do() {
		gameController.OrbitUp ();
	}
}
