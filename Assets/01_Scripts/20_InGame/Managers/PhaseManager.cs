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

  public RectTransform phaseIndicator;
  public Text phaseText;
  public float originalPosX = -220;
  public float showPosX = -480;

  public float showAfter = 1;
  public float changingDuration = 0.5f;
  public float showDuration = 5;

  private int level;
  private int status = 0;
  private float stayCount;
  private Vector2 pos;
  private float posX;
  private float diff;

  void Awake() {
    pm = this;
  }

  void Start() {
    level = 0;
    diff = Mathf.Abs(showPosX - originalPosX);
  }

  public int phase() {
    return level;
  }

	public void nextPhase() {
    level++;
    // status++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
      return;
    }

    string levelName = textPerLevel[level - 1];
    // phaseText.text = levelName;
    // pos = phaseIndicator.anchoredPosition;
    // posX = originalPosX;
    // stayCount = 0;

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
  }

  void Update() {
    if (status == 1) {
      if (stayCount < showAfter) stayCount += Time.deltaTime;
      else {
        stayCount = 0;
        status++;
      }
    } else if (status == 2) {
      posX = Mathf.MoveTowards(posX, showPosX, Time.deltaTime * diff / changingDuration);
      pos.x = posX;
      phaseIndicator.anchoredPosition = pos;
      if (posX == showPosX) status++;
    } else if (status == 3) {
      if (stayCount < showDuration) stayCount += Time.deltaTime;
      else {
        stayCount = 0;
        status++;
      }
    } else if (status == 4) {
      posX = Mathf.MoveTowards(posX, originalPosX, Time.deltaTime * diff / changingDuration);
      pos.x = posX;
      phaseIndicator.anchoredPosition = pos;
      if (posX == originalPosX) {
        status = 0;
      }
    }
  }
}
