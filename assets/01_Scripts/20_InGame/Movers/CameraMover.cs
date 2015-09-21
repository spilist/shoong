using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
  public Transform player;
  public bool slowly = false;

  private Vector3 velocity = Vector3.zero;
  public float smoothTime = 0.1f;
  public float speed = 20f;

  private Vector3 pastTargetPosition, pastFollowerPosition;

  void Update() {

    // transform.position = SmoothApproach(pastFollowerPosition, pastTargetPosition, player.position);
    // pastFollowerPosition = transform.position;
    // pastTargetPosition = player.position;

    if (slowly) {
      transform.position = Vector3.SmoothDamp(transform.position, new Vector3 (player.position.x, transform.position.y, player.position.z), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
    }
    else {
      transform.position = new Vector3 (player.position.x, transform.position.y, player.position.z);
    }
  }

  // Vector3 SmoothApproach( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition) {
  //   float t = Time.deltaTime * speed;
  //   Vector3 v = ( targetPosition - pastTargetPosition ) / t;
  //   Vector3 f = pastPosition - pastTargetPosition + v;
  //   Vector3 result = targetPosition - v + f * Mathf.Exp( -t );
  //   result.y = transform.position.y;
  //   return result;
  //  }

  public void setSlowly(bool val) {
    slowly = val;
  }
}

