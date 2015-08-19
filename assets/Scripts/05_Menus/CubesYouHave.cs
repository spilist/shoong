using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CubesYouHave : MonoBehaviour {

	void OnEnable () {
    GetComponent<Text>().text = ((int)GameController.control.cubes["now"]).ToString();
	}
}
