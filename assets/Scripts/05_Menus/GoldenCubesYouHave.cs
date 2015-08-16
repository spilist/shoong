using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldenCubesYouHave : MonoBehaviour {

  void Start () {
    GetComponent<Text>().text = ((int)GameController.control.goldenCubes["now"]).ToString();
  }
}
