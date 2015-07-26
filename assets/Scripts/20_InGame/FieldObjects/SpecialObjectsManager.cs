using UnityEngine;
using System.Collections;

public class SpecialObjectsManager : MonoBehaviour {
  public GameObject special;
  public GameObject special_next;
  public GameObject special_line;

  public FieldObjectsManager fom;
  public float radius = 20;

  public void run() {
    GameObject specialInstantiated = fom.spawn(special);
      spawnHint(specialInstantiated);
  }

  void spawnHint(GameObject origin) {
    Vector2 randomV = Random.insideUnitCircle;
    randomV.Normalize();

    Vector3 originV = origin.transform.position;

    Vector3 spawnPosition = new Vector3(originV.x + randomV.x * radius, 0, originV.z + randomV.y * radius);

    Quaternion spawnRotation = Quaternion.identity;
    GameObject specialNextInstance = (GameObject) Instantiate (special_next, spawnPosition, spawnRotation);
    specialNextInstance.transform.parent = gameObject.transform;
    specialNextInstance.GetComponent<OffsetFixer>().setParent(origin);

    origin.GetComponent<GenerateNextSpecial>().setNext(specialNextInstance);
    origin.GetComponent<GenerateNextSpecial>().setComboCount(0);

    // Draw line between special and next

    // GameObject specialLineInstance = (GameObject) Instantiate (special_line, originV, spawnRotation);
    // specialLineInstance.transform.parent = gameObject.transform;
    // specialLineInstance.GetComponent<OffsetFixer>().setParent(origin);
    // LineRenderer lineRenderer = specialLineInstance.GetComponent<LineRenderer>();
    // lineRenderer.SetPosition(1, new Vector3(randomV.x * radius, 0, randomV.y * radius));
  }
}
