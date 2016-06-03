using UnityEngine;
using System.Collections;

public class DashManager : MonoBehaviour {
  public static DashManager dm;

  public DashButton dash;

  public float duration = 0.3f;
  public float speedup = 300;

  public float maxEnlargeSize = 1.4f;
  public int maxStep = 4;
  private int currentStep = 0;

  void Awake() {
    dm = this;
  }

  public void getLarger() {
    if (currentStep >= maxStep) return;

    currentStep++;
    Player.pl.scaleChange(currentStep * (maxEnlargeSize - 1) / maxStep);

    if (currentStep == maxStep) {
      dash.enableAbility();
    }
  }

  public void resetStep() {
    currentStep = 0;
  }
}
