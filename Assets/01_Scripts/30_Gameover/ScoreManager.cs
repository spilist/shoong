using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using Heyzap;

public class ScoreManager : MonoBehaviour {
  public static ScoreManager sm;

  public MenusController menus;
  public BeforeIdle beforeIdle;

  // for score managing
  public Text bonusCount;

  // for gameover effect
  public float showPlayerExplosionDuring = 2;
  public ParticleSystem playerExplosion;
  public Transform debrisTransform;
  public GameObject characterDebris;
  public List<GameObject> characterDebrisPool;
  public int debrisTumble = 30;
  public int numDebrisSpawn = 10;
  public float minDebrisSize = 0.5f;
  public float maxDebrisSize = 1f;
  public int minDebrisSpeed = 200;
  public int maxDebrisSpeed = 400;
  public float minSizeAfterBreak = 5;
  public float maxSizeAfterBreak = 10;
  public float destroyLargeAfter = 0.5f;
  public float destroySmallAfter = 4;

  public GameObject contactCollider;
  public GameObject inputHandler;
  private bool isScoring = false;

  // after gameover
  public GameObject energyDangerFilter;
  private int gameOverStatus = 0;
  public GameObject inGameUI;
  public GameObject gameOverUI;
  public Transform bannerButtonsList;
  public GameObject gameOverButtons;

  public GameOverBanner createBanner;
  public CharacterCreateBannerButton createBannerButton;
  public GameOverBanner bottomBanner;

  public float gameOverShakeDuration = 1;
  public float gameOverShakeAmount = 8;
  public string lastGameOverReason = "";

  private bool isSaved = false;

  void Awake() {
    sm = this;
  }

  void Start() {
    characterDebrisPool = new List<GameObject>();
    for (int i = 0; i < numDebrisSpawn; ++i) {
      GameObject obj = (GameObject) Instantiate(characterDebris);
      obj.SetActive(false);
      characterDebrisPool.Add(obj);
    }
  }

  void Update() {
    if (gameOverStatus == 2) {
      if (Input.GetMouseButtonDown(0)) scoreUpdate();
    } else if (gameOverStatus > 2) {
      if (Input.GetMouseButtonDown(0)) {
        if (!beforeIdle.isLoading() && menus.touched() != "GameOverActions") return;
      }
    }
  }

  GameObject getDebris() {
    for (int i = 0; i < characterDebrisPool.Count; i++) {
      if (!characterDebrisPool[i].activeInHierarchy) {
        return characterDebrisPool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(characterDebris);
    characterDebrisPool.Add(obj);
    return obj;
  }
  public Camera mainCam;

  void showCharacterDebris() {
    Camera.main.GetComponent<CameraMover>().shake(gameOverShakeDuration, gameOverShakeAmount);

    for (int howMany = numDebrisSpawn; howMany > 0; howMany--) {
      GameObject debris = getDebris();
      debris.transform.position = Player.pl.transform.position;
      debris.transform.localScale = UnityEngine.Random.Range(minDebrisSize, maxDebrisSize) * Vector3.one;
      debris.GetComponent<MeshFilter>().sharedMesh = debrisTransform.GetChild(UnityEngine.Random.Range(0, debrisTransform.childCount)).GetComponent<MeshFilter>().sharedMesh;
      debris.SetActive(true);
      if (howMany == numDebrisSpawn) debris.GetComponent<AudioSource>().Play();
    }
  }

  IEnumerator rotateCamTest() {
    float time = 0.6f;
    /*
    while (time > 0) {
      //mainCam.transform.LookAt(Player.pl.transform);
      //mainCam.transform.Translate(Vector3.right * Time.deltaTime);
      mainCam.transform.RotateAround(Player.pl.transform.position, new Vector3(1.0f, 0.0f, 0.0f), -10 * Time.deltaTime);
      time -= Time.deltaTime;
      yield return null;
    }
    yield return new WaitForSeconds(1f);
    */
    time = 10f;
    while (time > 0) {
      Player.pl.transform.parent.parent.Translate(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime * 100);
      time -= Time.deltaTime;
      yield return null;
    }
  }

  public void gameOver(string reason) {
    DataManager.dm.setBool("FirstPlayFinished", true);
    lastGameOverReason = reason;
    gameOverStatus++;

    TimeManager.time.stopTime();
    SkillManager.sm.stopSkills();
    Player.pl.stopOtherEffects();
    RhythmManager.rm.stopBeat();

    if (!HZIncentivizedAd.IsAvailable()) {
      HZIncentivizedAd.Fetch();
    }

    if (reason == "Blackhole") {
      playerExplosion.GetComponent<AudioSource>().Play();
    } else {
      showCharacterDebris();
			playerExplosion.Play ();
    }

    StartCoroutine("startGameOver");
  }

  IEnumerator startGameOver() {
    /* Just for test
    mainCam.orthographic = false;
    mainCam.fieldOfView = 25;
    yield return rotateCamTest();
    mainCam.orthographic = true;
    mainCam.ResetProjectionMatrix();
    mainCam.orthographicSize = 110;
    */

    gameOverStatus++;

    inputHandler.GetComponent<TouchInputHandler>().stopReact();
    // inputHandler.SetActive(false);

    AudioManager.am.changeVolume("Main", "Small");


    Player.pl.rb.isKinematic = true;
    Player.pl.GetComponent<MeshRenderer>().enabled = false;
    Player.pl.GetComponent<SphereCollider>().enabled = false;
    Player.pl.transform.Find("Auras").gameObject.SetActive(false);
    DashManager.dm.stopEffects();
    contactCollider.SetActive(false);

    yield return new WaitForSeconds(showPlayerExplosionDuring);

    if (AdsManager.am != null) {
      yield return AdsManager.am.showGameOverAds();
    }

    energyDangerFilter.SetActive(false);

    if (!isScoring) scoreUpdate();
  }

  void scoreUpdate() {
    isScoring = true;
    gameOverUI.SetActive(true);
    inGameUI.SetActive(false);
    gameOverStatus++;
  }

  public void showBanner() {
    if (DataManager.dm.getBool("FirstPlay")) {
      TrackingManager.tm.firstPlayLog("5-1_FirstShowBanner");
    }

    save();

    BannerButton availableBanner = null;
    foreach (Transform tr in bannerButtonsList) {
      BannerButton bannerButton = tr.GetComponent<BannerButton>();
      if (bannerButton.available()) {
        availableBanner = bannerButton;
        break;
      }
    }

    createBannerButton.checkAffordable();
    createBanner.show(createBannerButton);

    if (availableBanner != null) {
      if (availableBanner.firstKey.Equals("GameOver_RewardAds")) {
        TrackingManager.tm.rewardAdButtonShowed();
      }
      bottomBanner.show(availableBanner, createBanner);
    }

    gameOverStatus++;
  }

  public void bannerEnd() {
    gameOverButtons.SetActive(true);
  }

  public void setButtonsAvailable() {
    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GameOverActions")) {
      obj.GetComponent<Collider>().enabled = true;
    }
  }

  public bool isGameOver() {
    return gameOverStatus > 0;
  }

  void save() {
    if (isSaved) return;

    isSaved = true;
    int count = CubeManager.cm.getCount() + CubeManager.cm.getBonus();
    DataManager.dm.increment("TotalCubes", count);
    DataManager.dm.setBestInt("BestCubes", count);

    int time = TimeManager.time.now;
    DataManager.dm.increment("TotalTime", time);
    DataManager.dm.setBestInt("BestTime", time);

    DataManager.dm.increment("TotalNumPlays");
    DataManager.dm.setAverage("AverageTime", "TotalTime");
    DataManager.dm.setAverage("AverageCubes", "TotalCubes");

    DataManager.dm.save();

    TrackingManager.tm.gameDone();

    if (SocialPlatformManager.isAuthenticated() == true) {
      // Report achievements after saving data
      SocialPlatformManager.spm.am.reportAchievements();
      // Report leaderboard after saving data
      SocialPlatformManager.spm.am.reportAllLeaderboard();
    }
  }

  void OnDisable() {
    if (menus.gameStarted()) {
      save();
    }
  }
}
