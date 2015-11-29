using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill_Solar : Skill {
  public float biggerScale = 4;
  private bool enlarging;
  private float curScale;
  public GameObject afterImagePrefab;
  private List<GameObject> afterImagePool;
  public int afterImageCount = 10;
  public float generatePer = 0.1f;
  public float afterImageDuration = 1;
  public Color originalColor;
  private CharacterChangeManager cm;

  override public void afterStart() {
    cm = Player.pl.GetComponent<CharacterChangeManager>();

    afterImagePool = new List<GameObject>();
    for (int i = 0; i < afterImageCount; ++i) {
      GameObject obj = (GameObject) Instantiate(afterImagePrefab);
      obj.SetActive(false);
      afterImagePool.Add(obj);
    }
  }

  override public void afterActivate(bool val) {
    if (val) {
      cm.changeCharacterTo("Solar");
      curScale = Player.pl.originalScale;
      enlarging = true;
      StartCoroutine("afterImage");
    } else {
      cm.changeCharacterToOriginal();
      Player.pl.afterStrengthenStart();
      StopCoroutine("afterImage");
    }
    RhythmManager.rm.setFever(val);
  }

  void Update() {
    if (enlarging) {
      curScale = Mathf.MoveTowards(curScale, biggerScale, Time.deltaTime * (biggerScale - Player.pl.originalScale) / 0.1f);
      Player.pl.transform.localScale = curScale * Vector3.one;
      if (curScale >= biggerScale) enlarging = false;
    }
  }

  IEnumerator afterImage() {
    while (true) {
      GameObject afterImage = getAfterImage();
      afterImage.transform.position = Player.pl.transform.position;
      afterImage.transform.rotation = Player.pl.transform.rotation;
      afterImage.SetActive(true);

      afterImage.GetComponent<PowerBoostAfterImageMover>().run(afterImageDuration, originalColor, biggerScale);
      // afterImage.GetComponent<PowerBoostAfterImageMover>().run(afterImageDuration, afterImageMainColors[index], afterImageEmissiveColors[index], transform.localScale.x);

      yield return new WaitForSeconds(generatePer);
    }
  }

  GameObject getAfterImage() {
    for (int i = 0; i < afterImagePool.Count; i++) {
      if (!afterImagePool[i].activeInHierarchy) {
        return afterImagePool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(afterImagePrefab);
    afterImagePool.Add(obj);
    return obj;
  }
}
