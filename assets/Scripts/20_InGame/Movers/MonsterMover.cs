using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterMover : ObjectsMover {
  private float speed_chase;
  private float speed_runaway;
  private float speed_weaken;

  private MonsterManager monm;

  private float originalScale;
  private bool weak = false;
  private bool shrinking = true;

  public ParticleSystem aura;
  public ParticleSystem weakenAura;
  private Renderer m_renderer;

  protected override void initializeRest() {
    monm = GameObject.Find("Field Objects").GetComponent<MonsterManager>();
    originalScale = shrinkedScale;
    speed_chase = monm.speed_chase;
    speed_runaway = monm.speed_runaway;
    speed_weaken = monm.speed_weaken;
    monm.indicator.startIndicate(gameObject);
    monm.indicator.GetComponent<Image>().color = Color.red;
    m_renderer = GetComponent<Renderer>();
    StartCoroutine("weakened");
  }

  protected override float getSpeed() {
    return speed_chase;
  }

  protected override float getTumble() {
    return monm.tumble;
  }

  protected override Vector3 getDirection() {
    Vector3 dir = player.transform.position - transform.position;
    return dir / dir.magnitude;
  }

  IEnumerator weakened() {
    yield return new WaitForSeconds(Random.Range(monm.minLifeTime, monm.maxLifeTime));
    weak = true;
    monm.stopWarning();
    weakenAura.Play();
    weakenAura.GetComponent<AudioSource> ().Play ();
    aura.Stop();
    m_renderer.material.SetColor("_OutlineColor", monm.weakenedOutlineColor);
    m_renderer.material.SetFloat("_Outline", 0.75f);
    monm.indicator.GetComponent<Image>().color = monm.weakenedOutlineColor;

    yield return new WaitForSeconds(monm.weakenDuration);
    destroyObject();
  }

  public bool isWeak() {
    return weak;
  }

  protected override float strength() {
    if (weak) {
      return 0.5f;
    } else {
      return objectsManager.strength;
    }
  }

  protected override void normalMovement() {
    direction = player.transform.position - transform.position;
    float distance = direction.magnitude;
    direction /= distance;
    if (distance > monm.detectDistance){
      rb.velocity = direction * speed_chase * 2;
    } else if (isMagnetized) {
      rb.velocity = direction * player.GetComponent<Rigidbody>().velocity.magnitude * 1.5f;
    } else if (weak) {
      rb.velocity = -direction * speed_weaken;
    } else if (player.isUnstoppable()) {
      rb.velocity = -direction * speed_runaway;
    } else {
      rb.velocity = direction * speed_chase;
    }

    if (weak) {
      if (shrinking) {
        shrinkedScale = Mathf.MoveTowards(shrinkedScale, monm.shrinkUntil, Time.deltaTime * monm.shrinkSpeed);
        if (shrinkedScale == monm.shrinkUntil) shrinking = false;
      } else {
        shrinkedScale = Mathf.MoveTowards(shrinkedScale, originalScale, Time.deltaTime * monm.restoreSpeed);
        if (shrinkedScale == originalScale) shrinking = true;
      }
      transform.localScale = new Vector3(shrinkedScale, shrinkedScale, shrinkedScale);
    }
  }

  public override void destroyObject(bool destroyEffect = true) {
    if (isInsideBlackhole && QuestManager.qm.doingQuest("DestroyMonsterByBlackhole")) {
      if (player.isUsingBlackhole()) {
        QuestManager.qm.addCountToQuest("DestroyMonsterByBlackhole");
      }
    }

    Destroy(gameObject);
    monm.indicator.stopIndicate();
    monm.stopWarning();
    if (destroyEffect) {
      Instantiate(monm.destroyEffect, transform.position, transform.rotation);
    }
    monm.run();
  }

  override public void encounterPlayer() {
    QuestManager.qm.addCountToQuest("DestroyMonster");

    if (player.isUsingRainbow()) {
      QuestManager.qm.addCountToQuest("DestroyMonsterByRainbow");
    }

    Destroy(gameObject);
    monm.indicator.stopIndicate();
    monm.stopWarning();
    Instantiate(monm.destroyEffect, transform.position, transform.rotation);
    monm.run();
  }

  override public string getManager() {
    return "MonsterManager";
  }

  override public bool dangerous() {
    if (weak || player.isUnstoppable() || player.isUsingRainbow() || player.isExitedBlackhole()) return false;
    else return true;
  }

  override public int cubesWhenEncounter() {
    return monm.cubesWhenDestroy;
  }

  public bool rideable() {
    return weak || player.isExitedBlackhole();
  }
}
