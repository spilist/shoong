using UnityEngine;
using System.Collections;

public class PartsCollector : MonoBehaviour {
	public PlayerMover player;
	public ParticleSystem collecteffect;
	public ParticleSystem particleeffect;

  public int maxCubesGet = 2000;
	public int maxEmission = 1000;
  public float startOffset = 20f;
  public float startScale = 10f;
	public float startEmission = 0;
  public float maxScale = 35f;

  private float offset;
  private float scaleDifference;
	private float emissionDifference;

  void Start() {
    offset = startOffset;
		emissionDifference = maxEmission - startEmission;
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
	}

  public void increaseSize(int partsGet) {
    if (transform.localScale.x < maxScale) {
      transform.localScale += Vector3.one * (scaleDifference * partsGet / (float) maxCubesGet);
     offset = transform.localScale.x / 2 + startOffset - startScale / 2;
    }
		if (particleeffect.emissionRate < maxEmission) {
			particleeffect.emissionRate += 1 * (emissionDifference * partsGet / (float) maxCubesGet);
  	}
  }
}
