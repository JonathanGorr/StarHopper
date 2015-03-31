using UnityEngine;
using System.Collections;

public class DotInput : MonoBehaviour {
	private SpriteRenderer sRenderer;
	Color defaultColor;

	void Start(){
		sRenderer = GetComponent<SpriteRenderer>();
		defaultColor = new Color(0f, 1f, 0f, 1f); // GET A COLOR INSTEAD
		sRenderer.material.color = defaultColor;
	}

	void OnMouseDown(){
		sRenderer.material.color = new Color(1f, 1f, 0f, 1f);
	}

	void OnMouseUp(){
		sRenderer.material.color = defaultColor;
	}

	void OnMouseDrag(){
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mousePosition.x, mousePosition.y, 0f);

	}
}
