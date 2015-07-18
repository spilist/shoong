using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {
  public GameObject player;

  private Transform playerTransform;

  void Start() {
    playerTransform = player.GetComponent<Transform>();
  }

  void LateUpdate() {
    transform.position = new Vector3 (playerTransform.position.x, transform.position.y, playerTransform.position.z);
  }
}
