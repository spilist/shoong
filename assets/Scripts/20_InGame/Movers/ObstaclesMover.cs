using UnityEngine;
using System.Collections;

public class ObstaclesMover : ObjectsMover {
  ObstaclesManager obm;
  private bool avoiding = false;
  private bool questDone = false;

  protected override void initializeRest() {
    obm = GameObject.Find("Field Objects").GetComponent<ObstaclesManager>();
    canBeMagnetized = false;
  }

  protected override float getSpeed() {
    return obm.speed;
  }

  protected override float getTumble() {
    return obm.tumble;
  }

  protected override Vector3 getDirection() {
    return obm.getDirection();
  }

  protected override float strength() {
    return obm.strength;
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (destroyEffect) {
      Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    }
    Destroy(gameObject);

    if (QuestManager.qm.doingQuest("AvoidFallingStar") && !questDone && avoiding) {
      questDone = true;
      QuestManager.qm.addCountToQuest("AvoidFallingStar");
    }
  }

  public override void encounterPlayer() {
    Instantiate(player.GetComponent<PlayerMover>().obstacleDestroy, transform.position, transform.rotation);
    QuestManager.qm.addCountToQuest("DestroyFallingStar");

    Destroy(gameObject);
  }

  override protected void normalMovement() {
    GetComponent<Rigidbody> ().velocity = direction * speed;

    if (QuestManager.qm.doingQuest("AvoidFallingStar") && !questDone) {
      if (Vector3.Distance(player.transform.position, transform.position) < player.GetComponent<PlayerMover>().avoidDistance) {
        avoiding = true;
      }

      if (avoiding && (Vector3.Distance(player.transform.position, transform.position) >= player.GetComponent<PlayerMover>().avoidDistance)) {
        QuestManager.qm.addCountToQuest("AvoidFallingStar");
        questDone = true;
      }
    }
  }

  override protected void doSomethingSpecial(Collision collision) {
    if (QuestManager.qm.doingQuest("FallingStarReboundByCubeDispenser") && !questDone && collision.collider.gameObject.tag == "CubeDispenser") {
      Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
      if (pos.x >= 0.0f && pos.x <= 1.0f && pos.y >= 0.0f && pos.y <= 1.0f) {
        QuestManager.qm.addCountToQuest("FallingStarReboundByCubeDispenser");
      }
    }
  }
}
