using UnityEngine;
using System.Collections;

public class PartsCount : MonoBehaviour {

  private int count = 0;

	void Start () {
    Vector3 top_left = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, Camera.main.transform.position.y - transform.position.y));
    transform.position = new Vector3(top_left.x + 10, top_left.y, top_left.z - 1);
	}

	void Update () {
	}

  public void addCount() {
    count++;
    GetComponent<TextMesh>().text = count.ToString();
  }
}
