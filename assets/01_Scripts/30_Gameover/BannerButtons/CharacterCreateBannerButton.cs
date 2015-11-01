using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterCreateBannerButton : BannerButton {
  public string toGoString;
  public CharacterCreateMenu createMenu;
  public GameObject menusOverlay;
  public Text text;
  private bool increasing = false;
  private int increasingSpeed;
  private float target;
  private float toGo;

  override public void activateSelf() {
    if (filter.sharedMesh == inactiveMesh) return;

    menusOverlay.SetActive(true);
    back.useBannerMenu(this, gameOverUI, createMenu.gameObject);
  }

  override public void goBack() {
    menusOverlay.SetActive(false);
    checkAffordable();
  }

  public void checkAffordable() {
    toGo = createMenu.createPrice - GoldManager.gm.getCount();
    if (toGo > 0) {
      text.text = toGo + toGoString;
      filter.sharedMesh = inactiveMesh;
      playTouchSound = false;
    } else {
      text.text = description;
      filter.sharedMesh = activeMesh;
      playTouchSound = true;
    }
  }

  public void decreaseToGo(int amount, int speed) {
    if (filter.sharedMesh == inactiveMesh) {
      increasing = true;
      target = (toGo - amount) > 0 ? (toGo - amount) : 0;
      increasingSpeed = speed;
    }
  }

  void Update() {
    if (increasing) {
      toGo = Mathf.MoveTowards(toGo, target, Time.deltaTime * increasingSpeed);
      text.text = toGo.ToString("0") + toGoString;

      if (toGo == target) {
        increasing = false;
        checkAffordable();
      }
    }
  }
}
