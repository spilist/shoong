using UnityEngine;
using System.Collections;

public class DirArrowController : MonoBehaviour {
	public GameObject directionArrow;
	float rotatePerMinute = 40.0f;
	bool rotating = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (rotating == true) {
			directionArrow.transform.Rotate(new Vector3(0.0f,6.0f*rotatePerMinute*Time.deltaTime,0.0f));
		}
	}

	public void init(Vector3 position, Vector3 rotation) {
		directionArrow.transform.position = position;
		directionArrow.transform.rotation = Quaternion.identity;
		directionArrow.transform.Rotate(rotation);
	}

	public void init(Vector3 position, Quaternion rotation) {
		directionArrow.transform.position = position;
		directionArrow.transform.rotation = rotation;
	}

	public Vector3 getDirection() {
		return directionArrow.transform.forward;
	}

	public void setVisible(bool value) {
		directionArrow.SetActive(value);
	}
	public void setRotate(bool value) {
		rotating = value;
	}
	public Quaternion getRotate() {
		return directionArrow.transform.rotation;
	}
}
