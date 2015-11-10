using UnityEngine;
using System.Collections;

public class LinkButton : MenusBehavior {
  public string linkTo;

  override public void activateSelf() {
    Application.OpenURL(linkTo);
  }
}
