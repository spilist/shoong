using UnityEngine;
using System.Collections;

public class BlackholeGravitySphere : MonoBehaviour {
  private BlackholeManager blm;

  void Start() {
    blm = GameObject.Find("Field Objects").GetComponent<BlackholeManager>();
  }

  // void LateUpdate() {
  //   if (startFollow) {
  //     if (blackhole == null) {
  //       Destroy(gameObject);
  //     } else {
  //       transform.position = new Vector3 (blackhole.transform.position.x, transform.position.y, blackhole.transform.position.z);
  //     }
  //   }
  // }

  void OnTriggerEnter(Collider other) {
    if (other.tag != "Player") {
      other.gameObject.GetComponent<ObjectsMover>().insideBlackhole();
    }
  }

  void OnTriggerExit(Collider other) {
    if (other.tag == "Player") {
      blm.instance.GetComponent<BlackholeMover>().encounterPlayer();
    }
  }
}
