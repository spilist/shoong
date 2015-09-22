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
  private bool shakeContinuously = false;
  private Vector3 originalPos;

  private Vector3 pastTargetPosition, pastFollowerPosition;
  private Vector3 pastPosition;

  void Update() {
    if (shaking) {
      if (slowly) {
        originalPos = Vector3.SmoothDamp(originalPos, new Vector3 (player.position.x, transform.position.y, player.position.z), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
      } else {
        originalPos = new Vector3(player.position.x, transform.position.y, player.position.z);
      }

      transform.position = originalPos + Random.insideUnitSphere * shakeAmount;
      shakeCount -= Time.deltaTime;

      if (!shakeContinuously && shakeCount < 0) stopShake();
    } else if (slowly) {
      transform.position = Vector3.SmoothDamp(transform.position, new Vector3 (player.position.x, transform.position.y, player.position.z), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
    } else {
      transform.position = new Vector3 (player.position.x, transform.position.y, player.position.z);
    }
  }

  public void setSlowly(bool val) {
    slowly = val;
  }

  public void shake(float duration = 0, float amount = 0.7f) {
    if (duration == 0) {
      shakeCount = shakeDuring;
    } else {
      shakeCount = duration;
    }

    shakeAmount = amount;
    shaking = true;
    originalPos = transform.position;
  }

  public void shakeUntilStop(float amount = 0.7f) {
    shaking = true;
    shakeContinuously = true;
    shakeAmount = amount;
    originalPos = transform.position;
  }

  public void stopShake() {
    shaking = false;
    shakeContinuously = false;
    transform.position = originalPos;
  }
}

