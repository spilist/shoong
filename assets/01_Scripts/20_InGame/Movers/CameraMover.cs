using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
  public Transform player;
  public bool slowly = false;

  private Vector3 velocity = Vector3.zero;
  public float smoothTime = 0.1f;
  public float speed = 20f;

  private bool shaking = false;
  public float shakeDuring = 0.5f;
  private float shakeCount = 0;
  public float shakeAmount = 0.7f;
  private Vector3 originalPos;

  private Vector3 pastTargetPosition, pastFollowerPosition;
  private Vector3 pastPosition;

  void Update() {
    if (shaking) {
      if (shakeCount < shakeDuring) {
        originalPos = Vector3.SmoothDamp(originalPos, new Vector3 (player.position.x, transform.position.y, player.position.z), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
        transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
        shakeCount += Time.deltaTime;
      } else {
        transform.position = originalPos;
        shaking = false;
      }
    } else if (slowly) {
      transform.position = Vector3.SmoothDamp(transform.position, new Vector3 (player.position.x, transform.position.y, player.position.z), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
    }
    else {
      transform.position = new Vector3 (player.position.x, transform.position.y, player.position.z);
    }
  }

  public void setSlowly(bool val) {
    slowly = val;
  }

  public void shake() {
    shaking = true;
    originalPos = transform.position;
    shakeCount = 0;
  }
}

