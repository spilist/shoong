using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {
  public GameObject energyCube;

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
  }
}
