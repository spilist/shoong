using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject sphere;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		//offset = transform.position;
		Debug.Log ("Changing resolution to fit current device...");
		if (Screen.width / 10 * 16 > Screen.height) {
			Screen.SetResolution (Screen.height / 16 * 10, Screen.height, true);
		} else {
			Screen.SetResolution (Screen.width, Screen.width / 10 * 16, true);
		}

	}
	
	// Update is called once per frame
	void LateUpdate () {
		//transform.LookAt (Vector3.Scale(sphere.transform.position, new Vector3(0.5f, 0.5f, 0.5f)));
	}
}
