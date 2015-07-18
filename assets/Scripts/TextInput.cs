using UnityEngine;
using System.Collections;

public class TextInput : MonoBehaviour {

	public SphereMover sphereMover;
	private int scoredisplay;
	private bool isScoring = false;

	float time=0.0f;

	// Use this for initialization
	void Start () {
		Vector3 top_right = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.y - transform.position.y));
		transform.position = new Vector3(top_right.x - 1, top_right.y, top_right.z - 1);
	}

	// Update is called once per frame time.ToString("0")+"_"+
	void Update () {
		if (isScoring) {
			time += Time.deltaTime;
			GetComponent<TextMesh>().text = time.ToString ("0");
			//scoredisplay = sphereMover.scored;
		}

	}

	public void startScoring() {
		isScoring = true;
	}

	public void stopScoring() {
		isScoring = false;
	}
}
