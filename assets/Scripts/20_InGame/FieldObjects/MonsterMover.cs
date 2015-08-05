using UnityEngine;
using System.Collections;

public class MonsterMover : MonoBehaviour {
  private float speed;
  private float tumble;
  private Vector3 direction;

  private FieldObjectsManager fom;

	void Start () {
    fom = GameObject.Find("Field Objects").GetComponent<FieldObjectsManager>();
    speed = fom.getSpeed(gameObject.tag);
    tumble = fom.getTumble(gameObject.tag);
    GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * tumble;
	}

	// Update is called once per frame
	void Update () {

	}
}
