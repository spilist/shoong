using UnityEngine;
using System.Collections;

public class AspectRatioEnforcer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Camera>().orthographicSize = (Screen.height / 1920) * (1080 / Screen.width);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
