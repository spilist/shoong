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

  private int level;

  void Awake() {
    pm = this;
  }

  void Start() {
    level = 0;
  }

  public int phase() {
    return level;
  }

	public void nextPhase() {
    level++;
    if (level > textPerLevel.Length) {
      level = textPerLevel.Length;
      return;
    }

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
  }
}
