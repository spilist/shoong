using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {
  public BackButton back;

  public Text currentScoreIngame;

  public GameObject cubesRecords;
  public GameObject coinRecords;
  public Text cubesHighscoreDescription;
  public Text cubesHighscoreNumber;
  public Text cubesCurrentScore;
  public GameObject noAutoBonus;
  public GameObject randomBonus;

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
  public Transform stageGiftList;

  private int updateStatus = 0;
  private bool newHighscore = false;
  private bool newHighscoreByBonus = false;
  private float positionX;
  private float distance;
  private int bonusCoin_difficulty;
  public int bonusCoin_random = 20;
  private int bonusCoinTotal = 0;

  float currentCoin = 0;
  int coinGetThisGame = 0;
  Text coinText;

  void Start() {
    if (DataManager.dm.isBonusStage) {
      coinRecords.SetActive(true);

      coinGetThisGame = GoldManager.gm.earned();
      coinText = coinRecords.transform.Find("Text").GetComponent<Text>();

      if (coinGetThisGame >= scoreUpdateMaxStandard) {
        duration = scoreUpdateMaxDuration;
      } else {
        duration = Mathf.Max(scoreUpdateMaxDuration * (float) coinGetThisGame / scoreUpdateMaxStandard, scoreUpdateMinDuration);
      }
    } else {
      cubesRecords.SetActive(true);
      cubeDifference = CubeManager.cm.getCount();
      bonusAmount = CubeManager.cm.getBonus();
      bonusScoreText.text = "+" + bonusAmount;
      bonusCoin_difficulty = PhaseManager.pm.phase() + 1;
      bonusCoinText.text = "+" + bonusCoin_difficulty;

      if (CharacterManager.cm.isRandom) {
        bonusCoinTotal = bonusCoin_random;
      }

      if (bonusAmount > 0) {
        bonusCoinTotal += bonusCoin_difficulty;
      }

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
    }

    updateStatus++;
    GetComponent<AudioSource>().Play();
  }

  void Update() {
    if (updateStatus == 1) {
      if (DataManager.dm.isBonusStage) {
        currentCoin = Mathf.MoveTowards(currentCoin, coinGetThisGame, Time.deltaTime * coinGetThisGame / duration);
        coinText.text = currentCoin.ToString("000");

        if (currentCoin == coinGetThisGame) {
          GetComponent<AudioSource>().Stop();
          updateStatus++;
        }
      } else {
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

          if (CharacterManager.cm.isRandom) {
            randomBonus.SetActive(true);
          }

          if (bonusAmount > 0) {
            noAutoBonus.SetActive(true);
          }

          if (CharacterManager.cm.isRandom || bonusAmount > 0) {
            ggc.change(bonusCoinTotal);
            GetComponent<AudioSource>().Play();
          } else {
            GetComponent<AudioSource>().Stop();
          }

          if (newHighscore) {
            cubesHighscoreDescription.GetComponent<AudioSource>().Play();
            highscoreNum = cubeCurrentNum;
          }
          positionX = cubesRecords.GetComponent<RectTransform>().anchoredPosition.x;
          distance = positionX;
        }
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
      if (DataManager.dm.isBonusStage)
        updateStatus++;
      else
        showStageGifts();
    } else if (updateStatus == 5) {
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

  void showStageGifts() {
    stageGiftList.GetChild(PhaseManager.pm.phase() / 3).gameObject.SetActive(true);
  }

  public void increaseStatus() {
    updateStatus++;
  }
}
