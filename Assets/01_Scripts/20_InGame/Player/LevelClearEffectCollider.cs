using UnityEngine;
using System.Collections;

public class LevelClearEffectCollider : MonoBehaviour {
  public float levelClearEffectDuration = 1f;
  public float levelClearEffectColliderScaleStart = 45;
  public float levelClearEffectColliderScaleEnd = 450;
  private float levelClearEffectColliderScale;

  void OnEnable() {
    levelClearEffectColliderScale = levelClearEffectColliderScaleStart;
    transform.localScale = Vector3.one * levelClearEffectColliderScaleStart;
  }

  void Update () {
    levelClearEffectColliderScale = Mathf.MoveTowards(levelClearEffectColliderScale, levelClearEffectColliderScaleEnd, Time.deltaTime * (levelClearEffectColliderScaleEnd - levelClearEffectColliderScaleStart) / levelClearEffectDuration);
    transform.localScale = Vector3.one * levelClearEffectColliderScale;

    if (levelClearEffectColliderScale == levelClearEffectColliderScaleEnd) {
      transform.parent.gameObject.SetActive(false);
    }
  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover == null || mover.tag == "Blackhole") return;

    Player.pl.goodPartsEncounter(mover, mover.cubesWhenDestroy(), false);
  }
}
