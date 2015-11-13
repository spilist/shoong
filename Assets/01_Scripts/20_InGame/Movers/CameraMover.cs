using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {
  public bool fixAspect = false;
  public float aspectWidth = 1920;
  public float aspectHeight = 1080;

  public int playerAheadSpeedBase = 100;
  public float playerAheadScale = 0.5f;
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
  private bool paused = false;

  void Start () {
    if (!fixAspect) return;

    // set the desired aspect ratio (the values in this example are
    // hard-coded for 16:9, but you could make them into public
    // variables instead so you can set them at design time)
    float targetaspect = aspectWidth / aspectHeight;

    // determine the game window's current aspect ratio
    float windowaspect = (float)Screen.width / (float)Screen.height;

    // current viewport height should be scaled by this amount
    float scaleheight = windowaspect / targetaspect;

    // obtain camera component so we can modify its viewport
    Camera camera = GetComponent<Camera>();

    // if scaled height is less than current height, add letterbox
    if (scaleheight < 1.0f) {
      Rect rect = camera.rect;

      rect.width = 1.0f;
      rect.height = scaleheight;
      rect.x = 0;
      rect.y = (1.0f - scaleheight) / 2.0f;

      camera.rect = rect;
    } else { // add pillarbox
      float scalewidth = 1.0f / scaleheight;

      Rect rect = camera.rect;

      rect.width = scalewidth;
      rect.height = 1.0f;
      rect.x = (1.0f - scalewidth) / 2.0f;
      rect.y = 0;

      camera.rect = rect;
    }
  }

  void Update() {
    if (shaking && !paused) {
      if (slowly) {
        originalPos = Vector3.SmoothDamp(originalPos, playerPos(), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
      } else {
        originalPos = playerPos();
      }

      transform.position = new Vector3(originalPos.x + Random.insideUnitSphere.x * shakeAmount, originalPos.y, originalPos.z + Random.insideUnitSphere.z * shakeAmount);
      shakeCount -= Time.deltaTime;

      if (!shakeContinuously && shakeCount < 0) stopShake();
    } else if (slowly) {
      transform.position = Vector3.SmoothDamp(transform.position, playerPos(), ref velocity, smoothTime, Mathf.Infinity, Time.smoothDeltaTime);
    } else {
      transform.position = playerPos();
    }
  }

  Vector3 playerPos() {
    Vector3 pos;
    // if (Player.pl.getSpeed() > playerAheadSpeedBase) {
      // pos = Player.pl.transform.position + Player.pl.getDirection() * (Player.pl.getSpeed() - playerAheadSpeedBase) * playerAheadScale;
    // } else {
      pos = Player.pl.transform.position;
    // }

    return new Vector3(pos.x, transform.position.y, pos.z);
  }

  public void setPaused(bool val) {
    paused = val;
  }

  public void setSlowly(bool val) {
    slowly = val;
  }

  public void shake(float duration = 0, float amount = 4) {
    if (duration == 0) {
      shakeCount = shakeDuring;
    } else {
      shakeCount = duration;
    }

    shakeAmount = amount;
    shaking = true;
    originalPos = transform.position;
  }

  public void shakeUntilStop(float amount = 4) {
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

