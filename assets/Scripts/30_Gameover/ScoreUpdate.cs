using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {
  public ScoreManager scoreManager;
  public ElapsedTime time;

  public Text totalCubes;
  public Text cubes;
  public Text goldenCubes;
  public GameObject cubesRecords;
  public Text cubesHighscoreDescription;
  public Text cubesHighscoreNumber;
  public Text cubesCurrentScore;
  public GameObject elapsedTime;
  public GameObject CPS;
  public GameObject questResult;

  public CubesCount cubesCount;
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
  private int goldenCubeDifference;
  private float totalNum;
  private float cubeNum;
  private float cubeCurrentNum;
  private float goldenCubeNum;
  private float highscoreNum;
  private float duration;

  private int updateStatus = 0;
  private bool newHighscore = false;
  private float positionX;
  private float distance;
  private bool goleCubeSoundPlayed = false;

  void OnEnable() {
    cubeDifference = cubesCount.getCount();

    totalNum = (int) GameController.control.cubes["total"];
    totalCubes.text = totalNum.ToString();
    totalChangeTo = (int) totalNum + cubeDifference;

    cubeNum = (int) GameController.control.cubes["now"];
    cubes.text = cubeNum.ToString();
    cubeChangeTo = (int) cubeNum + cubeDifference;

    goldenCubeNum = (int) goldenCubesCount.getCount();
    goldenCubes.text = goldenCubeNum.ToString();
    goldenCubeDifference = (int) goldenCubeNum + QuestManager.qm.questReward;

    if (cubeDifference >= scoreUpdateMaxStandard) {
      duration = scoreUpdateMaxDuration;
    } else {
      duration = Mathf.Max(scoreUpdateMaxDuration * (float) cubeDifference / scoreUpdateMaxStandard, scoreUpdateMinDuration);
    }

    highscoreNum = (int) GameController.control.cubes["highscore"];
    cubesHighscoreNumber.text = highscoreNum.ToString();

    cubeCurrentNum = 0;
    cubesCurrentScore.text = "0";

    elapsedTime.transform.Find("Number").GetComponent<Text>().text = time.getTime().ToString();
    CPS.transform.Find("Number").GetComponent<Text>().text = ((float) cubeDifference / time.getTime()).ToString("0.00");

    string result = QuestManager.qm.questResult;
    if (result == "FirstQuestComplete") {
      questResult.transform.Find("Description").GetComponent<Text>().text = "일일 퀘스트 보상";
    }

    if (QuestManager.qm.questReward > 0) {
      questResult.transform.Find("Complete").gameObject.SetActive(true);
      questResult.transform.Find("Complete").GetComponent<Text>().text = QuestManager.qm.questReward.ToString();
      questResult.transform.Find("Failed").gameObject.SetActive(false);
    } else {
      questResult.transform.Find("Complete").gameObject.SetActive(false);
      questResult.transform.Find("Failed").gameObject.SetActive(true);
    }

    updateStatus++;
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
      move(CPS, questResult);
    } else if (updateStatus == 6) {
      move(questResult);
      if (!goleCubeSoundPlayed && QuestManager.qm.questReward > 0) {
        goldenCubes.GetComponent<AudioSource>().Play();
        goleCubeSoundPlayed = true;
      }
    } else if (updateStatus == 7) {
      goldenCubeNum = Mathf.MoveTowards(goldenCubeNum, goldenCubeDifference, Time.deltaTime * goldenCubeDifference / 0.5f);
      goldenCubes.text = goldenCubeNum.ToString("0");
      if (goldenCubeNum == goldenCubeDifference) {
        updateStatus++;
        GameController.control.goldenCubes["now"] = (int) goldenCubeNum;
        scoreManager.scoringEnd();
      }
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
      }
    }
  }

}
