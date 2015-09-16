using UnityEngine;
using System.Collections;

public class SmallObjects : MonoBehaviour {
  public Mesh activeMesh;
  public Material activeMaterial;
  private UIObjects obj;
  private Quaternion originalRotation;

  void Start() {
    obj = transform.parent.Find("SelectionBox").GetComponent<UIObjects>();
    originalRotation = transform.localRotation;
    checkBought();
  }

  void Update () {
    if (obj.isActive()) {
      transform.Rotate(-Vector3.forward * Time.deltaTime * 150, Space.World);
      transform.parent.Find("Effect").gameObject.SetActive(true);
    } else {
      transform.localRotation = originalRotation;
      transform.parent.Find("Effect").gameObject.SetActive(false);
    }
	}

  public void checkBought() {
    if (DataManager.dm.getBool(transform.parent.name)) {
      GetComponent<MeshFilter>().sharedMesh = activeMesh;
      GetComponent<Renderer>().sharedMaterial = activeMaterial;
    }
  }
}
