using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	public PhaseManager phaseManager;
	public RectTransform progressCharacter;
	public int[] reqProgressPerPhase;
	public int progressPerSecond = 10;
	public float progressPerCube = 0.5f;
	public float progressChangeSpeed = 5f;
	public int progressStart = 30;
	public int progressEnd = 390;
	private bool progressChanging = false;
	private float guageChangeAmount = 0;
	private float currentProgress;
	private float progressScale;
	private int distance;
	private int reqProgress;

	public NormalPartsManager npm;
	public AsteroidManager asm;
	public SmallAsteroidManager sam;
	public DangerousEMPManager dem;
	public BlackholeManager blm;

	public static ElapsedTime time;

	public int now = 0;

	private bool spawnDangerousEMP = false;
	private bool spawnBlackhole = false;

	void Awake() {
		time = this;
		distance = progressEnd - progressStart;
	}

	public void startTime() {
		progressCharacter.GetComponent<MeshFilter>().sharedMesh = GameObject.Find("Player").GetComponent<MeshFilter>().sharedMesh;
		resetProgress();
		StartCoroutine("startElapse");
	}

	public void resetProgress() {
		progressChanging = true;
		if (phaseManager.phase() >= reqProgressPerPhase.Length) {
			progressChanging = false;
			progressCharacter.transform.parent.gameObject.SetActive(false);
		} else {
			reqProgress = reqProgressPerPhase[phaseManager.phase()];
			progressScale = (float)distance / reqProgress;
			progressCharacter.anchoredPosition = new Vector2(progressStart, 0);
			currentProgress = progressStart;
		}
	}

	public void addProgressByCube(int cubes) {
		addProgress(cubes * progressPerCube);
	}

	void addProgress(float val) {
		guageChangeAmount += val * progressScale;
	}

	public void startProgress(bool val) {
		progressChanging = val;
	}

	void Update() {
		if (progressChanging && currentProgress <= progressEnd) {
			currentProgress = Mathf.MoveTowards(currentProgress, progressEnd, Time.deltaTime * distance * progressPerSecond / reqProgress);
			currentProgress = Mathf.MoveTowards(currentProgress, currentProgress + guageChangeAmount, Time.deltaTime * distance / progressChangeSpeed);
			guageChangeAmount = Mathf.MoveTowards(guageChangeAmount, 0, Time.deltaTime * distance / progressChangeSpeed);
			progressCharacter.anchoredPosition = new Vector2(currentProgress, 0);
		}

		if (currentProgress >= progressEnd) {
			progressChanging = false;
			phaseManager.nextPhase();
			resetProgress();
		}
	}

	IEnumerator startElapse() {
		while(true) {
			yield return new WaitForSeconds(1);
			now++;

			asm.respawn();
			sam.respawn();
			npm.respawn();
			if (spawnDangerousEMP) dem.respawn();
			if (spawnBlackhole) blm.respawn();
		}
	}

	public void stopTime() {
		StopCoroutine("startElapse");
	}

	public void startSpawnDangerousEMP() {
		spawnDangerousEMP = true;
	}

	public void startBlackhole() {
		spawnBlackhole = true;
	}
}
