using UnityEngine;
using System.Collections;

public class LeaveTier : UpperRightAction {

	public UpperRightTierController tierToLeave;

	public override void Do() {
		tierToLeave.Leave();
	}
}
