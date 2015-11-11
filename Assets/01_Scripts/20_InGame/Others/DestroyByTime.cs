using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
  public float lifetime = 1;

  void OnEnable() {
    Invoke("inactivate", lifetime);
  }

  void inactivate() {
    gameObject.SetActive(false);
  }

  void OnDisble() {
    CancelInvoke();
  }
}
