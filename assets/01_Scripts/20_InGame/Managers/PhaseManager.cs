using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhaseManager : MonoBehaviour {
  public static PhaseManager pm;

  public string[] textPerLevel;
  public int[] reqProgressPerLevel;

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
    status++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
    }

    string levelName = textPerLevel[level - 1];
    phaseText.text = levelName;
    pos = phaseIndicator.anchoredPosition;
    posX = originalPosX;
    stayCount = 0;

    if (levelName == "IceDebris") {
      icm.enabled = true;
    } else if (levelName == "Minimon") {
      pmm.enabled = true;
    } else if (levelName == "Blackhole") {
      blm.enabled = true;
    } else if (levelName == "Meteroid") {
      ntm.enabled = true;
    } else if (levelName == "Bomb") {
      dem.enabled = true;
    } else if (levelName == "UFO") {
      asm.enabled = true;
    } else if (levelName == "BigBomb") {
      dem.startLarger();
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
