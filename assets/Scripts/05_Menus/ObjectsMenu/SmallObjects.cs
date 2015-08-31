using UnityEngine;
using System.Collections;

public class SmallObjects : MonoBehaviour {
  public Mesh activeMesh;
  public Material activeMaterial;
  private UIObjects obj;
  private Quaternion originalRotation;

  void OnEnable() {
    obj = transform.parent.Find("SelectionBox").GetComponent<UIObjects>();
    originalRotation = transform.localRotation;
    checkBought();
  }

  void Update () {
    if (obj.isActive()) {
      transform.Rotate(-Vector3.forward * Time.deltaTime * 150, Space.World);
      transform.Find("Effect").gameObject.SetActive(true);
    } else {
      transform.localRotation = originalRotation;
      transform.Find("Effect").gameObject.SetActive(false);
    }
	}

  public void checkBought() {
    if ((bool)GameController.control.objects[transform.parent.name]) {
      GetComponent<MeshFilter>().sharedMesh = activeMesh;
      GetComponent<Renderer>().sharedMaterial = activeMaterial;
    }
  }
}
