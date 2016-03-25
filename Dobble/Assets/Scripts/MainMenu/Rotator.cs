using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public float speed   = 1f;
	public float rotationSpeed = 1f;
	public float radiusX = 1f;
	public float radiusY = 1f;
	public float offset  = 0f;
	public float rotationOffset=0f;
	private float a, startY, startX;

	// Use this for initialization
	public void Start () {
		this.a = offset;
		transform.Rotate(Vector3.one*rotationOffset);
		this.startY = transform.localPosition.y;
		this.startX = transform.localPosition.x;
	}
	
	// Update is called once per frame
	public void Update () {
		this.a += Time.deltaTime*speed;
		this.a %= 2*Mathf.PI;
		Vector3 pos = transform.localPosition;
		pos.y = startY+Mathf.Cos(a)*radiusY;
		pos.x = startX+Mathf.Sin(a)*radiusX;
		transform.localPosition = pos;
		
		transform.Rotate(Vector3.one*rotationSpeed*Time.deltaTime);
	}
}
