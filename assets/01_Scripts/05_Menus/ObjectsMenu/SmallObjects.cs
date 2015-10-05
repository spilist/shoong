using UnityEngine;
using System.Collections;

public class SmallObjects : MonoBehaviour {
  public bool combined = false;
  public Mesh activeMesh;
  public Material activeMaterial;
  public Material activeMaterial2;
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
      if (combined) {
        foreach (Transform tr in transform) {
          if (tr.tag == "CombinedObject_0") tr.GetComponent<Renderer>().sharedMaterial = activeMaterial;
          else if (tr.tag == "CombinedObject_1") tr.GetComponent<Renderer>().sharedMaterial = activeMaterial2;
        }
      } else {
        GetComponent<MeshFilter>().sharedMesh = activeMesh;
        GetComponent<Renderer>().sharedMaterial = activeMaterial;
      }
    }
  }

  public void changeDetail(GameObject selected) {
    if (combined) {
      foreach (Transform tr in selected.transform) {
        if (tr.tag == "CombinedObject_0") tr.GetComponent<Renderer>().sharedMaterial = activeMaterial;
        else if (tr.tag == "CombinedObject_1") tr.GetComponent<Renderer>().sharedMaterial = activeMaterial2;
      }
    } else {
      selected.GetComponent<MeshFilter>().sharedMesh = activeMesh;
      selected.GetComponent<Renderer>().sharedMaterial = activeMaterial;
    }
  }
}
