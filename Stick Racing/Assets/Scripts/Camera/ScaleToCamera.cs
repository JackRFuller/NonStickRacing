using UnityEngine;
using System.Collections;

public class ScaleToCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {

		float height = Camera.main.orthographicSize * 2;
		float width = height * Screen.width/ Screen.height; // basically height * screen aspect ratio

		gameObject.transform.localScale = Vector3.one * height / 6f;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
