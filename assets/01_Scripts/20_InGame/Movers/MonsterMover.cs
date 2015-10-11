using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonsterMover : ObjectsMover {
  private float speed_runaway;
  private float speed_weaken;
  private float lifeTime;
  private float minimonRespawnTime;
  private int minimonCount = 0;

  private MonsterManager monm;
  private SpecialPartsManager spm;

  private float originalScale;
  private bool weak = false;
  private bool shrinking = false;

  private Renderer m_renderer;

  protected override void initializeRest() {
    monm = (MonsterManager)objectsManager;
    spm = GameObject.Find("Field Objects").GetComponent<SpecialPartsManager>();

    originalScale = transform.localScale.x;
    speed_runaway = monm.speed_runaway;
    speed_weaken = monm.speed_weaken;

    monm.indicator.startIndicate(gameObject);
    monm.indicator.GetComponent<Image>().color = Color.red;

    lifeTime = Random.Range(monm.minLifeTime, monm.maxLifeTime);
    minimonRespawnTime = lifeTime / monm.numMinimonRespawn;

    m_renderer = GetComponent<Renderer>();

    StartCoroutine("weakened");
  }

  IEnumerator weakened() {
    while (minimonCount < monm.numMinimonRespawn) {
      minimonCount++;
      shrinking = true;
      Instantiate(monm.minimonPrefab, transform.position, transform.rotation);

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
    } else if (player.isUnstoppable()) {
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
    if (isInsideBlackhole && QuestManager.qm.doingQuest("DestroyMonsterByBlackhole") && player.isUsingBlackhole()) {
      QuestManager.qm.addCountToQuest("DestroyMonsterByBlackhole");
    }

    monm.indicator.stopIndicate();
    monm.stopWarning();
  }

  override public void encounterPlayer(bool destroy = true) {
    foreach (Collider collider in GetComponents<Collider>()) {
      collider.enabled = false;
    }
    Destroy(gameObject);

    monm.indicator.stopIndicate();
    monm.stopWarning();

    if (rideable()) {
      objectsManager.objEncounterEffectForPlayer.Play();
      objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().Play();

      objectsManager.strengthenPlayerEffect.SetActive(true);
      player.strengthenBy(tag);

      monm.monsterFilter.SetActive(true);
      QuestManager.qm.addCountToQuest("RideMonster");
      if (player.isExitedBlackhole()) {
        QuestManager.qm.addCountToQuest("RideMonsterByBlackhole");
      }

      if (player.energyBar.currentEnergy() <= 30) {
        QuestManager.qm.addCountToQuest("RideMonsterWithLowEnergy");
      }

      if (player.isUsingDopple()) {
        QuestManager.qm.addCountToQuest("RideMonsterByDopple");
      }

      player.encounterObject(tag);
    }
    else {
      QuestManager.qm.addCountToQuest("DestroyMonster");

      if (player.isUsingRainbow()) {
        QuestManager.qm.addCountToQuest("DestroyMonsterByRainbow");
      }

      Instantiate(objectsManager.objEncounterEffect, transform.position, transform.rotation);
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

  override public int bonusCubes() {
    return player.isUnstoppable()? (int) (cubesWhenEncounter() * spm.bonus) : 0;
  }

  public bool rideable() {
    return !player.isUnstoppable() && (weak || player.isExitedBlackhole());
  }
}
