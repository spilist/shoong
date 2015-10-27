using UnityEngine;
using System.Collections;

public class PlayerDirectionIndicator : MonoBehaviour {
  public int followingSpeed = 150;
  float currentAngle = 0;
  bool dirChanging = false;
  float targetAngle;

  void Start() {
    currentAngle = ContAngle(Vector3.forward, Player.pl.getDirection());
    transform.localEulerAngles = new Vector3(0, 0, currentAngle);
  }

  void Update () {
    if (dirChanging) {
      currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, Time.deltaTime * followingSpeed);
      transform.localEulerAngles = new Vector3(0, 0, currentAngle);
      if (Mathf.Abs(currentAngle - targetAngle) < 0.1f) dirChanging = false;
    }
  }

  float ContAngle(Vector3 fwd, Vector3 targetDir) {
    float angle = Vector3.Angle(fwd, targetDir);

    if (AngleDir(fwd, targetDir, Vector3.up) == -1) {
        angle = 360.0f - angle;
        if( angle > 359.9999f ) angle -= 360.0f;
    }

    if (angle > 180) angle -= 360.0f;
    else if (angle < -180) angle += 360.0f;

    angle = -angle;

    return angle;
  }

  int AngleDir( Vector3 fwd, Vector3 targetDir, Vector3 up) {
    Vector3 perp = Vector3.Cross(fwd, targetDir);
    float dir = Vector3.Dot(perp, up);

    if (dir > 0.0) return 1;
    else if (dir < 0.0) return -1;
    else return 0;
  }

  public void setDirection(Vector3 dir) {
    dirChanging = true;
    targetAngle = ContAngle(Vector3.forward, dir);
    currentAngle = transform.localEulerAngles.z;
    if (currentAngle > 180) currentAngle -= 360.0f;
    else if (currentAngle < -180) currentAngle += 360.0f;

    if ((targetAngle - currentAngle) > 180) targetAngle -= 360;
    else if ((currentAngle - targetAngle) > 180) currentAngle -= 360;
  }
}
