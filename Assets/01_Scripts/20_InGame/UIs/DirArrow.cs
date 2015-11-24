using UnityEngine;
using System.Collections;

public class DirArrow : MonoBehaviour {

	void OnPointerDown() {
    Vector3 heading = transform.position - transform.parent.position;
    Player.pl.setDirection(heading / heading.magnitude);
    Player.pl.shootBooster();
  }
}
