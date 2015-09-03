using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
  public GameObject player;
  public GameObject barsCanvas;
  public ParticleSystem playerExplosion;
  public ElapsedTime elapsedTime;
  public TouchInputHandler inputHandler;
  public MenusController menus;
  public ScoreManager scoreManager;
  public GameObject pauseButton;

  public Renderer partsCollector;
  public GameObject unstoppableSphere;

  private bool gameOver = false;

  void Update() {
    if (gameOver) {
      if (Input.GetMouseButtonDown(0) && menus.touched() == "Ground") {
        Application.LoadLevel(Application.loadedLevel);
      }
    }
  }

  public void run() {
    gameOver = true;
    menus.gameEnd();
    GetComponent<Canvas>().enabled = true;

    inputHandler.stopReact();
    elapsedTime.stopTime();

    playerExplosion.Play ();
    playerExplosion.GetComponent<AudioSource>().Play();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;
    partsCollector.enabled = false;
    foreach (Transform tr in partsCollector.transform) {
      tr.gameObject.SetActive(false);
    }
    unstoppableSphere.SetActive(false);
    barsCanvas.SetActive(false);
    pauseButton.SetActive(false);
    scoreManager.run();
  }

  public bool isOver() {
    return gameOver;
  }
}
