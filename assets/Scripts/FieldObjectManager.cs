using UnityEngine;
using System.Collections;

public class FieldObjectManager : MonoBehaviour {
	private ArrayList partsList;
	private const int xPosInterval = 30;
	private const int zPosInterval = 60;
	private const float pi = 3.141592f;

	public GameObject unit_part1;
	public GameObject unit_part2;
	public GameObject unit_part3;
	public GameObject unit_spaceship_broken;
	public GameObject unit_spaceship_ok;

	public GameObject[] obstacles_big;
	public GameObject[] obstacles_middle;
	public GameObject[] obstacles_small;
	public GameObject[] parts;
	public GameObject[] energies;

	public int max_obstacles_big = 5;
	public int max_obstacles_middle = 40;
	public int max_obstacles_small = 10;
	public int max_parts = 50;
	public int max_energies = 5;
	// public int generateSpaceRadius = 2000;
	// public int minimumGenerateDistance = 100;
	public float generateSpaceRadius = 5f;
	public float minimumGenerateDistance = 0.5f;

	public float speed_obstacle_big = 1;
	public float speed_obstacle_middle = 5;
	public float speed_obstacle_small = 10;
	public float speed_part = 10;
	public float speed_energy = 10;
	private Hashtable speed;

	public float tumble_obstacle_big = 0.5f;
	public float tumble_obstacle_middle = 1f;
	public float tumble_obstacle_small = 2f;
	public float tumble_part = 5f;
	public float tumble_energy = 5f;
	private Hashtable tumble;


	GameObject spaceship_broken = null;
	GameObject spaceship_ok = null;

	// Use this for initialization
	void Start () {
		generateObjectValuesHashtable();
		// partsList = new ArrayList();
		gameObject.SetActive(false);
		// init (2, 2, 2);

		generateObjectsAtStart();
	}

	void generateObjectValuesHashtable() {
		speed = new Hashtable();
		speed.Add("Obstacle_big", speed_obstacle_big);
		speed.Add("Obstacle_middle", speed_obstacle_middle);
		speed.Add("Obstacle_small", speed_obstacle_small);
		speed.Add("Part", speed_part);
		speed.Add("Energy", speed_energy);

		tumble = new Hashtable();
		tumble.Add("Obstacle_big", tumble_obstacle_big);
		tumble.Add("Obstacle_middle", tumble_obstacle_middle);
		tumble.Add("Obstacle_smaller", tumble_obstacle_middle);
		tumble.Add("Obstacle_small", tumble_obstacle_small);
		tumble.Add("Part", tumble_part);
		tumble.Add("Energy", tumble_energy);
	}

	void generateObjectsAtStart() {
		for (int i = 0; i < max_obstacles_big; i++) {
		instantiateFieldObject(obstacles_big);
		 }

		for (int i = 0; i < max_obstacles_middle; i++) {
			instantiateFieldObject(obstacles_middle);
		}

		for (int i = 0; i < max_obstacles_small; i++) {
		instantiateFieldObject(obstacles_small);
		}

		 for (int i = 0; i < max_parts; i++) {
		instantiateFieldObject(parts);
		 }

		for (int i = 0; i < max_energies; i++) {
			instantiateFieldObject(energies);
		}
	}

	private void instantiateFieldObject(GameObject[] objects) {
		GameObject target = objects[Random.Range(0, objects.Length)];

		Vector3 spawnPosition = getSpawnPosition(target);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject newInstance = (GameObject) Instantiate (target, spawnPosition, spawnRotation);
		newInstance.transform.parent = gameObject.transform;
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

	// void init(int part1, int part2, int part3) {
	// 	for (int i = 0;i < part1;i++) {
	// 		partsList.Add(instantiateFieldObject(unit_part1));
	// 	}
	// 	for (int i = 0;i < part2;i++) {
	// 		partsList.Add(instantiateFieldObject(unit_part2));
	// 	}
	// 	for (int i = 0;i < part3;i++) {
	// 		partsList.Add(instantiateFieldObject(unit_part3));
	// 	}
	// 	spaceship_broken = instantiateFieldObject(unit_spaceship_broken);
	// 	spaceship_ok = (GameObject) Instantiate (unit_spaceship_ok, getNextRandomPosition(), Quaternion.identity);
	// 	spaceship_ok.SetActive(false);
	// }

	public void run() {
		gameObject.SetActive(true);
		foreach (Rigidbody fieldObjectRb in gameObject.GetComponentsInChildren<Rigidbody>()) {
			// fieldObjectRb.AddForce(getRandomUnitVector());
			fieldObjectRb.angularVelocity = Random.insideUnitSphere * (float) tumble[fieldObjectRb.tag];
		}
	}

	private Vector3 getRandomUnitVector() {
		float seed = Random.Range (0.0f, 1.0f) * 2 * pi;
		Vector3 direction = new Vector3 (
			Mathf.Cos(seed),
			0.0f,
			Mathf.Sin(seed));
		return direction.normalized;
	}

	// private GameObject instantiateFieldObject(GameObject prefab) {
	// 	GameObject newInstance = (GameObject) Instantiate (prefab, getNextRandomPosition(), Quaternion.identity);
	// 	newInstance.transform.parent = gameObject.transform;
	// 	return newInstance;
	// }

	private Vector3 getNextRandomPosition() {
		// TODO: compare with other field objects and character, and preserve minimum distance
		float xPos = Random.Range (-xPosInterval, xPosInterval);
		float yPos = 0;
		float zPos = Random.Range (-zPosInterval, zPosInterval);
		return new Vector3(xPos, yPos, zPos);
	}

	// Update is called once per frame
	void Update () {

		if (GameObject.FindGameObjectsWithTag ("Obstacle_middle").Length < max_obstacles_middle) {
			instantiateFieldObject(obstacles_middle);

		}

		if (GameObject.FindGameObjectsWithTag ("Obstacle_big").Length < max_obstacles_big) {
			instantiateFieldObject(obstacles_big);

		}

		if (GameObject.FindGameObjectsWithTag ("Obstacle_small").Length < max_obstacles_small) {
			instantiateFieldObject(obstacles_small);

		}

		if (GameObject.FindGameObjectsWithTag ("Part").Length < max_parts) {
			instantiateFieldObject(parts);

		}

		if (GameObject.FindGameObjectsWithTag ("Energy").Length < max_energies) {
			instantiateFieldObject(energies);

		}

	}
}
