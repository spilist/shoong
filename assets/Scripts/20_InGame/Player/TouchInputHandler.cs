using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchInputHandler : MonoBehaviour
{
	public PlayerMover player;
	public FieldObjectsManager fieldObjectsManager;
	public GameObject barsCanvas;
	public EnergyBar energyBar;
	public ElapsedTime elapsedTime;
	public GameObject idleUI;

	private bool gameStarted = false;
	private bool react = true;

	void Start() {
		barsCanvas.GetComponent<Canvas>().enabled = false;
	}

	void Update() {
		if (react && Input.GetMouseButtonDown(0)) {
			if (!gameStarted) {
				fieldObjectsManager.run();
				barsCanvas.GetComponent<Canvas>().enabled = true;
				energyBar.startDecrease();
				elapsedTime.startTime();
				idleUI.SetActive(false);
				gameStarted = true;
			}
			player.rotatePlayerBody();
			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			Vector3 heading = worldTouchPosition - player.transform.position;
			Vector3 direction = heading / heading.magnitude;
			player.energyBar.loseByShoot();
			player.booster.Play();
			player.booster.GetComponent<AudioSource>().Play();
			player.setDirection(direction);
		}
	}

	public void stopReact() {
		react = false;
	}
}

