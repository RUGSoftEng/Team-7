using UnityEngine;
using System.Collections;

public class IconAnimator : MonoBehaviour {

	// For animating the icon.
	private bool animateIcon = false;
	private float animateSpeed = 0.5f;
	private float angle = 0f;
	private float iconScale=1f;

	public void AnimateIcon() {
		if (animateIcon) {
			angle += animateSpeed;
			iconScale = 1f+ 0.2f*Mathf.Sin(angle);
		}

		if (angle > 2*Mathf.PI) {
			angle = 0f;
			iconScale = 1.0f;
			animateIcon = false;
		}
	}

	public void setAnimation(bool active){
		this.animateIcon = active;
	}

	public float getIconScale(){
		return this.iconScale;
	}

}
