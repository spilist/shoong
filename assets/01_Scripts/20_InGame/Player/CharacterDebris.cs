using UnityEngine;
using System.Collections;

public class CharacterDebris : MonoBehaviour {
	void Start () {
    ScoreManager scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    for (int howMany = scoreManager.numDebrisSpawn; howMany > 0; howMany--) {
      GameObject broken = (GameObject) Instantiate(transform.GetChild(Random.Range(0, transform.childCount)).gameObject, transform.position, Quaternion.identity);
      broken.transform.localScale = Random.Range(scoreManager.minDebrisSize, scoreManager.maxDebrisSize) * Vector3.one;
      broken.SetActive(true);
    }

    Destroy(gameObject, 2);
	}
}
