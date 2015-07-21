using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
  public GameObject target;

  private Transform targetTransform;

  void Start() {
    targetTransform = target.GetComponent<Transform>();
  }

  void LateUpdate() {
    transform.position = new Vector3 (targetTransform.position.x, transform.position.y, targetTransform.position.z);
  }
}
