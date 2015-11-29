using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {
	public float parallax = 2f;

  private Material mat;

  void Start() {
    mat = GetComponent<MeshRenderer>().material;
  }

	void Update () {
		Vector2 offset = mat.mainTextureOffset;

		offset.x = Camera.main.transform.position.x / transform.localScale.x / parallax;
		offset.y = Camera.main.transform.position.z / transform.localScale.y / parallax;

		mat.mainTextureOffset = offset;

	}

}
