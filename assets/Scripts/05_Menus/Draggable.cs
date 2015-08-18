using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
  public GameObject whatToDrag;
  protected bool returningToLeft = false;
  protected bool returningToRight = false;
  public int returningSpeed = 500;
  public int moveLimit = 300;
  float positionX = 0;

  public GameObject draggable() {
    return whatToDrag;
  }

  virtual public float leftDragEnd() {
    return 0;
  }

  virtual public float rightDragEnd() {
    return 0;
  }

  virtual public void returnToLeftEnd() {
    positionX = whatToDrag.transform.localPosition.x;
    returningToLeft = true;
  }

  virtual public void returnToRightEnd() {
    positionX = whatToDrag.transform.localPosition.x;
    returningToRight = true;
  }

  void Update() {
    if (returningToLeft) {
      positionX = Mathf.MoveTowards(positionX, leftDragEnd(), Time.deltaTime * returningSpeed);
      whatToDrag.transform.localPosition = new Vector3(positionX, whatToDrag.transform.localPosition.y, whatToDrag.transform.localPosition.z);
      if (positionX == leftDragEnd()) returningToLeft = false;
    } else if (returningToRight) {
      positionX = Mathf.MoveTowards(positionX, rightDragEnd(), Time.deltaTime * returningSpeed);
      whatToDrag.transform.localPosition = new Vector3(positionX, whatToDrag.transform.localPosition.y, whatToDrag.transform.localPosition.z);
      if (positionX == rightDragEnd()) returningToRight = false;
    }
  }
}
