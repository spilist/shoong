using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class SphereInputHandler : MonoBehaviour
{
	public GameObject sphere;
	SphereMover mover;

	private bool react = false;

	void Start() {
		mover = sphere.GetComponent<SphereMover>();
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			mover.rotateSphereBody();

			if (react) {
				Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
				Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
				Vector3 heading = worldTouchPosition - sphere.transform.position;
				Vector3 direction = heading / heading.magnitude;
				mover.setDirection(direction);
				mover.energyBar.loseByShoot();
				mover.boosterPs.Play();
				mover.boosterPs.GetComponent<AudioSource>().Play();
			}
		}
	}

	public void reactToInput() {
		react = true;
	}

	public void stopReact() {
		react = false;
	}
}

