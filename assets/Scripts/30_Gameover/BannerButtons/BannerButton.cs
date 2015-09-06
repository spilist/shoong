using UnityEngine;
using System.Collections;

public class BannerButton : MenusBehavior {
  public string description;

  virtual public bool available() {
    return false;
  }
}
