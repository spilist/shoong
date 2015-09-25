using UnityEngine;
using System.Collections;

public class BrokenAsteroids : MonoBehaviour {
	public string what;

	void Start () {
    if (what == "big") {
      AsteroidManager asm = GameObject.Find("Field Objects").GetComponent<AsteroidManager>();
      for (int howMany = Random.Range(asm.minBrokenSpawn, asm.maxBrokenSpawn + 1); howMany > 0; howMany--) {
        GameObject broken = (GameObject) Instantiate(transform.GetChild(Random.Range(0, transform.childCount)).gameObject, transform.position, Quaternion.identity);
        broken.transform.localScale = Random.Range(asm.minBrokenSize, asm.maxBrokenSize) * Vector3.one;
        broken.SetActive(true);
      }
    } else if (what == "small") {
      SmallAsteroidManager sam = GameObject.Find("Field Objects").GetComponent<SmallAsteroidManager>();
      for (int howMany = Random.Range(sam.minBrokenSpawn, sam.maxBrokenSpawn + 1); howMany > 0; howMany--) {
        GameObject broken = (GameObject) Instantiate(transform.GetChild(Random.Range(0, transform.childCount)).gameObject, transform.position, Quaternion.identity);
        broken.transform.localScale = Random.Range(sam.minBrokenSize, sam.maxBrokenSize) * Vector3.one;
        broken.SetActive(true);
      }
    }

    Destroy(gameObject, 2);
	}

}
