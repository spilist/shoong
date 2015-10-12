using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	public PhaseManager phaseManager;
	public int[] limitPerLevel;
	private int timeLimit;
	public Text timeLimitText;
	private bool timeMonsterSpawned = false;

	public NormalPartsManager npm;
	public AsteroidManager asm;
	public SmallAsteroidManager sam;
	public DangerousEMPManager dem;
	public TimeMonsterManager tmm;

	public static ElapsedTime time;

	public int addObstaclePer = 40;
	public int addSmallObstaclePer = 30;
	public int removePartPer = 10;
	public int now = 0;

	private int prevObstacleCounter = 0;
	private int prevSmallObstacleCounter = 0;
	private int prevPartCounter = 0;
	private bool spawnDangerousEMP = false;
	private bool isCountingLimit = false;

	void Awake() {
		time = this;
	}

	public void startTime() {
		resetCount();
		StartCoroutine("startElapse");
	}

	public void resetCount() {
		timeMonsterSpawned = false;
		isCountingLimit = true;
		timeLimit = limitPerLevel[phaseManager.phase()];
		timeLimitText.text = timeLimit.ToString();
	}

	public void stopCountLimit(bool val) {
		isCountingLimit = val;
	}

	IEnumerator startElapse() {
		while(true) {
			yield return new WaitForSeconds(1);
			now++;
			if (isCountingLimit) {
	 			if (timeLimit > 0) {
					timeLimit--;
					timeLimitText.text = timeLimit.ToString();
				} else if (!timeMonsterSpawned) {
					timeMonsterSpawned = true;
					tmm.run();
				}
			}

			// if (prevObstacleCounter < Mathf.Floor(now/addObstaclePer)) {
			// 	prevObstacleCounter++;
			// 	asm.max_obstacles++;
			// }

			// if (prevSmallObstacleCounter < Mathf.Floor(now/addSmallObstaclePer)) {
			// 	prevSmallObstacleCounter++;
			// 	sam.max_obstacles++;
			// }

			// if (prevPartCounter < Mathf.Floor(now/removePartPer)) {
			// 	prevPartCounter++;
			// 	npm.max_parts--;
			// }

			asm.respawn();
			sam.respawn();
			npm.respawn();
			if (spawnDangerousEMP) dem.respawn();
		}
	}

	public void stopTime() {
		StopCoroutine("startElapse");
	}

	void OnDestroy() {
		time = null;
	}

	public void startSpawnDangerousEMP() {
		spawnDangerousEMP = true;
	}
}
