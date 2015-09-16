using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
  public GameObject target;
  public bool copyRotation = false;

  private Transform targetTransform;

  void Start() {
    targetTransform = target.GetComponent<Transform>();
  }

  void LateUpdate() {
    if (target == null) {
      Destroy(gameObject);
    } else {
      transform.position = new Vector3 (targetTransform.position.x, transform.position.y, targetTransform.position.z);
      if (copyRotation) {
        transform.rotation = targetTransform.rotation;
      }
    }
  }
}
