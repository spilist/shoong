using UnityEngine;
using System.Collections;

public class PatternPartsMover : MonoBehaviour {
  private PatternPartsManager ppm;
  private ParticleSystem appp;
  private PatternPartsGroup ppg;
  private MeshRenderer mRenderer;
  private bool activated = false;

  void Start () {
    FieldObjectsManager fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    float tumble = fom.getTumble(gameObject.tag);
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    ppm = GameObject.Find("Field Objects").GetComponent<PatternPartsManager>();
    appp = transform.Find("ActivePatternPartsParticle").GetComponent<ParticleSystem>();
    ppg = transform.parent.GetComponent<PatternPartsGroup>();
    mRenderer = GetComponent<MeshRenderer>();
	}

  public void becomeActive() {
    activated = true;
    mRenderer.material = ppm.activePatternPartsMaterial;
    appp.Play();
    ppg.destroyAfterCount();
  }

  public bool isActive() {
    return activated;
  }
}
