using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageGiftGold : MonoBehaviour {
  public int minGoldsGet = 1;
	public int maxGoldsGet = 3;
  int numGoldsGet = 0;

  GameOverGoldCubes ggc;

  void Awake() {
    ggc = transform.parent.parent.parent.parent.Find("GameOverGoldCubes").GetComponent<GameOverGoldCubes>();
  }

  public void show() {
    numGoldsGet = Random.Range(minGoldsGet, maxGoldsGet + 1);
    transform.Find("AmountText").GetComponent<Text>().text = "x" + numGoldsGet;
    gameObject.SetActive(true);
    GetComponent<AudioSource>().Play();
    ggc.change(numGoldsGet);
  }
}
