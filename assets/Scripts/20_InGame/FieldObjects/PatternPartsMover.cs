using UnityEngine;
using System.Collections;

public class PatternPartsMover : MonoBehaviour {
  private PatternPartsManager ppm;
  private ParticleSystem appp;
  private PatternPartsGroup ppg;
  // private MeshRenderer mRenderer;
  private bool activated = false;

  private Material mat;
  private Color color;

  void Start () {
    FieldObjectsManager fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    float tumble = fom.getTumble(gameObject.tag);
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;

    ppm = GameObject.Find("Field Objects").GetComponent<PatternPartsManager>();
    appp = transform.Find("ActivePatternPartsParticle").GetComponent<ParticleSystem>();
    ppg = transform.parent.GetComponent<PatternPartsGroup>();

    mat = GetComponent<Renderer>().material;
    color = mat.GetColor("_TintColor");

    // mRenderer = GetComponent<MeshRenderer>();
	}

  public void becomeActive() {
    activated = true;
    // mRenderer.material = ppm.activePatternPartsMaterial;
    appp.Play();
    ppg.destroyAfterCount();
  }

  public bool isActive() {
    return activated;
  }

  public void startBlink() {
    StartCoroutine("blink");
  }

  IEnumerator blink() {
    while(true) {
      color.a = 0.08f;
      mat.SetColor("_TintColor", color);
      yield return new WaitForSeconds(ppm.blinkingInvisible);

      color.a = 0.28f;
      mat.SetColor("_TintColor", color);
      yield return new WaitForSeconds(ppm.blinkingVisible);
    }
  }
}
