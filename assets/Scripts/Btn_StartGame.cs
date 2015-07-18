using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Btn_StartGame : MonoBehaviour {
	public GameObject inputModuleObject;
	private TouchInputModule inputMoudle;

	// Use this for initialization
	void Start () {
		inputMoudle = inputModuleObject.GetComponent<TouchInputModule> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
