using UnityEngine;
using System.Collections;

public class DoNotRotate : MonoBehaviour {
  private Quaternion rotation;

	void Start () {
    rotation = transform.rotation;
	}

	void Update () {
    transform.rotation = rotation;
	}
}
