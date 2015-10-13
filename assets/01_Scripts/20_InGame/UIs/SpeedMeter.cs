using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedMeter : MonoBehaviour {
  public Superheat superheat;
  public PlayerMover player;
  public Text current;
  public Text maximum;

  public Color origCurrentColor;
  public Color largerCurrentColor;

	void Start () {
    current.text = "0";
    setMaximum(player.baseSpeed + player.maxBooster());
	}

	void Update () {
    current.text = player.getSpeed().ToString("0");

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
    if (player.isOnSuperheat()) setMaximum(superheat.baseSpeed + player.maxBooster());
    else setMaximum(player.baseSpeed + player.maxBooster());
  }

  bool largerThanMax() {
    int cur = int.Parse(current.text);
    int max = int.Parse(maximum.text.Replace("/", ""));
    return cur > max;
  }
}
