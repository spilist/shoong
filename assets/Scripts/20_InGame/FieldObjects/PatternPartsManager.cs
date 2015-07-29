using UnityEngine;
using System.Collections;

public class PatternPartsManager : MonoBehaviour {
  public GameObject patternPartsPrefab;
  public Material activePatternPartsMaterial;

  public float group_speed = 10;

	public void run() {
    GameObject patternInstantiated = GetComponent<FieldObjectsManager>().spawn(patternPartsPrefab);
    patternInstantiated.GetComponent<PatternPartsGroup>().run();
  }
}
