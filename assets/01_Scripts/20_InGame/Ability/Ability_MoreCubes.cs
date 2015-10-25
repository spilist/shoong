using UnityEngine;
using System.Collections;

public class Ability_MoreCubes : CharacterAbility {
  public int moreCubes;

  override public void apply() {
    CubeManager.cm.moreCubes(moreCubes);
  }
}
