using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {
  public GameObject energyCube;
  public EMPManager empManager;

  private ProceduralMaterial mat;
  private int rotatingSpeed;

  void Start() {
    mat = GetComponent<Renderer>().sharedMaterial as ProceduralMaterial;
    rotatingSpeed = empManager.fieldRotateSpeed;
  }

  void Update() {
    transform.Rotate(-Vector3.up * Time.deltaTime * rotatingSpeed);

  }

  void OnTriggerEnter(Collider other) {
    ObjectsMover mover = other.GetComponent<ObjectsMover>();
    for (int e = 0; e < mover.cubesWhenEncounter(); e++) {
      GameObject cube = (GameObject) Instantiate(energyCube, other.transform.position, other.transform.rotation);
      if (e == 0) {
        cube.GetComponent<ParticleMover>().triggerCubesGet(mover.cubesWhenEncounter(), false);
      }
    }

    mover.destroyObject();
  }

  void OnDisable() {
    transform.localScale = Vector3.one;
    transform.Find("Halo").GetComponent<Light>().range = 1;

    mat.SetProceduralFloat("$randomseed", Random.Range(0, 1000));
    mat.RebuildTextures();
  }
}
