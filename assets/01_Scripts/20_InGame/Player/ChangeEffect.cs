using UnityEngine;
using System.Collections;

public class ChangeEffect : MonoBehaviour {
  private CharacterChangeManager cm;
  private PlayerMover player;

  public string changeTo;
  public string effectName;

  void OnEnable() {
    if (player == null) {
      player = GameObject.Find("Player").GetComponent<PlayerMover>();
      cm = player.GetComponent<CharacterChangeManager>();
    }

    if (changeTo != "") {
      cm.changeCharacterTo(changeTo);
    }

    if (effectName != "") {
      player.showEffect(effectName);
    }
  }

  void OnDisable() {
    if (changeTo != "") {
      cm.changeCharacterToOriginal();
    }
  }
}
