using UnityEngine;
using System.Collections;

public class RotationFixer : MonoBehaviour {
  private Quaternion rotation;

  void OnEnable() {
    rotation = transform.rotation;
  }

  void Update () {
    transform.rotation = rotation;
	}
}
