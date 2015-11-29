using UnityEngine;
using System.Collections;

public class SuperheatMover : MonoBehaviour {
  public float generatePer = 0.1f;
  public float afterImageDuration = 1;
  public int rotatingSpeed = 1000;
  public Color[] afterImageMainColors;
  public Color[] afterImageEmissiveColors;
  public float sizeChangeInterval = 0.1f;
  public float middleSize = 3;
  public float bigSize = 6.5f;

  private GameObject superheatParticle;
  private Vector3 direction;
  private bool rotating = false;

  void Awake() {
    superheatParticle = transform.Find("Particle").gameObject;
  }

  void OnEnable() {
    Player.pl.rb.isKinematic = true;
    Player.pl.startPowerBoost();
    StartCoroutine("superHeat");
  }

  IEnumerator superHeat() {
    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * bigSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * middleSize;

    yield return new WaitForSeconds(sizeChangeInterval);
    transform.localScale = Vector3.one * bigSize;

    Player.pl.rb.isKinematic = false;
    AudioManager.am.startPowerBoost();
    superheatParticle.SetActive(true);
    rotating = true;
    SuperheatManager.sm.setStatus();

    int index = 0;

    while (true) {
      GameObject afterImage = SuperheatManager.sm.getAfterImage();
      afterImage.transform.position = transform.position;
      afterImage.transform.rotation = transform.rotation;
      afterImage.SetActive(true);

      if (index >= afterImageMainColors.Length) index = 0;

      // afterImage.GetComponent<PowerBoostAfterImageMover>().run(afterImageDuration, transform.localScale.x);
      // afterImage.GetComponent<PowerBoostAfterImageMover>().run(afterImageDuration, afterImageMainColors[index], afterImageEmissiveColors[index], transform.localScale.x);
      index++;

      yield return new WaitForSeconds(generatePer);
    }
  }

  void Update() {
    transform.position = Player.pl.transform.position;

    if (rotating) transform.Rotate(Player.pl.getDirection() * Time.deltaTime * rotatingSpeed);
  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover == null) return;
    Player.pl.goodPartsEncounter(mover, mover.cubesWhenDestroy(), false);
  }


  void OnDisable() {
    StopCoroutine("superHeat");
    superheatParticle.SetActive(false);
    transform.localScale = Vector3.one;
    rotating = false;
    AudioManager.am.stopPowerBoost();
    Player.pl.stopPowerBoost();
  }
}
