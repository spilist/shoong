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
	public GameObject inGameUI;
	public ParticleSystem touchEffect;

	public GameObject objectsManager;
	private FieldObjectsManager fom;
	private ComboPartsManager cpm;
	private MonsterManager monm;
	private BlackholeManager blm;
	private CubeDispenserManager cdm;

	public MenusController menus;

	private bool gameStarted = false;
	private bool react = true;
	private Vector3 direction;

	void Start() {
		fom = objectsManager.GetComponent<FieldObjectsManager>();
		cpm = objectsManager.GetComponent<ComboPartsManager>();
		monm = objectsManager.GetComponent<MonsterManager>();
		blm = objectsManager.GetComponent<BlackholeManager>();
		cdm = objectsManager.GetComponent<CubeDispenserManager>();

		barsCanvas.GetComponent<Canvas>().enabled = false;
	}

	void Update() {
		if (react && Input.GetMouseButtonDown(0) && menus.touched() == "Ground") {
			if (player.isRebounding()) return;

			if (!gameStarted) {
				menus.gameStart();
				cpm.run();
				fom.run();
				monm.run();
				blm.run();
				cdm.run();
				barsCanvas.GetComponent<Canvas>().enabled = true;
				energyBar.startDecrease();
				inGameUI.SetActive(true);
				elapsedTime.startTime();
				idleUI.SetActive(false);
				menus.gameObject.SetActive(false);
				gameStarted = true;
			}

			player.rotatePlayerBody();
			Vector3 touchPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
			Vector3 heading = worldTouchPosition - player.transform.position;
			direction = heading / heading.magnitude;

			if (!player.isUnstoppable()) {
				energyBar.loseByShoot();
				comboBar.loseByShoot();
				player.boosterSpeedup();
			}

			player.booster.Play();
			player.booster.GetComponent<AudioSource>().Play();
			player.setDirection(direction);
			Instantiate(touchEffect, worldTouchPosition, Quaternion.identity);

			cpm.tryToGet();
		}
	}

	public void stopReact() {
		react = false;
	}
}

