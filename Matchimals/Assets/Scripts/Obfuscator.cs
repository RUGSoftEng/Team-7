using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Renderer))]
public class Obfuscator : MonoBehaviour {
    public float speed = 0.6f;
    private float opacity;
    private Material material;

	// Get the color of the material.
	public void OnEnable () {
        this.material = gameObject.GetComponent<Renderer>().material;
        this.opacity = material.color.a;
        Debug.Assert(material != null);
	}
	
	// Change the opacity based on obfuscation goal.
	public void Update () {
        float timedSpeed = speed * Time.deltaTime;
        Color c = material.color;
        if (c.a != opacity) {
            float diff = c.a - opacity;
            if (Mathf.Abs(diff) > timedSpeed) {
                if (diff < 0)
                    c.a += timedSpeed;
                else
                    c.a -= timedSpeed;
            } else {
                c.a = this.opacity;
            }
            material.color = c;
        }
    }

    public void Obfuscate(float opacity){
        this.opacity = opacity;
    }
}
