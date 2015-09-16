using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	public BasicObjectsManager bom;

	public static ElapsedTime time;
	public int addObstaclePer = 20;
	public int removePartPer = 10;
	public int now = 0;

	private int prevObstacleCounter = 0;
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
				bom.max_obstacles++;
			}

			if (prevPartCounter < Mathf.Floor(now/removePartPer)) {
				prevPartCounter++;
				bom.max_parts--;
			}
		}
	}

	void OnDisable() {
		StopCoroutine("startElapse");
	}

	void OnDestroy() {
		time = null;
	}
}
