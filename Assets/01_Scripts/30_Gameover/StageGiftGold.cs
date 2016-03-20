using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageGiftGold : MonoBehaviour {
  public int lowGolds = 1;
  public int middleGolds = 5;
  public int highGolds = 10;

  public int highProb = 5;
  public int middleProb = 15;

  int numGoldsGet = 0;

  GameOverGoldCubes ggc;
  GameObject openingParticle;

  void Awake() {
    ggc = transform.parent.parent.parent.parent.Find("GameOverGoldCubes").GetComponent<GameOverGoldCubes>();
  }

  public void show() {
    int prob = Random.Range(0, 100);
    if (prob < highProb) {
      numGoldsGet = lowGolds;
      openingParticle = transform.parent.Find("OpeningParticles_low").gameObject;
    } else if (prob < highProb + middleProb) {
      numGoldsGet = middleGolds;
      openingParticle = transform.parent.Find("OpeningParticles_mid").gameObject;
    } else {
      numGoldsGet = highGolds;
      openingParticle = transform.parent.Find("OpeningParticles_high").gameObject;
    }

    transform.Find("AmountText").GetComponent<Text>().text = "x" + numGoldsGet;
    gameObject.SetActive(true);
    openingParticle.SetActive(true);
    GetComponent<AudioSource>().Play();
    ggc.change(numGoldsGet);
  }
}
