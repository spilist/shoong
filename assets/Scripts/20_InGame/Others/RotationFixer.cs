using UnityEngine;
using System.Collections;

public class RotationFixer : MonoBehaviour {
  private Quaternion rotation;

	void Start () {
    rotation = transform.rotation;
	}

	void Update () {
    transform.rotation = rotation;
	}
}
