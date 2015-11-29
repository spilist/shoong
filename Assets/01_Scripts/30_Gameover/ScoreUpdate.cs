using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {
  public BackButton back;

  public Text currentScoreIngame;

  public GameObject cubesRecords;
  public Text cubesHighscoreDescription;
  public Text cubesHighscoreNumber;
  public Text cubesCurrentScore;
  public GameObject noAutoBonus;

  public Color newHighscoreColor;
  public int scoreUpdateMaxStandard = 1000;
  public float scoreUpdateMaxDuration = 3;
  public float scoreUpdateMinDuration = 0.2f;
  public float stayDuration = 0.5f;
  private float stayCount = 0;
  public float moveDuration = 0.2f;

  private int cubeDifference;
  private float cubeCurrentNum;
  private float highscoreNum;
  private float duration;
  private int bonusAmount;
  public Text bonusScoreText;
  public Text bonusCoinText;
  private float bonusDuration;
  public GameOverGoldCubes ggc;

  private int updateStatus = 0;
  private bool newHighscore = false;
  private bool newHighscoreByBonus = false;
  private float positionX;
  private float distance;
  private int bonusCoin;

  void Start() {
    cubeDifference = CubeManager.cm.getCount();
    bonusAmount = CubeManager.cm.getBonus();
    bonusScoreText.text = "+" + bonusAmount;
    bonusCoin = PhaseManager.pm.phase() + 1;
    bonusCoinText.text = "+" + bonusCoin;

    currentScoreIngame.text = cubeDifference.ToString();

    if (cubeDifference >= scoreUpdateMaxStandard) {
      duration = scoreUpdateMaxDuration;
    } else {
      duration = Mathf.Max(scoreUpdateMaxDuration * (float) cubeDifference / scoreUpdateMaxStandard, scoreUpdateMinDuration);
    }

    bonusDuration = Mathf.Max(2 * scoreUpdateMaxDuration * (float) bonusAmount / scoreUpdateMaxStandard, 2 * scoreUpdateMinDuration);

    highscoreNum = DataManager.dm.getInt("BestCubes");
    cubesHighscoreNumber.text = highscoreNum.ToString();

    cubeCurrentNum = 0;
    cubesCurrentScore.text = "0";

    updateStatus++;
    GetComponent<AudioSource>().Play();
  }

  void Update() {
    if (updateStatus == 1) {
      cubeCurrentNum = Mathf.MoveTowards(cubeCurrentNum, cubeDifference, Time.deltaTime * cubeDifference / duration);
      cubesCurrentScore.text = cubeCurrentNum.ToString("0");

      if (highscoreNum < cubeCurrentNum) {
        newHighscore = true;
        cubesHighscoreDescription.color = newHighscoreColor;
        cubesHighscoreNumber.color = newHighscoreColor;
        cubesHighscoreNumber.text = cubeCurrentNum.ToString("0");
      }

      if (cubeCurrentNum == cubeDifference) {
        updateStatus++;

        if (bonusAmount > 0) {
          noAutoBonus.SetActive(true);
          ggc.change(bonusCoin);
          GetComponent<AudioSource>().Play();
        }
        else {
          GetComponent<AudioSource>().Stop();
        }

        if (newHighscore) {
          cubesHighscoreDescription.GetComponent<AudioSource>().Play();
          highscoreNum = cubeCurrentNum;
        }
        positionX = cubesRecords.GetComponent<RectTransform>().anchoredPosition.x;
        distance = positionX;
      }
    } else if (updateStatus == 2) {
      if (bonusAmount > 0) {
        cubeCurrentNum = Mathf.MoveTowards(cubeCurrentNum, cubeDifference + bonusAmount, Time.deltaTime * bonusAmount / bonusDuration);
        cubesCurrentScore.text = cubeCurrentNum.ToString("0");

        if (highscoreNum < cubeCurrentNum) {
          if (newHighscore) newHighscoreByBonus = true;
          else newHighscore = true;
          cubesHighscoreDescription.color = newHighscoreColor;
          cubesHighscoreNumber.color = newHighscoreColor;
          cubesHighscoreNumber.text = cubeCurrentNum.ToString("0");
        }

        if (cubeCurrentNum == cubeDifference + bonusAmount) {
          updateStatus++;
          GetComponent<AudioSource>().Stop();

          if (newHighscore && !newHighscoreByBonus) cubesHighscoreDescription.GetComponent<AudioSource>().Play();
          positionX = cubesRecords.GetComponent<RectTransform>().anchoredPosition.x;
          distance = positionX;
        }
      } else updateStatus++;
    } else if (updateStatus == 3) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        updateStatus++;
      }
    } else if (updateStatus == 4) {
    //   move(cubesRecords, elapsedTime);
    // } else if (updateStatus == 4) {
    //   move(elapsedTime, CPS);
    // } else if (updateStatus == 5) {
    //   move(CPS, phaseBonus);
    // } else if (updateStatus == 6) {
    //   move(phaseBonus, collectorBonus);
    // } else if (updateStatus == 7) {
    //   move(collectorBonus);
    // } else if (updateStatus == 8) {
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
      }
    }
  }

}
