using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElapsedTime : MonoBehaviour {
	public PhaseManager phaseManager;
	public Transform phaseStars;
	public GameObject phaseStarPrefab;
	public float distanceBetweenStars = 50;
	private Image phaseStar;
	private int popStatus = 0;
	public float startScale = 0.1f;
	public float largeScale = 1.2f;
	public float smallScale = 0.6f;
	public float changeTime = 0.1f;
	private float starScale;

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
		resetProgress();
		StartCoroutine("startElapse");
	}

	public void resetProgress() {
		progressChanging = true;
		if (phaseManager.phase() >= reqProgressPerPhase.Length) {
			progressChanging = false;
		} else {
			reqProgress = reqProgressPerPhase[phaseManager.phase()];
			progressScale = (float)distance / reqProgress;
			currentProgress = progressStart;
		}

		GameObject obj = (GameObject) Instantiate(phaseStarPrefab);
		obj.transform.SetParent(phaseStars, false);
		obj.GetComponent<RectTransform>().anchoredPosition += new Vector2(phaseManager.phase() * distanceBetweenStars, 0);
		phaseStar = obj.GetComponent<Image>();
		popStatus = 1;
		starScale = startScale;
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
			// phaseStar.fillAmount = currentProgress / progressEnd;
		}

		if (currentProgress >= progressEnd) {
			progressChanging = false;
			phaseManager.nextPhase();
			resetProgress();
		}

		if (popStatus > 0) {
      if (popStatus == 1) {
        changeScale(largeScale, largeScale - startScale);
      } else if (popStatus == 2) {
        changeScale(smallScale, smallScale - largeScale);
      } else if (popStatus == 3) {
        changeScale(1, 1 - smallScale);
      } else if (popStatus == 4) {
      	popStatus = 0;
      }

      phaseStar.transform.localScale = starScale * Vector3.one;
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

	void changeScale(float targetScale, float difference) {
    starScale = Mathf.MoveTowards(starScale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
    if (starScale == targetScale) popStatus++;
  }
}
