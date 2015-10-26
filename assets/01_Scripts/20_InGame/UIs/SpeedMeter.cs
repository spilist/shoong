using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedMeter : MonoBehaviour {
  public Text current;
  public Text maximum;

  public Color origCurrentColor;
  public Color largerCurrentColor;

	void Start () {
    current.text = "0";
    setMaximum(Player.pl.baseSpeed + Player.pl.maxBooster());
	}

	void Update () {
    current.text = Player.pl.getSpeed().ToString("0");

    if (largerThanMax()) current.color = largerCurrentColor;
    else current.color = origCurrentColor;
	}

  void setMaximum(int val) {
    maximum.text = "/" + val.ToString("0");
  }

  void setMaximum(float val) {
    maximum.text = "/" + val.ToString("0");
  }

  public void updateMaximum() {
    if (Player.pl.isOnSuperheat()) setMaximum(SuperheatManager.sm.baseSpeed + Player.pl.maxBooster());
    else setMaximum(Player.pl.baseSpeed + Player.pl.maxBooster());
  }

  bool largerThanMax() {
    int cur = int.Parse(current.text);
    int max = int.Parse(maximum.text.Replace("/", ""));
    return cur > max;
  }
}
