using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartsCollector : MonoBehaviour {
	public ParticleSystem collecteffect;
	public ParticleSystem particleeffect;
  public GameObject howManyCubesGet;
  public Text cubesCount;
  public Material enchanted;

	public int headFollowingSpeed = 50;
  public int maxEmission = 1000;
  public float startOffset = 20f;
  public float startEmission = 0;
  private float offset;
	private float emissionDifference;
  private Rigidbody rb;

  private Vector3 pastPlayerPos;
  private bool followingUser = true;
  float angleY;

  void Start() {
    offset = startOffset;
    rb = GetComponent<Rigidbody>();
    // checkEnchant();
  }

  public void checkEnchant() {
    if (DataManager.dm.getBool("GoldenCollector")) {
      GetComponent<Renderer>().sharedMaterial = enchanted;
    }
  }

  float getAngle() {
    Vector3 dir = Player.pl.transform.position - transform.position;
    dir = dir / dir.magnitude;
    Quaternion rotation = Quaternion.LookRotation(dir);
    float targetAngle = rotation.eulerAngles.y;
    if (Mathf.Abs(targetAngle - angleY) > 180) targetAngle -= 360;
    return targetAngle;
  }

  void Update() {
    if (followingUser) {
      // if (Player.pl.isUsingDopple()) {
        // if (pastPlayerPos != Player.pl.transform.position) {
          // followUserOnDopple();
          // pastPlayerPos = Player.pl.transform.position;
        // }
      // } else {
        Vector3 heading = new Vector3 (Player.pl.transform.position.x - Player.pl.getDirection().x * offset - transform.position.x, 0, Player.pl.transform.position.z - Player.pl.getDirection().z * offset - transform.position.z);

        if (heading.magnitude < 5.0f) {
          heading /= heading.magnitude;
          rb.velocity = heading * Player.pl.getSpeed();
        } else {
          heading /= heading.magnitude;
          rb.velocity = heading * Player.pl.getSpeed() * 1.3f;
        }
      // }

      float angle = getAngle();
      angleY = Mathf.MoveTowards(angleY, angle, Time.deltaTime * headFollowingSpeed);
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, angleY, transform.eulerAngles.z);
    }
  }

  public void followUserOnDopple() {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();
    Vector3 dir = new Vector3(randomV.x, 0, randomV.y);
    transform.position = new Vector3 (Player.pl.transform.position.x - dir.x * offset, 0, Player.pl.transform.position.z - dir.z * offset);
    rb.velocity = Vector3.zero;
  }

	public void effect() {
		if (!followingUser) return;

    collecteffect.Play ();
    GetComponent<AudioSource>().Play();
  }

  public void addEmission(int count) {
    if (particleeffect.emissionRate < maxEmission) {
			// particleeffect.emissionRate += 1 * (maxEmission * count / (float) cubeRequired);
  	}
  }
}
