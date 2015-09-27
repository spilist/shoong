using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	public NormalPartsManager npm;
	public AsteroidManager asm;
	public SmallAsteroidManager sam;

	public static ElapsedTime time;

	public int addObstaclePer = 40;
	public int addSmallObstaclePer = 30;
	public int removePartPer = 10;
	public int now = 0;

	private int prevObstacleCounter = 0;
	private int prevSmallObstacleCounter = 0;
	private int prevPartCounter = 0;

	void OnEnable() {
		time = this;
		StartCoroutine("startElapse");
	}

	IEnumerator startElapse() {
		while(true) {
			yield return new WaitForSeconds(1);
			now++;

			if (prevObstacleCounter < Mathf.Floor(now/addObstaclePer)) {
				prevObstacleCounter++;
				asm.max_obstacles++;
			}

			if (prevSmallObstacleCounter < Mathf.Floor(now/addSmallObstaclePer)) {
				prevSmallObstacleCounter++;
				sam.max_obstacles++;
			}

			if (prevPartCounter < Mathf.Floor(now/removePartPer)) {
				prevPartCounter++;
				npm.max_parts--;
			}

			asm.respawn();
			sam.respawn();
			npm.respawn();
		}
	}

	void OnDisable() {
		StopCoroutine("startElapse");
	}

	void OnDestroy() {
		time = null;
	}
}
