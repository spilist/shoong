using UnityEngine;
using System.Collections;

public class PartsCollector : MonoBehaviour {
	public PlayerMover player;
  public ComboBar comboBar;
  public GameObject collected;
	public ParticleSystem collecteffect;

  public int maxPartsGet = 1000;
  public float startOffset = 20f;
  public float startScale = 10f;
  public float maxScale = 35f;

  private float offset;
  private float scaleDifference;

  void Start() {
    offset = startOffset;
    scaleDifference = maxScale - startScale;
  }

  void Update() {
    Vector3 heading = new Vector3 (player.transform.position.x - player.getDirection().x * offset - transform.position.x, 0, player.transform.position.z - player.getDirection().z * offset - transform.position.z);

    if (heading.magnitude < 5.0f) {
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude;
    } else {
      heading /= heading.magnitude;
      GetComponent<Rigidbody> ().velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.3f;
    }
    transform.rotation = player.transform.rotation;
  }

	public void effect(){
		collecteffect.Play ();
		GetComponent<AudioSource>().Play();

    Vector3 rndPosWithin = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
    rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
    GameObject newInstance = (GameObject) Instantiate(collected, rndPosWithin, Quaternion.identity);
    // newInstance.transform.parent = transform;
	}

  public void increaseSize(int partsGet) {
    if (transform.localScale.x < maxScale) {
      transform.localScale += Vector3.one * (scaleDifference * partsGet / (float) maxPartsGet);
      offset = transform.localScale.x / 2 + startOffset - startScale / 2;
    }
  }
}
