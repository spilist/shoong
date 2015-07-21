using UnityEngine;
using System.Collections;

public class FieldObjectsManager : MonoBehaviour {
	public GameObject[] obstacles;
	public GameObject[] parts;

	public int max_obstacles = 5;
	public int max_parts = 50;

	public float generateSpaceRadius = 5f;
	public float minimumGenerateDistance = 0.5f;

	public float speed_obstacles = 1;
	public float speed_parts = 10;
	public float speed_special = 5;
	private Hashtable speed;

	public float tumble_obstacles = 0.5f;
	public float tumble_parts = 5f;
	public float tumble_special = 5f;
	private Hashtable tumble;

	void Start () {
	}

	public void run() {
		generateObjectsAtStart();
		generateObjectValuesHashtable();
		gameObject.SetActive(true);
	}

	void generateObjectValuesHashtable() {
		speed = new Hashtable();
		speed.Add("Obstacle", speed_obstacles);
		speed.Add("Part", speed_parts);
		speed.Add("SpecialPart", speed_special);

		tumble = new Hashtable();
		tumble.Add("Obstacle", tumble_obstacles);
		tumble.Add("Part", tumble_parts);
		tumble.Add("SpecialPart", tumble_special);
	}

	void generateObjectsAtStart() {
		for (int i = 0; i < max_obstacles; i++) {
			instantiateFieldObject(obstacles);
		}

		for (int i = 0; i < max_parts; i++) {
			instantiateFieldObject(parts);
		}
	}

	private void instantiateFieldObject(GameObject[] objects) {
		GameObject target = objects[Random.Range(0, objects.Length)];
		spawn(target);
	}

	public GameObject spawn(GameObject target) {
		Vector3 spawnPosition = getSpawnPosition(target);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject newInstance = (GameObject) Instantiate (target, spawnPosition, spawnRotation);
		newInstance.transform.parent = gameObject.transform;
		return newInstance;
	}

	private Vector3 getSpawnPosition(GameObject target) {
		float screenX, screenY;
		screenX = Random.Range(0, minimumGenerateDistance + generateSpaceRadius * 2);
		if (generateSpaceRadius < screenX && screenX <= generateSpaceRadius + 1 + minimumGenerateDistance)
			screenX += generateSpaceRadius;

		screenY = Random.Range(0, minimumGenerateDistance + generateSpaceRadius * 2);
		if (generateSpaceRadius < screenY && screenY <= generateSpaceRadius + 1 + minimumGenerateDistance)
			screenY += generateSpaceRadius;

		screenX -= generateSpaceRadius;
		screenY -= generateSpaceRadius;

		Vector3 screenVector = new Vector3(screenX, screenY, Camera.main.transform.position.y);
		return Camera.main.ViewportToWorldPoint(screenVector);
	}

	void Update () {
		if (GameObject.FindGameObjectsWithTag ("Obstacle").Length < max_obstacles) {
			instantiateFieldObject(obstacles);
		}

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
}
