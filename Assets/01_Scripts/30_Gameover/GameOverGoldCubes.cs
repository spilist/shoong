using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverGoldCubes : MonoBehaviour {
  public CharacterCreateBannerButton ccbb;
  public int increasingSpeed = 100;
  public AudioSource increasingSound;
  private Text text;
  private float current;
  private int target;
  private bool increasing = false;

  void Awake() {
    text = GetComponent<Text>();
    target = GoldManager.gm.getCount();
    current = target;
    text.text = current.ToString();
  }

	public void change(int amount, bool changeData = true) {
    if (changeData) GoldManager.gm.addOutsideGame(amount);

    target = (int)current + amount;

    if (amount > 0) {
      increasing = true;
      increasingSound.Play();
      ccbb.decreaseToGo(amount, increasingSpeed);
    } else {
      increasing = false;
      current = target;
      text.text = target.ToString();
    }
  }

  void Update() {
    if (increasing) {
      current = Mathf.MoveTowards(current, target, Time.deltaTime * increasingSpeed);
      text.text = current.ToString("0");
      if (current == target) {
        increasing = false;
        increasingSound.Stop();
      }
    }
  }

  public int currentAmount() {
    return target;
  }
}
