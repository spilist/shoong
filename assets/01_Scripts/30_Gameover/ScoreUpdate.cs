using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {
  public BackButton back;

  public Text totalCubes;
  public Text cubes;
  public Text goldenCubes;
  public GameObject cubesRecords;
  public Text cubesHighscoreDescription;
  public Text cubesHighscoreNumber;
  public Text cubesCurrentScore;
  public GameObject elapsedTime;
  public GameObject CPS;
  public GameObject phaseBonus;
  public GameObject collectorBonus;

  public GoldCubesCount goldenCubesCount;
  public Color newHighscoreColor;
  public int scoreUpdateMaxStandard = 1000;
  public float scoreUpdateMaxDuration = 3;
  public float scoreUpdateMinDuration = 0.2f;
  public float stayDuration = 0.5f;
  private float stayCount = 0;
  public float moveDuration = 0.2f;

  private int totalChangeTo;
  private int cubeChangeTo;
  private int cubeDifference;
  private float totalNum;
  private float cubeNum;
  private float cubeCurrentNum;
  private float goldenCubeNum;
  private float highscoreNum;
  private float duration;
  private int bonusAmount;

  private int updateStatus = 0;
  private bool newHighscore = false;
  private float positionX;
  private float distance;

  void Start() {
    cubeDifference = CubeManager.cm.getCount();

    totalNum = DataManager.dm.getInt("TotalCubes");
    totalCubes.text = totalNum.ToString();
    totalChangeTo = (int) totalNum + cubeDifference;

    cubeNum = DataManager.dm.getInt("CurrentCubes");
    cubes.text = cubeNum.ToString();
    cubeChangeTo = (int) cubeNum + cubeDifference;

    goldenCubeNum = (int) goldenCubesCount.getCount();
    goldenCubes.text = goldenCubeNum.ToString();

    if (cubeDifference >= scoreUpdateMaxStandard) {
      duration = scoreUpdateMaxDuration;
    } else {
      duration = Mathf.Max(scoreUpdateMaxDuration * (float) cubeDifference / scoreUpdateMaxStandard, scoreUpdateMinDuration);
    }

    highscoreNum = DataManager.dm.getInt("BestCubes");
    cubesHighscoreNumber.text = highscoreNum.ToString();

    cubeCurrentNum = 0;
    cubesCurrentScore.text = "0";

    elapsedTime.transform.Find("Number").GetComponent<Text>().text = TimeManager.time.now.ToString();
    float cps_ = ((float) cubeDifference / TimeManager.time.now);
    CPS.transform.Find("Number").GetComponent<Text>().text = cps_.ToString("0.00");
    DataManager.dm.setBestFloat("BestCPS", cps_);

    phaseBonus.transform.Find("Number").GetComponent<Text>().text = PhaseManager.pm.getPhaseBonus().ToString();

    float bonusScale = ((DataManager.dm.getInt("NormalCollectorLevel") - 1) * 5 + DataManager.dm.getInt("GoldenCollectorLevel") * 50) / 100f;
    bonusAmount = (int) Mathf.Floor(cubeDifference * bonusScale);

    collectorBonus.transform.Find("Number").GetComponent<Text>().text = bonusAmount.ToString();
    updateStatus++;
    GetComponent<AudioSource>().Play();
  }

  void Update() {
    if (updateStatus == 1) {
      totalNum = Mathf.MoveTowards(totalNum, totalChangeTo, Time.deltaTime * cubeDifference / duration);
      totalCubes.text = totalNum.ToString("0");

      cubeNum = Mathf.MoveTowards(cubeNum, cubeChangeTo, Time.deltaTime * cubeDifference / duration);
      cubes.text = cubeNum.ToString("0");

      cubeCurrentNum = Mathf.MoveTowards(cubeCurrentNum, cubeDifference, Time.deltaTime * cubeDifference / duration);
      cubesCurrentScore.text = cubeCurrentNum.ToString("0");

      if (highscoreNum < cubeCurrentNum) {
        newHighscore = true;
        cubesHighscoreDescription.color = newHighscoreColor;
        cubesHighscoreNumber.color = newHighscoreColor;
        cubesHighscoreNumber.text = cubeCurrentNum.ToString("0");
      }

      if (totalNum == totalChangeTo) {
        updateStatus++;
        GetComponent<AudioSource>().Stop();

        if (newHighscore) cubesHighscoreDescription.GetComponent<AudioSource>().Play();
        positionX = cubesRecords.GetComponent<RectTransform>().anchoredPosition.x;
        distance = positionX;
      }
    } else if (updateStatus == 2) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        updateStatus++;
      }
    } else if (updateStatus == 3) {
      move(cubesRecords, elapsedTime);
    } else if (updateStatus == 4) {
      move(elapsedTime, CPS);
    } else if (updateStatus == 5) {
      move(CPS, phaseBonus);
    } else if (updateStatus == 6) {
      move(phaseBonus, collectorBonus);
    } else if (updateStatus == 7) {
      move(collectorBonus);
    } else if (updateStatus == 8) {
      totalNum = Mathf.MoveTowards(totalNum, totalChangeTo, Time.deltaTime * bonusAmount / duration);
      totalCubes.text = totalNum.ToString("0");

      cubeNum = Mathf.MoveTowards(cubeNum, cubeChangeTo, Time.deltaTime * bonusAmount / duration);
      cubes.text = cubeNum.ToString("0");
      if (totalNum == totalChangeTo) {
        updateStatus++;
        GetComponent<AudioSource>().Stop();
      }
    } else if (updateStatus == 9) {
      ScoreManager.sm.showBanner();
      updateStatus++;
    }
  }

  void move(GameObject target, GameObject nextTarget = null) {
    positionX = Mathf.MoveTowards(positionX, 0, Time.deltaTime / moveDuration * distance);
    target.GetComponent<RectTransform>().anchoredPosition = new Vector2(positionX, target.GetComponent<RectTransform>().anchoredPosition.y);
    if (positionX == 0) {
      updateStatus++;
      if (nextTarget != null) {
        positionX = nextTarget.GetComponent<RectTransform>().anchoredPosition.x;
        distance = positionX;
      } else {
        totalChangeTo += bonusAmount;
        cubeChangeTo += bonusAmount;
        GetComponent<AudioSource>().Play();
      }
    }
  }

}
