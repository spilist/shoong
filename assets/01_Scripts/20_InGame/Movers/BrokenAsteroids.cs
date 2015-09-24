using UnityEngine;
using System.Collections;

public class BrokenAsteroids : MonoBehaviour {
	AsteroidManager asm;

	void Start () {
    asm = GameObject.Find("Field Objects").GetComponent<AsteroidManager>();

    for (int howMany = Random.Range(asm.minBrokenSpawn, asm.maxBrokenSpawn + 1); howMany > 0; howMany--) {
      GameObject broken = (GameObject) Instantiate(transform.GetChild(Random.Range(0, transform.childCount)).gameObject, transform.position, Quaternion.identity);
      broken.transform.localScale = Random.Range(asm.minBrokenSize, asm.maxBrokenSize) * Vector3.one;
      broken.SetActive(true);
    }

    Destroy(gameObject, 2);
	}

}
