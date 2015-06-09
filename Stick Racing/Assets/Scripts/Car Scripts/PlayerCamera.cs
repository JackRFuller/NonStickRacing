using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	[SerializeField] Transform CameraTarget;
	[SerializeField] int[] Offset = new int[3];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (CameraTarget.position.x + Offset [0], CameraTarget.position.y + Offset [1], CameraTarget.position.z + Offset [2]);

	}
}
