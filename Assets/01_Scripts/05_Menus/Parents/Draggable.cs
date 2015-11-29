using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {
  public GameObject whatToDrag;
  public string direction;

  protected bool returningToTop = false;
  protected bool returningToBottom = false;
  protected bool returningToLeft = false;
  protected bool returningToRight = false;
  public int returningSpeed = 500;
  float positionX = 0;
  float positionY = 0;

  public GameObject draggable() {
    return whatToDrag;
  }

  virtual public float topDragEnd() {
    return 0;
  }

  virtual public float bottomDragEnd() {
    return 0;
  }

  virtual public float leftDragEnd() {
    return 0;
  }

  virtual public float rightDragEnd() {
    return 0;
  }

  public void returnToTopEnd() {
    positionY = whatToDrag.transform.localPosition.y;
    returningToTop = true;
  }

  public void returnToBottomEnd() {
    positionY = whatToDrag.transform.localPosition.y;
    returningToBottom = true;
  }

  public void returnToLeftEnd() {
    positionX = whatToDrag.transform.localPosition.x;
    returningToLeft = true;
  }

  public void returnToRightEnd() {
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
    } else if (returningToTop) {
      positionY = Mathf.MoveTowards(positionY, topDragEnd(), Time.deltaTime * returningSpeed);
      whatToDrag.transform.localPosition = new Vector3(whatToDrag.transform.localPosition.x, positionY, whatToDrag.transform.localPosition.z);
      if (positionY == topDragEnd()) returningToTop = false;
    } else if (returningToBottom) {
      positionY = Mathf.MoveTowards(positionY, bottomDragEnd(), Time.deltaTime * returningSpeed);
      whatToDrag.transform.localPosition = new Vector3(whatToDrag.transform.localPosition.x, positionY, whatToDrag.transform.localPosition.z);
      if (positionY == bottomDragEnd()) returningToBottom = false;
    }
  }
}
