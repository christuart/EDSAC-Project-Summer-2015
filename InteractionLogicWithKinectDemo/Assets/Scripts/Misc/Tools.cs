using UnityEngine;
using System.Collections;

public abstract class Tools : MonoBehaviour {

	public static float LerpPlus(float from, float to, float rate, float threshold, float finalRate, ref bool inProgress) {
		if (Mathf.Abs(to - from) > threshold) {
			inProgress = true;
			return Mathf.Lerp (from, to, rate);
		} else {
			if (Mathf.Abs(to - from) > finalRate) {
				inProgress = true;
				return from + Mathf.Sign (to - from) * finalRate;
			} else {
				inProgress = false;
				return to;
			}
		}
	}

}
