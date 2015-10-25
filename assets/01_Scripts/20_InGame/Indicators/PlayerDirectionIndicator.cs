using UnityEngine;
using System.Collections;

public class PlayerDirectionIndicator : MonoBehaviour {
	Vector3 dir;

	void Update () {
    dir = Player.pl.getDirection();
    float angle = ContAngle(Vector3.forward, dir);
    Debug.Log(angle);
    transform.localEulerAngles = new Vector3(0, 0, angle);
	}

  float ContAngle(Vector3 fwd, Vector3 targetDir) {
    float angle = Vector3.Angle(fwd, targetDir);

    if (AngleDir(fwd, targetDir, Vector3.up) == -1) {
        angle = 360.0f - angle;
        if( angle > 359.9999f )
            angle -= 360.0f;
        return angle;
    } else return angle;
  }

  int AngleDir( Vector3 fwd, Vector3 targetDir, Vector3 up) {
    Vector3 perp = Vector3.Cross(fwd, targetDir);
    float dir = Vector3.Dot(perp, up);

    if (dir > 0.0) return 1;
    else if (dir < 0.0) return -1;
    else return 0;
  }
}

