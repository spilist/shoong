using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ScoreManager : MonoBehaviour {
  public MenusController menus;
  public BeforeIdle beforeIdle;

  // for score managing
  public CubesCount cubesCount;
  public GoldCubesCount goldCubesCount;

  // for gameover effect
  public float showPlayerExplosionDuring = 2;
  public ParticleSystem playerExplosion;
  public GameObject player;
  public GameObject contactCollider;
  public GameObject barsCanvas;
  public Renderer partsCollector;
  public TouchInputHandler inputHandler;
  private bool isScoring = false;

  // after gameover
  private int gameOverStatus = 0;
  public GameObject inGameUI;
  public GameObject gameOverUI;
  public GameObject gameOverBannerPrefab;
  public Transform bannerButtonsList;
  public float[] bannerPos;
  public GameObject gameOverButtons;

  private bool isSaved = false;

  void Update() {
    if (gameOverStatus == 1) {
      if (Input.GetMouseButtonDown(0)) scoreUpdate();
    } else if (gameOverStatus > 1) {
      if (Input.GetMouseButtonDown(0)) {
        if (!beforeIdle.isLoading() && menus.touched() != "GameOverActions") return;
      }
    }
  }

  public void gameOver(string reason) {
    if (reason == "NoEnergy") {
      DataManager.dm.increment("DeathByNoEnergy");
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
    } else if (reason == "Trap") {
      DataManager.dm.increment("DeathByTrap");
    }

    StartCoroutine("startGameOver");
  }

  IEnumerator startGameOver() {
    gameOverStatus++;

    inputHandler.stopReact();
    QuestManager.qm.checkQuestComplete();
    AudioManager.am.changeVolume("Main", "Small");

    playerExplosion.Play ();
    playerExplosion.GetComponent<AudioSource>().Play();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;
    partsCollector.enabled = false;
    foreach (Transform tr in partsCollector.transform) {
      tr.gameObject.SetActive(false);
    }
    player.GetComponent<PlayerMover>().stopStrengthen();
    barsCanvas.SetActive(false);
    contactCollider.SetActive(false);
    QuestManager.qm.hideOnGoingQuests();

    yield return new WaitForSeconds(showPlayerExplosionDuring);
    if (!isScoring) scoreUpdate();
  }

  void scoreUpdate() {
    isScoring = true;
    gameOverUI.SetActive(true);
    inGameUI.SetActive(false);
    gameOverStatus++;
  }

  public void scoringEnd() {
    save();

    BannerButton[] availableBanners = new BannerButton[bannerButtonsList.childCount];
    int availableBannerCount = 0;
    foreach (Transform tr in bannerButtonsList) {
      BannerButton bannerButton = tr.GetComponent<BannerButton>();
      if (bannerButton.available()) {
        availableBanners[availableBannerCount++] = bannerButton;
      }
    }

    int bannerCount = 0;
    GameObject[] banners = new GameObject[2];
    foreach (BannerButton bannerButton in availableBanners){
      if (bannerCount >= 2 || bannerCount >= availableBannerCount) break;

      banners[bannerCount] = (GameObject)Instantiate(gameOverBannerPrefab);
      banners[bannerCount].transform.SetParent(gameOverUI.transform, false);
      banners[bannerCount].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, bannerPos[bannerCount]);

      if (bannerCount == 0) {
        banners[bannerCount].GetComponent<GameOverBanner>().show(bannerButton);
      } else {
        banners[bannerCount].GetComponent<GameOverBanner>().show(bannerButton, banners[0]);
      }
      bannerCount++;
    }

    gameOverStatus++;
    if (availableBannerCount == 0) bannerEnd();
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
    int count = cubesCount.getCount();
    DataManager.dm.increment("CurrentCubes", count);
    DataManager.dm.increment("TotalCubes", count);
    DataManager.dm.setBestInt("BestCubes", count);

    DataManager.dm.increment("CurrentGoldenCubes", QuestManager.qm.questReward);
    DataManager.dm.increment("TotalGoldenCubes", QuestManager.qm.questReward);

    int time = ElapsedTime.time.now;
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
      DataManager.dm.setInt("NumBooster_" + numPlay, player.GetComponent<PlayerMover>().getNumBoosters());
    }
  }

  void OnDisable() {
    if (menus.gameStarted()) {
      save();
    }
    DataManager.dm.save();
  }
}
