using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

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

  private bool isSaved = false;
  private int boosterCount;

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
    if (gameOverStatus == 1) {
      if (Input.GetMouseButtonDown(0)) scoreUpdate();
    } else if (gameOverStatus > 1) {
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

  public void gameOver(string reason) {
    TimeManager.time.stopTime();
    SkillManager.sm.stopSkills();
    Player.pl.stopOtherEffects();
    RhythmManager.rm.stopBeat();

    if (reason == "NoEnergy") {
      DataManager.dm.increment("DeathByLowEnergy");
    } else if (reason == "Obstacle_small") {
      DataManager.dm.increment("DeathBySmallAsteroid");
    } else if (reason == "Obstacle_big") {
      DataManager.dm.increment("DeathByAsteroid");
    } else if (reason == "Obstacle") {
      DataManager.dm.increment("DeathByMeteroid");
    } else if (reason == "Blackhole") {
      DataManager.dm.increment("DeathByBlackhole");
    } else if (reason == "Monster") {
      DataManager.dm.increment("DeathByMonster");
    } else if (reason == "Dopple") {
      DataManager.dm.increment("DeathByDopple");
    } else if (reason == "Trap") {
      DataManager.dm.increment("DeathByTrap");
    } else if (reason == "TimeMonster") {
      DataManager.dm.increment("DeathByTimeMonster");
    }

    if (reason == "NoEnergy") {
      playerExplosion.Play ();
      playerExplosion.GetComponent<AudioSource>().Play();
    } else if (reason == "Blackhole") {
      playerExplosion.GetComponent<AudioSource>().Play();
    } else {
      showCharacterDebris();
    }

    StartCoroutine("startGameOver");
  }

  IEnumerator startGameOver() {
    gameOverStatus++;

    // inputHandler.stopReact();
    inputHandler.SetActive(false);

    AudioManager.am.changeVolume("Main", "Small");

    // playerExplosion.Play ();
    Player.pl.GetComponent<Rigidbody>().isKinematic = true;
    Player.pl.GetComponent<MeshRenderer>().enabled = false;
    Player.pl.GetComponent<SphereCollider>().enabled = false;
    boosterCount = Player.pl.getNumBoosters();
    contactCollider.SetActive(false);

    yield return new WaitForSeconds(showPlayerExplosionDuring);

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

    int daysPassed = (int)(DateTime.Now.Date - DataManager.dm.getDateTime("FirstPlayDate").Date).TotalDays;
    if (0 < daysPassed && daysPassed <= 7) {
      if (DataManager.dm.getInt("NumPlay_" + daysPassed) == 0) {
        DataManager.dm.setInt("NumPlay_" + daysPassed, DataManager.dm.getInt("TotalNumPlays") - DataManager.dm.getInt("NumPlay_" + (daysPassed - 1)));
      }
    }

    isSaved = true;
    int count = CubeManager.cm.getCount();
    DataManager.dm.increment("CurrentCubes", count + int.Parse(bonusCount.text));
    DataManager.dm.increment("TotalCubes", count + int.Parse(bonusCount.text));
    DataManager.dm.setBestInt("BestCubes", count);

    // DataManager.dm.increment("CurrentGoldenCubes", QuestManager.qm.questReward);
    // DataManager.dm.increment("TotalGoldenCubes", QuestManager.qm.questReward);

    int time = TimeManager.time.now;
    DataManager.dm.increment("TotalTime", time);
    DataManager.dm.setBestInt("BestTime", time);

    DataManager.dm.increment("TotalNumPlays");
    DataManager.dm.setAverage("QuestCompleteRate", "TotalQuestCompletes");
    DataManager.dm.setAverage("AverageTime", "TotalTime");
    DataManager.dm.setAverage("AverageCubes", "TotalCubes");
    DataManager.dm.setAverage("AverageGoldenCubes", "TotalGoldenCubes");
    DataManager.dm.setAverage("AverageBoosters", "TotalBoosters");
    DataManager.dm.setAverage("AverageSuperheats", "TotalSuperheats");
    DataManager.dm.setAverage("AverageNumDestroyObstacles", "TotalNumDestroyObstacles");
    DataManager.dm.setAverage("AverageNumUseObjects", "TotalNumUseObjects");
    DataManager.dm.setFloat("AverageCPS", 100 * DataManager.dm.getInt("TotalCubes") / (float) DataManager.dm.getInt("TotalTime"));

    int numPlay = DataManager.dm.getInt("TotalNumPlays");
    if (numPlay <= 10) {
      DataManager.dm.setInt("PlayTime_" + numPlay, time);
      DataManager.dm.setInt("CubesGet_" + numPlay, count);
      DataManager.dm.setInt("NumBooster_" + numPlay, boosterCount);
    }
  }

  void OnDisable() {
    if (menus.gameStarted()) {
      save();
    }
    DataManager.dm.save();
  }
}
