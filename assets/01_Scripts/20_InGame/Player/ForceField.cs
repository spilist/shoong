using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {
  public GameObject energyCube;
  public EMPManager empManager;
  public PlayerMover player;

  public Superheat superheat;
  public GoldCubesCount gcCount;

  // private ProceduralMaterial mat;
  private int rotatingSpeed;
  private bool isSuper;
  private bool isGolden;
  private int count = 0;

  void Start() {
    rotatingSpeed = empManager.fieldRotateSpeed;
  }

  public void setProperty(Material mat, bool isSuper, bool isGolden) {
    GetComponent<Renderer>().sharedMaterial = mat;
    // this.mat = GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
    this.isSuper = isSuper;
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
      GameObject cube = (GameObject) Instantiate(energyCube, other.transform.position, other.transform.rotation);
      cube.GetComponent<Renderer>().material.SetColor("_TintColor", player.goldenCubeParticleColor);
      cube.GetComponent<TrailRenderer>().material.SetColor("_TintColor", player.goldenCubeParticleTrailColor);
      mover.destroyObject(true, true);
    } else {
      player.goodPartsEncounter(mover, mover.cubesWhenDestroy(), 0, false);
      DataManager.dm.increment("NumCubesGetByForcefield", mover.cubesWhenDestroy());
    }

    if (mover.isNegativeObject()) {
      DataManager.dm.increment("NumDestroyObstaclesByForcefield");
    }
  }

  void OnDisable() {
    if (isGolden) gcCount.add(empManager.goldCubesGet * count);
    else if (isSuper) superheat.addGuageWithEffect(empManager.guageAmountSuper * count);

    transform.localScale = Vector3.one;
    transform.Find("Halo").GetComponent<Light>().range = 1;

    // mat.SetProceduralFloat("$randomseed", Random.Range(0, 1000));
    // mat.RebuildTextures();
  }
}
