using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsCount : MonoBehaviour {

  private int count = 0;

	void Start () {
	}

	void Update () {
	}

  public void addCount() {
    count++;
    GetComponent<Text>().text = count.ToString();
  }
}
