using UnityEngine;
using System.Collections;

public class MenusBehavior : MonoBehaviour {
  public Mesh activeMesh;
  public Mesh inactiveMesh;
  public Mesh blinkingMesh;
  public float blinkingSeconds = 0.4f;

  public bool playTouchSound = true;
  public bool blinkOnStart = false;
  private bool blinking = false;

  protected MeshFilter filter;

  void Awake() {
    filter = GetComponent<MeshFilter>();
    initializeRest();
  }

  void OnEnable() {
    if (blinkOnStart) startBlink();
  }

  virtual public void initializeRest() {}

  virtual public void activateSelf() {}


  protected IEnumerator blinkButton() {
    while(true) {
      filter.sharedMesh = activeMesh;

      yield return new WaitForSeconds(blinkingSeconds);

      filter.sharedMesh = blinkingMesh;

      yield return new WaitForSeconds(blinkingSeconds);
    }
  }

  void OnDisable() {
    stopBlink();
  }

  protected void startBlink() {
    blinking = true;
    StopCoroutine("blinkButton");
    filter.sharedMesh = activeMesh;
    StartCoroutine("blinkButton");
  }

  protected void stopBlink() {
    if (!blinking) return;
    blinking = false;
    blinkOnStart = false;
    StopCoroutine("blinkButton");
    filter.sharedMesh = activeMesh;
  }
}
