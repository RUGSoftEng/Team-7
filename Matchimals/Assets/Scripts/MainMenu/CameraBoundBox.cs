using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Places a 2d physics bounding box around the camera;
[RequireComponent(typeof(Camera))]
public class CameraBoundBox : MonoBehaviour {
    // The Material to apply to the collider.
    public PhysicsMaterial2D material;

    private Camera boxedCamera;
    private EdgeCollider2D edgeCollider;

    // Use this for initialization
    void Start () {
        this.edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        edgeCollider.sharedMaterial = material;
        this.boxedCamera = gameObject.GetComponent<Camera>();
        float height = 2f * boxedCamera.orthographicSize;
        float width = height * boxedCamera.aspect;
        SetBox(0, 0, width, height);
	}
	
	private void SetBox(float x, float y, float width, float height) {
        List<Vector2> newPoints = new List<Vector2>();
        float padX = width / 2; float padY = height / 2;
        newPoints.Add(new Vector2(x - padX, y - padY));
        newPoints.Add(new Vector2(x + padX, y - padY));
        newPoints.Add(new Vector2(x + padX, y + padY));
        newPoints.Add(new Vector2(x - padX, y + padY));
        newPoints.Add(new Vector2(x - padX, y - padY));
        edgeCollider.points = newPoints.ToArray();
    }
}
