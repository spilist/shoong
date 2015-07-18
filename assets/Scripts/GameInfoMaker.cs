using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfoMaker : MonoBehaviour {
	public static GameInfoMaker instance;
	public GameObject wallUnit;
	public GameObject ballVelocityObject;
	public GameObject currWallUnitCountObject;
	public GameObject gameScoreObject;
	public GameObject lifeCountUnit;
	public GameObject lifeCountObject;

	bool isUpdated = false;
	public int ballVelocity = 0;
	private int maxVelocity = 100;
	private int mexVelocityLength = 40;
	private WallGroup velocityGauge;
	public int gameScore = 0;
	public int comboCount = 0;
	public int maxWallUnitCount = 40;
	public int currWallUnitCount = 40;
	private int mexWallUnitCountLength = 40;
	private WallGroup wallCountGauge;

	private int maxLifeLength = 40;
	public int lifeUnitCount = 5;
	private int lifeLength;
	public int maxLifeUnitCount = 5;
	private WallGroup lifeCountGauge;


	// Use this for initialization
	void Start () {
		instance = this;
		velocityGauge = new WallGroup(wallUnit);
		velocityGauge.wallParent = ballVelocityObject;
		ballVelocityObject.SetActive(false);
		wallCountGauge = new WallGroup(wallUnit);
		wallCountGauge.wallParent = currWallUnitCountObject;
		currWallUnitCountObject.SetActive(false);
		lifeCountGauge = new WallGroup(lifeCountUnit);
		lifeCountGauge.wallParent = lifeCountObject;
		updateGameScore();
	}

	// Update is called once per frame
	void Update () {
		if (isUpdated == false)
			return;
		// updateBallVelocity();
		// updateGameScore();
		// updateMaxWallUnitCount();
		// updateCurrWallUnitCount();
		// updateLifeCount();
		// isUpdated = false;
	}
	public static void notifyUpdate() {
		instance.isUpdated = true;
	}

	void updateBallVelocity() {
		velocityGauge.setWallLength((int) (((float)ballVelocity / maxVelocity) * mexVelocityLength));
		MeshRenderer[] renderers = ballVelocityObject.GetComponentsInChildren<MeshRenderer>();
		float r = (float)ballVelocity / maxVelocity;
		float g = 1.0f - r;
		Color color = new Color(r, g, 0, 0.5f);
		foreach (MeshRenderer renderer in renderers) {
			renderer.material.SetColor("_Color", color);
		}
	}
	void updateGameScore() {
		gameScoreObject.GetComponent<TextMesh>().text = gameScore.ToString();
	}
	void updateMaxWallUnitCount() {
	}
	void updateCurrWallUnitCount() {
		wallCountGauge.setWallLength((int) (((float)currWallUnitCount / maxWallUnitCount) * mexWallUnitCountLength));
	}
	void updateLifeCount() {
		lifeLength = lifeUnitCount * 8;
		lifeCountGauge.setWallLength(lifeLength);
	}

	public static void updateComboCount(int count) {
		instance.comboCount += count;
		if (instance.comboCount < 0)
			instance.comboCount = 0;
	}

	public static int updateWallScore(float velocity, int wallLength) {
		int score = instance.comboCount;
		instance.gameScore += score;

		return score;
	}

	public static void updateLife(int val) {
		instance.lifeUnitCount += val;
		if (instance.lifeUnitCount > instance.maxLifeUnitCount) {
			instance.lifeUnitCount = instance.maxLifeUnitCount;
		} else if (instance.lifeUnitCount < 1) {
			instance.lifeUnitCount = 0;
			instance = null;
			Application.LoadLevel(Application.loadedLevel);
		}
	}


	class WallGroup {
		public GameObject wallParent;
		public List<GameObject> walls;
		public GameObject wallUnit;
		public float wallThickness;
		public WallGroup(GameObject wallUnit) {
			wallParent= new GameObject();
			walls = new List<GameObject>();
			this.wallThickness = wallUnit.GetComponent<Renderer>().bounds.extents.z * 2;
			this.wallUnit = wallUnit;
		}
		public float getWallLength() {
			return wallThickness * walls.Count;
		}
		public void setWallLength(float distance) {
			float unitDistance = ((int)(distance / wallThickness)) * wallThickness;
			if (getWallLength() < unitDistance) {
				int wallsToAdd = (int) ((distance - getWallLength()) / wallThickness);
				for (int i = 0;i < wallsToAdd;i++) {
					addWall ();
				}
			} else if (getWallLength() > unitDistance) {
				int wallsToRemove = (int) ((getWallLength() - distance) / wallThickness);
				for (int i = 0;i < wallsToRemove;i++) {
					removeWall ();
				}
			}
		}
		private void addWall() {
			Vector3 nextPos = wallParent.transform.position;
			nextPos += wallParent.transform.forward * wallThickness * walls.Count;
			GameObject wall = (GameObject) Instantiate (wallUnit, nextPos, wallParent.transform.rotation);
			wall.transform.parent = wallParent.transform;
			wall.SetActive(true);
			walls.Add(wall);
		}
		private void removeWall() {
			GameObject wall = walls[walls.Count - 1];
			Destroy (wall);
			walls.Remove(wall);
		}
	}
}
