using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
  public GameObject unstoppableSphere;
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

  public void gameOver() {
    StartCoroutine("startGameOver");
  }

  IEnumerator startGameOver() {
    gameOverStatus++;

    inputHandler.stopReact();
    QuestManager.qm.checkQuestComplete();

    playerExplosion.Play ();
    playerExplosion.GetComponent<AudioSource>().Play();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;
    partsCollector.enabled = false;
    foreach (Transform tr in partsCollector.transform) {
      tr.gameObject.SetActive(false);
    }
    unstoppableSphere.SetActive(false);
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

  public bool isGameOver() {
    return gameOverStatus > 0;
  }

  void save() {
    if (isSaved) return;

    isSaved = true;
    int count = cubesCount.getCount();
    GameController.control.cubes["now"] = (int) GameController.control.cubes["now"] + count;
    GameController.control.cubes["total"] = (int) GameController.control.cubes["total"] + count;
    if (count > (int) GameController.control.cubes["highscore"]) {
      GameController.control.cubes["highscore"] = count;
    }

    GameController.control.times["total"] = (int) GameController.control.times["total"] + ElapsedTime.time.now;

  }

  void OnDisable() {
    if (menus.gameStarted()) {
      GameController.control.save();
    }
  }
}
