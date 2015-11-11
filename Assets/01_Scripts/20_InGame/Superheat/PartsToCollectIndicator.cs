using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsToCollectIndicator : MonoBehaviour {
  private Renderer mRenderer;
  private int rotatingSpeed;

  private PartsToBeCollected ptb;
  private Transform followTr;
  private Mesh checkMesh;

  void Awake() {
    mRenderer = GetComponent<Renderer>();
  }

  public void run(Transform tr, Mesh mesh) {
    GameObject ptbObj = GameObject.Find("PartsToBeCollected");
    if (ptbObj == null) return;

    ptb = ptbObj.GetComponent<PartsToBeCollected>();
    rotatingSpeed = ptb.indicatorRotatingSpeed;
    followTr = tr;
    checkMesh = mesh;
  }

  void Update() {
    if (followTr == null || !followTr.gameObject.activeSelf) gameObject.SetActive(false);
    else {
      if (ptb.isMeshCollectable(checkMesh)) {
        showImg(true);

        transform.position = new Vector3(followTr.position.x, transform.position.y, followTr.position.z);
        transform.Rotate(0, 0, Time.deltaTime * rotatingSpeed);
      } else {
        showImg(false);
      }
    }
  }

  void showImg(bool val) {
    if (mRenderer.enabled != val) mRenderer.enabled = val;
  }
}
