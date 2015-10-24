using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {
	// public TimeMonsterManager tmm;
	// public Text timeLimitText;
	// private int timeLimit;
	// public Transform cuberNeedsMore;
	// public PartsCollector cuber;
	// private Text currentProgessText;
 //  private Text requiredProgressText;
 //  public OffscreenObjectIndicator spaceShipIndicator;
 //  public GameObject spaceShipDebris;
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

	public int progressPerSecond = 10;
	public float progressPerCube = 0.5f;
	public float progressChangeSpeed = 5f;

	public NormalPartsManager npm;
	public AsteroidManager asm;
	public SmallAsteroidManager sam;
	public DangerousEMPManager dem;
	public BlackholeManager blm;

	public static TimeManager time;

	public int now = 0;

	private bool spawnDangerousEMP = false;
	private bool spawnBlackhole = false;

	void Awake() {
		time = this;
		// currentEnergyText = cuberNeedsMore.Find("Current").GetComponent<Text>();
  //   requiredProgressText = cuberNeedsMore.Find("Required").GetComponent<Text>();
	}

	public void startTime() {
		resetProgress(false);
		StartCoroutine("startElapse");
	}

	public void resetProgress(bool nextPhase = true) {
		if (nextPhase) {
      // setLimit(false);
      // spaceShipIndicator.stopIndicate();
      // tmm.stopMonster();
      PhaseManager.pm.nextPhase();
      // EnergyManager.em.getFullHealth();
		}
		progressChanging = true;

		// cuberNeedsMore.gameObject.SetActive(true);
		addGuageAmount = 0;
    currentProgressCount = 0;

		requiredProgress = PhaseManager.pm.cuberNeedsPerLevel[PhaseManager.pm.phase()];
    // cuber.setUserFollow(true, requiredProgress);
    // currentEnergyText.text = "0";
    // requiredProgressText.text = "/" + requiredProgress;

		GameObject obj = (GameObject) Instantiate(phaseStarPrefab);
		obj.transform.SetParent(phaseStars, false);
		obj.GetComponent<RectTransform>().anchoredPosition += new Vector2(PhaseManager.pm.phase() * distanceBetweenStars, 0);
		phaseStar = obj.GetComponent<Image>();
		popStatus = 1;
		starScale = startScale;
	}

	// void startFindNextDebris() {
 //    cuberNeedsMore.gameObject.SetActive(false);

 //    Vector2 screenPos = Random.insideUnitCircle;
 //    screenPos.Normalize();
 //    screenPos *= PhaseManager.pm.debrisDistancesPerLevel[PhaseManager.pm.phase()];;

 //    Vector3 spawnPos = screenToWorld(screenPos);
 //    GameObject debris = (GameObject) Instantiate(spaceShipDebris, spawnPos, Quaternion.identity);
 //    spaceShipIndicator.startIndicate(debris);
 //    cuber.transform.position = spawnPos;
 //    cuber.setUserFollow(false);

 //    setLimit(true);
 //  }

  // bool isCuberCharging() {
  //   return cuberNeedsMore.gameObject.activeSelf;
  // }

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
				// currentEnergyText.text = currentProgressCount.ToString("0");
			}

			if (currentProgressCount >= requiredProgress) {
				// startFindNextDebris();
				resetProgress();
			}
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

			// if (hasLimit()) {
			// 	if (timeLimit > 0) {
			// 		timeLimit--;
			// 		timeLimitText.text = timeLimit.ToString();
			// 	} else if (!tmm.isSpawned()) {
			// 		tmm.spawnMonster();
			// 		timeLimitText.color = Color.red;
			// 		timeLimitText.text = "Hurry Up!";
			// 	}
			// }

			asm.respawn();
			sam.respawn();
			npm.respawn();
			if (spawnDangerousEMP) dem.respawn();
			if (spawnBlackhole) blm.respawn();
		}
	}

	// public void setLimit(bool val) {
	// 	timeLimitText.gameObject.SetActive(val);
	// 	if (val) {
	// 		timeLimit = PhaseManager.pm.timeLimitPerLevel[PhaseManager.pm.phase()];
	// 		timeLimitText.text = timeLimit.ToString();
	// 		timeLimitText.color = Color.white;
	// 	}
	// }

	// public bool hasLimit() {
	// 	return timeLimitText.gameObject.activeSelf;
	// }

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
