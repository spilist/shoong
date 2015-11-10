using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {
  public int speed = 30;
  Vector3 randomVector;

	void OnEnable() {
    randomVector = Random.onUnitSphere;
  }

  void Update() {
    transform.Rotate(randomVector * Time.deltaTime * speed);
  }
}
