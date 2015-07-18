using UnityEngine;
using System.Collections;

public class FollowUV : MonoBehaviour {

	public float parralax = 2f;

	void Update () {

		MeshRenderer mr = GetComponent<MeshRenderer>();

		Material mat = mr.material;

		Vector2 offset = mat.mainTextureOffset;

		offset.x = Camera.main.transform.position.x / transform.localScale.x / parralax;
		offset.y = Camera.main.transform.position.z / transform.localScale.y / parralax;

		mat.mainTextureOffset = offset;

	}

}
