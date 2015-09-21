using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsCollector : MonoBehaviour {
	public PlayerMover player;
	public ParticleSystem collecteffect;
	public ParticleSystem particleeffect;
  public GameObject howManyCubesGet;
  public Text cubesCount;

  public int maxCubesGet = 2000;
	public int maxEmission = 1000;
  public float startOffset = 20f;
  public float startScale = 10f;
	public float startEmission = 0;
  public float maxScale = 35f;

  private float offset;
  private float scaleDifference;
	private float emissionDifference;
  private Rigidbody rb;

  private Vector3 pastPlayerPos;

  void Start() {
    offset = startOffset;
		emissionDifference = maxEmission - startEmission;
    scaleDifference = maxScale - startScale;
    rb = GetComponent<Rigidbody>();
  }

  void Update() {
    if (player.isUsingDopple()) {
      if (pastPlayerPos != player.transform.position) {
        followUserOnDopple();
        pastPlayerPos = player.transform.position;
      }
    } else {
      Vector3 heading = new Vector3 (player.transform.position.x - player.getDirection().x * offset - transform.position.x, 0, player.transform.position.z - player.getDirection().z * offset - transform.position.z);

      if (heading.magnitude < 5.0f) {
        heading /= heading.magnitude;
        rb.velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude;
      } else {
        heading /= heading.magnitude;
        rb.velocity = heading * player.GetComponent<Rigidbody>().velocity.magnitude * 1.3f;
      }
    }

		transform.rotation = player.transform.rotation;
  }

  public void followUserOnDopple() {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 dir = new Vector3(randomV.x, 0, randomV.y);
    transform.position = new Vector3 (player.transform.position.x - dir.x * offset, 0, player.transform.position.z - dir.z * offset);
    rb.velocity = Vector3.zero;
  }

	public void effect(bool triggerCubesGet, int count, bool playerGeneration){
    if (triggerCubesGet) {
      // GameObject cubesGetInstance = Instantiate(howManyCubesGet);
      // cubesGetInstance.transform.SetParent(transform.parent.Find("Bars Canvas"), false);
      // cubesGetInstance.transform.position = transform.position;
      // cubesGetInstance.GetComponent<ShowChangeText>().run(count);
      increaseSize(count);
    }

    if (!playerGeneration) {
      player.addCubeCount();
    }

    collecteffect.Play ();
    GetComponent<AudioSource>().Play();
    // cubesCount.text = (int.Parse(cubesCount.text) + 1).ToString();
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
