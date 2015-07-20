using UnityEngine;
using System.Collections;

public class HighlightItem : UpperRightAction {

	public HighlightController target;

	override public void Do() {
		target.Highlight ();
	}
}
