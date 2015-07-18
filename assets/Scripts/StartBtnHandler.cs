using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class StartBtnHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	// Variables for this handler
	private TextMesh startButtonText;
	private float clickedTime;
	private bool startButtonClicked;
	private int delay = 3;
	private int elapsedDelay = 0;

	// GameObjects to control when start button clicked
	public SphereMover sphereMover;
	public FieldObjectManager fieldObjectManager;
	public EnergyBar energyBar;
	public EnergyIndicator energyIndicator;
	public TextInput gameScoreObject;
	public SphereInputHandler handler;

	static bool restart = false;
	//public WallCreator wallCreator;

	// Use this for initialization
	void Start () {
		startButtonText = GetComponentsInChildren<TextMesh>()[0];

		if (restart) {
			onStartButtonClicked();
			onGameStart();
		}
		else {
			startButtonClicked = false;
			startButtonText.gameObject.SetActive(false);
		}

	}

	public void setRestart() {
		restart = true;
	}

	// Update is called once per frame
	void Update () {
		if (restart)
			return;

		if (startButtonClicked == false)
			return;

		float elapsedTime = Time.time - clickedTime;
		if (elapsedTime > delay) {
			onGameStart ();
		}
		if (elapsedTime > elapsedDelay) {
			startButtonText.text = (delay - elapsedDelay).ToString();
			elapsedDelay++;
		}
	}

	void onStartButtonClicked() {
		gameObject.GetComponent<Collider>().enabled = false;
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		startButtonText.gameObject.SetActive(true);
		clickedTime = Time.time;
		startButtonClicked = true;
	}

	public void onGameStart() {
		startButtonText.gameObject.SetActive(false);
		startButtonClicked = false;
		gameObject.SetActive(false);
		sphereMover.run();
		fieldObjectManager.run();
		energyBar.startDecrease();
		gameScoreObject.startScoring();
		energyIndicator.startIndicate();
		handler.reactToInput();
		//wallCreator.runWallCreator();
	}

	public void onGameFinished() {
		gameObject.GetComponent<Collider>().enabled = true;
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		startButtonClicked = false;
		gameObject.SetActive(true);
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		// TODO: change button effect?
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		onStartButtonClicked();
	}
}
