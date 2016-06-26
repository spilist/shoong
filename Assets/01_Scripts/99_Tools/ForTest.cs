using UnityEngine;
using System.Collections;

public class ForTest : MonoBehaviour {
  public RainbowDonutsManager rdm;

  void OnEnable() {
    rdm.runImmediately();
  }

}
