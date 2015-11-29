using UnityEngine;
using System.Collections;

public class PanelCharacter : MonoBehaviour {
  public Transform player;

  void OnEnable() {
    changeMesh();
  }

  public void changeMesh() {
    GetComponent<MeshFilter>().sharedMesh = player.GetComponent<MeshFilter>().sharedMesh;
  }

  void Update () {
    if (player != null) {
      transform.rotation = player.rotation;
    }
	}
}
