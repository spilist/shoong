using UnityEngine;
using System.Collections;

public class PatternPartsMover : MonoBehaviour {
  void Start () {
    FieldObjectsManager fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    float tumble = fom.getTumble(gameObject.tag);
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
	}

  public void becomeActive() {
    GetComponent<MeshRenderer>().material = GameObject.Find("Field Objects").GetComponent<PatternPartsManager>().activePatternPartsMaterial;
    transform.Find("ActivePatternPartsParticle").GetComponent<ParticleSystem>().Play();
  }
}
