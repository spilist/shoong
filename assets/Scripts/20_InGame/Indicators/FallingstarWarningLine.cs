using UnityEngine;
using System.Collections;

public class FallingstarWarningLine : MonoBehaviour {
  private float drawingSpeed;
  private bool isDrawing = false;
  private bool isErasing = false;
  private LineRenderer outer;
  private int distanceToDest;
  private float drawingDistance = 0;
  private float erasingDistance = 0;

  private Vector3 origin;
  private Vector3 destination;
  private GameObject target;

  void Update () {
    if (isDrawing) {
      drawingDistance = Mathf.MoveTowards(drawingDistance, distanceToDest, Time.deltaTime * drawingSpeed);
      Vector3 nextPos = drawingDistance * Vector3.Normalize(destination - origin) + origin;
      outer.SetPosition(1, nextPos);

      if (drawingDistance == distanceToDest) {
        isDrawing = false;
      }
    }

    if (isErasing) {
      erasingDistance = Mathf.MoveTowards(erasingDistance, distanceToDest, Time.deltaTime * drawingSpeed);
      Vector3 nextPos = erasingDistance * Vector3.Normalize(destination - origin) + origin;

      outer.SetPosition(0, nextPos);
      if (erasingDistance == distanceToDest) {
        isErasing = false;
        Destroy(gameObject);
      }
    }
  }

  public void run(Vector3 startPos, Vector3 destPos, int distance, float duration) {
    outer = transform.Find("Outer").GetComponent<LineRenderer>();

    origin = startPos;
    destination = destPos;
    outer.SetPosition(0, startPos);
    outer.SetPosition(1, startPos);
    distanceToDest = distance;
    drawingSpeed = distance / duration;
    isDrawing = true;

    outer.enabled = true;
  }

  public void erase() {
    isErasing = true;
  }
}
