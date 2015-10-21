using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsToCollectIndicator : MonoBehaviour {
  private Image img;
  // private RectTransform rectTr;
  // private float originalWidth;
  // private float shrinkedWidth;
  // private float width;
  // private float shrinkDuration;
  private int rotatingSpeed;

  private PartsToBeCollected ptb;
  private Transform followTr;
  private Mesh checkMesh;

  void Start() {
    // rectTr = GetComponent<RectTransform>();
    img = GetComponent<Image>();
    // originalWidth = rectTr.sizeDelta.x;
    // width = originalWidth;
  }

  public void run(Transform tr, Mesh mesh) {
    ptb = GameObject.Find("PartsToBeCollected").GetComponent<PartsToBeCollected>();
    // shrinkedWidth = ptb.indicatorShrinkedWidth;
    // shrinkDuration = ptb.indicatorShrinkDuration;
    rotatingSpeed = ptb.indicatorRotatingSpeed;
    followTr = tr;
    checkMesh = mesh;
  }

  void Update() {
    if (followTr == null) Destroy(gameObject);
    else {
      if (ptb.isMeshCollectable(checkMesh)) {
        showImg(true);

        transform.position = new Vector3(followTr.position.x, transform.position.y, followTr.position.z);
        transform.Rotate(0, 0, Time.deltaTime * rotatingSpeed);

        // width = Mathf.MoveTowards(width, shrinkedWidth, Time.deltaTime * (originalWidth - shrinkedWidth) / shrinkDuration);
        // rectTr.sizeDelta = width * Vector2.one;
        // if (width == shrinkedWidth) width = originalWidth;
      } else {
        showImg(false);
      }
    }
  }

  void showImg(bool val) {
    if (img.enabled != val) img.enabled = val;
  }
}
