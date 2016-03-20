using UnityEngine;
using System.Collections;

public class StageGiftBox : MenusBehavior {
  StageGiftGold gold;
  GameObject bg;

  void Awake() {
    gold = transform.parent.Find("Gold").GetComponent<StageGiftGold>();
    bg = transform.parent.Find("GiftBoxBackground").gameObject;
  }

  override public void activateSelf() {
    gameObject.SetActive(false);
    bg.SetActive(false);
    gold.show();
  }
}
