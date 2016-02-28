using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {
  private float addGuageAmount = 0;
  private float currentProgressCount = 0;
  private int requiredProgress;

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
	private bool progressChanging = false;

  public RectTransform progressCharacter;
  private float progressCharacterPos;
  public float progressCharacterPosStart = -95;
  public float progressCharacterPosEnd = 95;

	public float progressPerSecond = 10;
	public float progressPerCube = 0.5f;
	public float progressChangeSpeed = 5f;

	public NormalPartsManager npm;
	public AsteroidManager asm;
	public SmallAsteroidManager sam;
	public DangerousEMPManager dem;
	public BlackholeManager blm;
  public MeteroidManager mtm;
  public IceDebrisManager idm;
  public RubberBallManager rbm;
  public RubberBallBiggerManager rbbm;

	public static TimeManager time;

	public int now = 0;

	private bool spawnAsteroid = false;
  private bool spawnDangerousEMP = false;
	private bool spawnBlackhole = false;
  private bool spawnRubberBall = false;
  private bool spawnRubberBallBigger = false;

	void Awake() {
		time = this;
	}

	public void startTime() {
		resetProgress();
    CubeManager.cm.startCount();

		StartCoroutine("startElapse");
	}

  public void resetProgressCharacter() {
    progressCharacterPos = progressCharacterPosStart;
    progressCharacter.anchoredPosition = new Vector2(progressCharacterPosStart, progressCharacter.anchoredPosition.y);
  }

	public void resetProgress() {
    if (!PhaseManager.pm.nextPhase()) return;
		progressChanging = true;

		addGuageAmount = 0;
    currentProgressCount = 0;

		requiredProgress = PhaseManager.pm.reqProgressPerLevel[PhaseManager.pm.phase()];

		// GameObject obj = (GameObject) Instantiate(phaseStarPrefab);
		// obj.transform.SetParent(phaseStars, false);
		// obj.GetComponent<RectTransform>().anchoredPosition += new Vector2(PhaseManager.pm.phase() * distanceBetweenStars, 0);
		// phaseStar = obj.GetComponent<Image>();
		// popStatus = 1;
		// starScale = startScale;
	}

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + Player.pl.transform.position.x, Player.pl.transform.position.y, screenPos.y + Player.pl.transform.position.z);
  }

	public void addProgressByCube(int cubes) {
		addProgress(cubes * progressPerCube);
	}

	void addProgress(float val) {
		addGuageAmount += val;
	}

	void Update() {
		if (progressChanging) {
			if (currentProgressCount <= requiredProgress) {
				currentProgressCount = Mathf.MoveTowards(currentProgressCount, requiredProgress, Time.deltaTime * progressPerSecond);
				currentProgressCount = Mathf.MoveTowards(currentProgressCount, currentProgressCount + addGuageAmount, Time.deltaTime * requiredProgress / progressChangeSpeed);
				addGuageAmount = Mathf.MoveTowards(addGuageAmount, 0, Time.deltaTime * requiredProgress / progressChangeSpeed);

        progressCharacterPos = (currentProgressCount / requiredProgress) * (progressCharacterPosEnd - progressCharacterPosStart) + progressCharacterPosStart;
        progressCharacter.anchoredPosition = new Vector2(progressCharacterPos, progressCharacter.anchoredPosition.y);
			}

			if (currentProgressCount >= requiredProgress) {
				resetProgress();
			}
		}

		// if (popStatus > 0) {
  //     if (popStatus == 1) {
  //       changeScale(largeScale, largeScale - startScale);
  //     } else if (popStatus == 2) {
  //       changeScale(smallScale, smallScale - largeScale);
  //     } else if (popStatus == 3) {
  //       changeScale(1, 1 - smallScale);
  //     } else if (popStatus == 4) {
  //     	popStatus = 0;
  //     }

  //     phaseStar.transform.localScale = starScale * Vector3.one;
  //   }
	}

	IEnumerator startElapse() {
		while(true) {
			yield return new WaitForSeconds(1);
			now++;

      CubeManager.cm.addPointsByTime();

      sam.respawn();
      npm.respawn();

      if (spawnAsteroid) asm.respawn();
      if (spawnDangerousEMP) dem.respawn();
			if (spawnBlackhole) blm.respawn();
      if (spawnRubberBall) rbm.respawn();
      if (spawnRubberBallBigger) rbbm.respawn();
		}
	}

	public void stopTime() {
    progressChanging = false;
		StopCoroutine("startElapse");
    mtm.stopSpawn();
    idm.stopSpawn();
	}

  public void startSpawnAsteroid() {
    spawnAsteroid = true;
  }

	public void startSpawnDangerousEMP() {
		spawnDangerousEMP = true;
	}

	public void startBlackhole() {
		spawnBlackhole = true;
	}

  public void startRubberBall(bool val = true) {
    spawnRubberBall = val;
  }

  public void startRubberBallBigger(bool val = true) {
    spawnRubberBallBigger = val;
  }

	// void changeScale(float targetScale, float difference) {
 //    starScale = Mathf.MoveTowards(starScale, targetScale, Time.deltaTime * Mathf.Abs(difference) / changeTime);
 //    if (starScale == targetScale) popStatus++;
 //  }
}
