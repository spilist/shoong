using UnityEngine;
using System.Collections;

public class GoogleAuthManager : MonoBehaviour {

	// Use this for initialization
  void Start () {
    // By the implementation of OnOffButton, 'false' actually means 'is logged in'
    if (DataManager.dm.getBool("GoogleLoggedInSetting") == false) {
      DataManager.npbManager.authenticate(null);
    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
