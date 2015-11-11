using UnityEngine;
using System.Collections;

public class CubeDispenserMover : ObjectsMover {
  private CubeDispenserManager cdm;
  private int comboCount = 0;
  private int brokenCount = 0;
  private bool isGolden = false;
  private ParticleSystem inside;

  protected override void initializeRest() {
    canBeMagnetized = false;
    cdm = (CubeDispenserManager)objectsManager;
  }

  override protected void afterEnable() {
    comboCount = 0;
    brokenCount = 0;
    GetComponent<MeshFilter>().sharedMesh = cdm.originalMesh;
  }

  public void setGolden() {
    isGolden = true;

    inside = transform.Find("GoldenInside").GetComponent<ParticleSystem>();
    inside.gameObject.SetActive(true);

    transform.Find("BasicInside").gameObject.SetActive(false);
    inside.emissionRate = cdm.originalEmissionRate;
  }

  public void setNormal() {
    isGolden = false;

    inside = transform.Find("BasicInside").GetComponent<ParticleSystem>();
    inside.gameObject.SetActive(true);

    transform.Find("GoldenInside").gameObject.SetActive(false);
    inside.emissionRate = cdm.originalEmissionRate;
  }

  public void tryBreak() {
    if (brokenCount == 2) {
      destroyObject(true, true);
      player.goodPartsEncounter(this, cdm.cubesByEncounter * cdm.fullComboCount);
      return;
    }

    Camera.main.GetComponent<CameraMover>().shake();
    cdm.tryBreak();

    GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[brokenCount];
    brokenCount++;
  }

  override protected void afterDestroy(bool byPlayer) {
    if (byPlayer) {
      if (comboCount != cdm.fullComboCount) {
        DataManager.dm.increment("NumDestroyCubeDispenser");
      }

      if (isGolden) {
        GoldManager.gm.add(transform.position, cdm.goldenCubeAmount * cdm.fullComboCount, false);
      }
    }
  }

  override protected void afterCollide(Collision collision) {
    if (collision.collider.tag == "ContactCollider") {
      if (player.isUsingRainbow() || player.isUnstoppable()) {
        player.goodPartsEncounter(this, cdm.cubesByEncounter * cdm.fullComboCount);
      } else {
        player.contactCubeDispenser(transform, cdm.cubesByEncounter, collision, cdm.reboundDuring);
        Camera.main.GetComponent<CameraMover>().shake();

        if (isGolden) {
          GoldManager.gm.add(transform.position, cdm.goldenCubeAmount, false);
        }

        encounterPlayer(false);
      }
    }
  }

  override protected bool beforeEncounter() {
    cdm.checkTrying();

    objectsManager.objEncounterEffectForPlayer.GetComponent<AudioSource>().pitch = cdm.pitchStart + comboCount * cdm.pitchIncrease;

    inside.emissionRate -= cdm.decreaseEmissionAmount;

    comboCount++;
    Camera.main.GetComponent<CameraMover>().shake(cdm.shakeDurationByHit, cdm.shakeAmountByHit);

    if (comboCount == cdm.fullComboCount - 4) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[0];

    if (comboCount == cdm.fullComboCount - 2) GetComponent<MeshFilter>().sharedMesh = cdm.brokenMeshes[1];

    if (comboCount == cdm.fullComboCount) {
      destroyObject(true, true);
      DataManager.dm.increment("NumCompleteCubeDispenser");
      return false;
    }

    return true;
  }

  override public string getManager() {
    return "CubeDispenserManager";
  }

  override public int cubesWhenEncounter() {
    return cdm.cubesByEncounter * restCount();
  }

  override public int cubesWhenDestroy() {
    return cdm.cubesByEncounter * restCount();
  }

  public int restCount() {
    return cdm.fullComboCount - comboCount;
  }
}
