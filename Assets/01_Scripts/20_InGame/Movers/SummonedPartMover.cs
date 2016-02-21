using UnityEngine;
using System.Collections;

public class SummonedPartMover : ObjectsMover {
  SummonPartsManager summonManager;
  Renderer mRenderer;
  private Skill_Gold goldSkill;
  private Animation beatAnimation;

  override protected void initializeRest() {
    summonManager = (SummonPartsManager) objectsManager;
    mRenderer = GetComponent<Renderer>();
    goldSkill = (Skill_Gold)SkillManager.sm.getSkill("Gold");
    beatAnimation = GetComponent<Animation>();
    beatAnimation.wrapMode = WrapMode.Once;
    RhythmManager.rm.registerCallback(GetInstanceID(), () => {
      beatAnimation.Play();
    });
  }

  override protected void afterEnable() {
    mRenderer.enabled = true;
    StartCoroutine("destroyAfter");
  }

  override public void destroyObject (bool destroyEffect = true, bool byPlayer = false, bool resapwn = true) {

    gameObject.SetActive(false);

    if (destroyEffect) {
      showDestroyEffect(byPlayer);
    }
  }

  override public string getManager() {
    return "SummonPartsManager";
  }

  IEnumerator destroyAfter() {
    yield return new WaitForSeconds(summonManager.summonedPartLifetime - summonManager.blinkBeforeDestroy);
    float duration = summonManager.blinkBeforeDestroy;
    float showDuring = summonManager.showDurationStart;
    float emptyDuring = summonManager.emptyDurationStart;
    float showDurationDecrease = summonManager.showDurationDecrease;
    float emptyDurationDecrease = summonManager.emptyDurationDecrease;

    while (duration > 0) {
      mRenderer.enabled = true;

      yield return new WaitForSeconds (showDuring);

      mRenderer.enabled = false;
      yield return new WaitForSeconds (emptyDuring);

      duration -= showDuring + emptyDuring;

      if(showDuring > 1f) showDuring -= showDurationDecrease;
      if(emptyDuring > 0.5f) emptyDuring -= emptyDurationDecrease;
    }

    destroyObject();
  }

  override public bool hasEncounterEffect() {
    return false;
  }

  public void transformToGold(Vector3 pos) {
    GameObject laser = goldSkill.getLaser(pos);
    laser.SetActive(true);
    laser.GetComponent<TransformLaser>().shoot(transform.position, goldSkill.laserShootDuration);

    Invoke("changeToGold", goldSkill.laserShootDuration);
  }

  void changeToGold() {
    destroyObject(false);
    goldSkill.getParticle(transform.position);
    summonManager.gcm.spawnGoldenCube(transform.position);
  }
}
