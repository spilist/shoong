using UnityEngine;
using System.Collections;

public class PatternPartsManager : MonoBehaviour {
  public GameObject patternPartsPrefab;
  public Material activePatternPartsMaterial;
  public GameObject particlesPrefab;
  public int destroyAfter = 3;
  public float blinkingInvisible = 0.2f;
  public float blinkingVisible = 0.6f;
  public int spawnAfter = 3;
  public int numBonusParts = 5;

  public float group_speed = 10;

	public void run() {
    GameObject patternInstantiated = GetComponent<FieldObjectsManager>().spawn(patternPartsPrefab);
    patternInstantiated.GetComponent<PatternPartsGroup>().run();
  }

  public void spawnReady() {
    StartCoroutine("runAfterSeconds");
  }

  IEnumerator runAfterSeconds() {
    yield return new WaitForSeconds(spawnAfter);
    run();
  }
}
