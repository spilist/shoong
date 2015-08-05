﻿using UnityEngine;
using System.Collections;

public class FieldObjectsManager : MonoBehaviour {
	public GameObject[] parts;
	public GameObject[] obstacles_big;

	public int max_parts = 50;
	public int max_obstacles = 3;

	public float generateSpaceRadius = 5f;
	public float generateOffset = 0.2f;
	public int partsMinDistance = 50;
 	public LayerMask fieldObjectsLayerMask;

	public float speed_obstacles = 15;
	public float speed_parts = 5;
	public float speed_special = 5;
	private Hashtable speed;

	public float tumble_obstacles = 0.5f;
	public float tumble_parts = 1f;
	public float tumble_special = 3f;
	public float tumble_patternparts = 3f;
	private Hashtable tumble;

	public float unstoppableFollowSpeed = 1.1f;

	public Material collectedPartsMaterial;

	void Start () {
	}

	public void run() {
		gameObject.SetActive(true);
		generateObjectsAtStart();
		generateObjectValuesHashtable();
	}

	void generateObjectValuesHashtable() {
		speed = new Hashtable();
		speed.Add("Obstacle_big", speed_obstacles);
		speed.Add("Part", speed_parts);
		speed.Add("SpecialPart", speed_special);

		tumble = new Hashtable();
		tumble.Add("Obstacle_big", tumble_obstacles);
		tumble.Add("Part", tumble_parts);
		tumble.Add("SpecialPart", tumble_special);
		tumble.Add("PatternPart", tumble_patternparts);
	}

	void generateObjectsAtStart() {
		for (int i = 0; i < max_parts; i++) {
			instantiateFieldObject(parts);
		}

		for (int i = 0; i < max_obstacles; i++) {
			instantiateFieldObject(obstacles_big);
		}
	}

	private void instantiateFieldObject(GameObject[] objects) {
		GameObject target = objects[Random.Range(0, objects.Length)];
		spawn(target);
	}

	public GameObject spawn(GameObject target) {
		Vector3 spawnPosition = getSpawnPosition(target.tag);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject newInstance = (GameObject) Instantiate (target, spawnPosition, spawnRotation);
		newInstance.transform.parent = gameObject.transform;
		return newInstance;
	}

	private Vector3 getSpawnPosition(string tag) {
		float screenX, screenY;
		Vector3 spawnPosition;
		int count = 0;
		do {
			do {
				screenX = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
				screenY = Random.Range(-generateSpaceRadius, 1 + generateSpaceRadius);
			} while(-generateOffset < screenX && screenX < generateOffset + 1 && -generateOffset < screenY && screenY < generateOffset + 1);

			spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(screenX, screenY, Camera.main.transform.position.y));
		} while(tag == "Part" && Physics.OverlapSphere(spawnPosition, partsMinDistance, fieldObjectsLayerMask).Length > 0 && count++ < 100);

		if (count >= 100) Debug.Log("A part is overlapped");

		return spawnPosition;
	}

	void Update () {
		if (GameObject.FindGameObjectsWithTag ("Part").Length < max_parts) {
			instantiateFieldObject(parts);
		}

		if (GameObject.FindGameObjectsWithTag ("Obstacle_big").Length < max_obstacles) {
			instantiateFieldObject(obstacles_big);
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
