using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {
  public EMPManager empManager;

  private int rotatingSpeed;
  private bool isGolden;
  private int count = 0;

  void Start() {
    rotatingSpeed = empManager.fieldRotateSpeed;
  }

  public void setProperty(Material mat, bool isGolden) {
    GetComponent<Renderer>().sharedMaterial = mat;
    this.isGolden = isGolden;
    count = 0;
  }

  void Update() {
    transform.Rotate(-Vector3.up * Time.deltaTime * rotatingSpeed);
  }

  void OnTriggerEnter(Collider other) {
    count++;

    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    if (mover.tag == "Blackhole") return;

    if (isGolden) {
      GoldManager.gm.add(mover.transform.position, empManager.goldCubesGet);
    }
    Player.pl.goodPartsEncounter(mover, mover.cubesWhenDestroy(), 0, false);
  }

  void OnDisable() {
    transform.localScale = Vector3.one;
    transform.Find("Halo").GetComponent<Light>().range = 1;
  }
}
