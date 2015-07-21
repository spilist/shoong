using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {
  public float lifetime = 1;

  void Awake() {
    Destroy(gameObject, lifetime);
  }
}
