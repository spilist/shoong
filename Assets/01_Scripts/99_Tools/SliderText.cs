using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void setValue(Slider slider) {
    GetComponent<Text>().text = ((int) (slider.value * 100)) / 100.0 + "";
  }
}
