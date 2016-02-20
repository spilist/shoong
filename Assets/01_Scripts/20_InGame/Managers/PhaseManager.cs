using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public static PhaseManager pm;

  public string[] textPerLevel;
  public int[] reqProgressPerLevel;

  public Skill_Transform skillTransform;

  public IceDebrisManager icm;
  public PhaseMonsterManager pmm;
  public BlackholeManager blm;
  public MeteroidManager ntm;
  public DangerousEMPManager dem;
  public AlienshipManager asm;

  public ComboPartsManager cpm;
  public CubeDispenserManager cdm;
  public RainbowDonutsManager rdm;
  public SummonPartsManager spm;
  public EMPManager em;
  public AsteroidManager asteroidM;
  public SmallAsteroidManager smallAsteroidM;

  public RectTransform stageIndicatorTop;
  public RectTransform stageIndicatorBottom;

  public int startTopPos = -895;
  public int slowTopPos = 5;
  public int fastTopPos = 45;
  public int endTopPos = 945;

  public int startBottomPos = 945;
  public int slowBottomPos = 45;
  public int fastBottomPos = 5;
  public int endBottomPos = -895;

  public int phaseChangingStatus = 0;
  public float indicatorPauseDuration = 0.5f;
  public float indicatorFastDuration = 0.1f;
  public float indicatorSlowDuration = 0.8f;
  private float indicatorTopXPos;
  private float indicatorBottomXPos;

  public GameObject levelClearEffect;
  public Text currentLevelText;
  public Text nextLevelText;
  private int level;

  void Awake() {
    pm = this;
  }

  void Start() {
    level = -1;
  }

  public int phase() {
    return level;
  }

	public bool nextPhase() {
    level++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
      return false;
    }

    StartCoroutine("showNextLevel");
    if (level == 0) return true;

    string levelName = textPerLevel[level - 1];

    if (levelName == "IceDebris") {
      icm.enabled = true;
      // cdm.enabled = true;
      // skillTransform.addManager("CubeDispenser");
      cpm.adjustForLevel(2);
    } else if (levelName == "Minimon") {
      pmm.enabled = true;
      rdm.enabled = true;
      skillTransform.addManager("RainbowDonuts");
      cpm.adjustForLevel(3);
      // cdm.adjustForLevel(2);
    } else if (levelName == "Blackhole") {
      blm.enabled = true;
      spm.enabled = true;
      skillTransform.addManager("SummonParts");
      // cdm.adjustForLevel(3);
      rdm.adjustForLevel(2);
    } else if (levelName == "Meteroid") {
      ntm.enabled = true;
      spm.adjustForLevel(2);
      rdm.adjustForLevel(3);
      asteroidM.startPhase();
      smallAsteroidM.startPhase();
    } else if (levelName == "Bomb") {
      dem.enabled = true;
      em.enabled = true;
      skillTransform.addManager("EMP");
      spm.adjustForLevel(3);
    } else if (levelName == "UFO") {
      asm.enabled = true;
      em.adjustForLevel(2);
    } else if (levelName == "BigBomb") {
      dem.startLarger();
      em.adjustForLevel(3);
    } else if (levelName == "Minimon2") {
      pmm.nextPhase();
    } else if (levelName == "UFO2") {
      asm.nextPhase();
    }

    return true;
  }

  private IEnumerator showNextLevel() {
    if (level > 0) {
      EnergyManager.em.getFullHealth();
      levelClearEffect.SetActive(true);
    }

    yield return new WaitForSeconds(indicatorPauseDuration);

    phaseChangingStatus = 1;
    indicatorTopXPos = startTopPos;
    indicatorBottomXPos = startBottomPos;

    if (level > 0) {
      currentLevelText.text = levelText(level);
      nextLevelText.text = levelText(level + 1);
      TimeManager.time.resetProgressCharacter();
    }

    if (level == textPerLevel.Length) {
      stageIndicatorTop.GetComponent<Text>().text = "final";
      stageIndicatorBottom.GetComponent<Text>().text = "level";
      nextLevelText.text = "final";
    } else {
      // stageIndicatorTop.GetComponent<Text>().text = "level " + (level + 1).ToString();
      stageIndicatorBottom.GetComponent<Text>().text = levelText(level);
    }
  }

  private string levelText(int level) {
    return (level/3 + 1).ToString() + "-" + (level%3 + 1).ToString();
  }

  void Update() {
    if (phaseChangingStatus > 0) {

      if (phaseChangingStatus == 1) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, slowTopPos, Time.deltaTime * Mathf.Abs(startTopPos - slowTopPos) / indicatorFastDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, slowBottomPos, Time.deltaTime * Mathf.Abs(startBottomPos - slowBottomPos) / indicatorFastDuration);
        if (indicatorTopXPos == slowTopPos) {
          phaseChangingStatus++;
        }
      } else if (phaseChangingStatus == 2) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, fastTopPos, Time.deltaTime * Mathf.Abs(fastTopPos - slowTopPos) / indicatorSlowDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, fastBottomPos, Time.deltaTime * Mathf.Abs(fastBottomPos - slowBottomPos) / indicatorSlowDuration);

        if (indicatorTopXPos == fastTopPos) {
          phaseChangingStatus++;
        }
      } else if (phaseChangingStatus == 3) {
        indicatorTopXPos = Mathf.MoveTowards(indicatorTopXPos, endTopPos, Time.deltaTime * Mathf.Abs(fastTopPos - endTopPos) / indicatorFastDuration);
        indicatorBottomXPos = Mathf.MoveTowards(indicatorBottomXPos, endBottomPos, Time.deltaTime * Mathf.Abs(fastBottomPos - endBottomPos) / indicatorFastDuration);
        if (indicatorTopXPos == endTopPos) phaseChangingStatus = 0;
      }
      stageIndicatorTop.anchoredPosition = new Vector2(indicatorTopXPos, stageIndicatorTop.anchoredPosition.y);
      stageIndicatorBottom.anchoredPosition = new Vector2(indicatorBottomXPos, stageIndicatorBottom.anchoredPosition.y);
    }
  }
}
