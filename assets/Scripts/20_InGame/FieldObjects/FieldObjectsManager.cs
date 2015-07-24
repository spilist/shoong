using UnityEngine;
using System.Collections;

public class FieldObjectsManager : MonoBehaviour {
	public GameObject[] parts;

	public int max_parts = 50;

	public float generateSpaceRadius = 5f;
	public float generateOffset = 0.2f;
	public int partsMinDistance = 50;
 	public LayerMask fieldObjectsLayerMask;

	// public float speed_obstacles = 1;
	public float speed_parts = 10;
	public float speed_special = 5;
	private Hashtable speed;

	// public float tumble_obstacles = 0.5f;
	public float tumble_parts = 5f;
	public float tumble_special = 5f;
	private Hashtable tumble;

	public float unstoppableFollowSpeed = 1.1f;

	void Start () {
	}

	public void run() {
		gameObject.SetActive(true);
		generateObjectsAtStart();
		generateObjectValuesHashtable();
	}

	void generateObjectValuesHashtable() {
		speed = new Hashtable();
		// speed.Add("Obstacle", speed_obstacles);
		speed.Add("Part", speed_parts);
		speed.Add("SpecialPart", speed_special);

		tumble = new Hashtable();
		// tumble.Add("Obstacle", tumble_obstacles);
		tumble.Add("Part", tumble_parts);
		tumble.Add("SpecialPart", tumble_special);
	}

	void generateObjectsAtStart() {
		for (int i = 0; i < max_parts; i++) {
			instantiateFieldObject(parts);
		}
	}

	private void instantiateFieldObject(GameObject[] objects) {
		GameObject target = objects[Random.Range(0, objects.Length)];
		spawn(target);
	}

	public GameObject spawn(GameObject target) {
		Vector3 spawnPosition = getSpawnPosition();
		Quaternion spawnRotation = Quaternion.identity;
		GameObject newInstance = (GameObject) Instantiate (target, spawnPosition, spawnRotation);
		newInstance.transform.parent = gameObject.transform;
		return newInstance;
	}

	private Vector3 getSpawnPosition() {
		float screenX, screenY;
		Vector3 spawnPosition;
		int count = 0;
		do {
			do {
				screenX = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
				screenY = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
			} while(-generateOffset < screenX && screenX < generateOffset + 1 && -generateOffset < screenY && screenY < generateOffset + 1);

			spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(screenX, screenY, Camera.main.transform.position.y));
		} while(Physics.OverlapSphere(spawnPosition, partsMinDistance, fieldObjectsLayerMask).Length > 0 && count++ < 100);

		if (count >= 100) Debug.Log(count);

		return spawnPosition;
	}

	void Update () {
		if (GameObject.FindGameObjectsWithTag ("Part").Length < max_parts) {
			instantiateFieldObject(parts);
		}

	}

	public float getSpeed(string tag) {
		if (tag == "Untagged") {
			Debug.LogError("Try to get speed of untagged");
			return 0;
		}
		else {
			return (float)speed[tag];
		}
	}

	public float getTumble(string tag) {
		if (tag == "Untagged") {
			Debug.LogError("Try to get tumble of untagged");
			return 0;
		}
		else {
			return (float)tumble[tag];
		}
	}

	public float getUnstoppableFollowSpeed() {
		return unstoppableFollowSpeed;
	}
}
