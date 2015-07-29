using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class TouchInputHandler : MonoBehaviour
{
	public PlayerMover player;
	public GameObject barsCanvas;
	public EnergyBar energyBar;
	public ComboBar comboBar;
	public ElapsedTime elapsedTime;
	public GameObject idleUI;

	public GameObject objectsManager;
	private FieldObjectsManager fom;
	private SpecialObjectsManager som;
	private PatternPartsManager ppm;

	public SpecialPartIndicator spIndicator;
	public GameObject partsCollector;

	private bool gameStarted = false;
	private bool react = true;
	private Vector3 direction;

	void Start() {
		fom = objectsManager.GetComponent<FieldObjectsManager>();
		som = objectsManager.GetComponent<SpecialObjectsManager>();
		ppm = objectsManager.GetComponent<PatternPartsManager>();

		barsCanvas.GetComponent<Canvas>().enabled = false;
		// partsCollector.SetActive(false);
	}

	void Update() {
		if (react && Input.GetMouseButtonDown(0)) {
			if (!gameStarted) {
				som.run();
				fom.run();
				ppm.run();
				spIndicator.startIndicate();
				barsCanvas.GetComponent<Canvas>().enabled = true;
				energyBar.startDecrease();
				elapsedTime.startTime();
				idleUI.SetActive(false);
				gameStarted = true;
				partsCollector.SetActive(true);
			}
			player.rotatePlayerBody();
			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			Vector3 heading = worldTouchPosition - player.transform.position;
			direction = heading / heading.magnitude;

			if (!player.isUnstoppable()) {
				energyBar.loseByShoot();
				comboBar.loseByShoot();
			}

			player.booster.Play();
			player.booster.GetComponent<AudioSource>().Play();
			player.setDirection(direction);

			if (player.getNextSpecialTry() != null ) {
				player.getNextSpecialTry().GetComponent<GenerateNextSpecial>().tryGetSpecial();
			}
		}
	}

	public void stopReact() {
		react = false;
	}
}

