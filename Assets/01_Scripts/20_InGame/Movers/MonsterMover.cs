using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterMover : ObjectsMover {
  private float speed_runaway;
  private float speed_weaken;
  private float minimonRespawnTime;
  private int minimonCount = 0;

  private MonsterManager monm;

  private bool weak = false;
  private bool shrinking = false;

  private Renderer m_renderer;
  private Color originalColor;
  private Animation beatAnimation;

  protected override void initializeRest() {
    monm = (MonsterManager)objectsManager;

    speed_runaway = monm.speed_runaway;
    speed_weaken = monm.speed_weaken;

    float lifeTime = Random.Range(monm.minLifeTime, monm.maxLifeTime);
    minimonRespawnTime = lifeTime / monm.numMinimonRespawn;

    m_renderer = GetComponent<Renderer>();
    originalColor = m_renderer.material.GetColor("_OutlineColor");
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  override protected void afterEnable() {
    monm.indicator.startIndicate(gameObject);
    monm.indicator.GetComponent<Image>().color = Color.red;
    minimonCount = 0;
    weak = false;
    shrinking = false;
    transform.Find("Aura").gameObject.SetActive(true);
    transform.Find("Weaken Aura").gameObject.SetActive(false);

    m_renderer.material.SetColor("_OutlineColor", originalColor);
    m_renderer.material.SetFloat("_Outline", 1.5f);

    StartCoroutine("weakened");
  }

  IEnumerator weakened() {
    while (minimonCount < monm.numMinimonRespawn) {
      minimonCount++;
      shrinking = true;
      monm.spawnMinimon(transform.position);

      yield return new WaitForSeconds(minimonRespawnTime);
    }

    weak = true;
    monm.stopWarning();

    transform.Find("Aura").gameObject.SetActive(false);
    transform.Find("Weaken Aura").gameObject.SetActive(true);

    m_renderer.material.SetColor("_OutlineColor", monm.weakenedOutlineColor);
    m_renderer.material.SetFloat("_Outline", 0.75f);
    monm.indicator.GetComponent<Image>().color = monm.weakenedOutlineColor;

    yield return new WaitForSeconds(monm.weakenDuration);
    destroyObject();
  }

  protected override void normalMovement() {
    direction = getDirection();
    float distance = Vector3.Distance(player.transform.position, transform.position);
    if (distance > monm.detectDistance) {
      rb.velocity = direction * speed * 2;
    } else if (isMagnetized) {
      rb.velocity = direction * player.getSpeed() * 1.5f;
    } else if (weak) {
      rb.velocity = -direction * speed_weaken;
    } else if (player.isUnstoppable() || player.isUsingMagnet()) {
      rb.velocity = -direction * speed_runaway;
    } else {
      rb.velocity = direction * speed;
    }

    if (shrinking) {
      float until = originalScale - (originalScale - monm.shrinkUntil) * minimonCount / monm.numMinimonRespawn;
      shrinkedScale = Mathf.MoveTowards(shrinkedScale, until, Time.deltaTime * monm.shrinkSpeed);
      if (shrinkedScale == until) shrinking = false;

      transform.localScale = Vector3.one * shrinkedScale;
    }
  }

  override protected void afterDestroy(bool byPlayer) {
    monm.indicator.stopIndicate();
    monm.stopWarning();
    StopCoroutine("weakened");
  }

  override public void encounterPlayer(bool destroy = true) {
    gameObject.SetActive(false);

    monm.indicator.stopIndicate();
    monm.stopWarning();
    StopCoroutine("weakened");

    if (rideable()) {
      objectsManager.objEncounterEffectForPlayer.Play();
      objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();

      objectsManager.strengthenPlayerEffect.SetActive(true);
      player.effectedBy(tag);

      monm.monsterFilter.SetActive(true);
    }
    else {
      showEncounterEffect();
      monm.run();

      player.destroyObject(tag);
    }
  }

  override public string getManager() {
    return "MonsterManager";
  }

  override public bool dangerous() {
    if (rideable() || player.isInvincible()) return false;
    else return true;
  }

  override public int cubesWhenEncounter() {
    return rideable()? 0 : objectsManager.cubesWhenEncounter();
  }

  public bool rideable() {
    return !player.isUnstoppable() && (weak || player.isUsingMagnet());
  }
}
