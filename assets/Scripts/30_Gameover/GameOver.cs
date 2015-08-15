using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
  public GameObject player;
  public GameObject barsCanvas;
  public ParticleSystem playerExplosion;
  public ParticleSystem comboGlow;
  public ElapsedTime elapsedTime;
  public TouchInputHandler inputHandler;
  public MenusController menus;
  public ScoreManager scoreManager;

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
    comboGlow.Stop();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;
    barsCanvas.SetActive(false);


    menus.gameObject.SetActive(true);
    scoreManager.run();
  }

  public bool isOver() {
    return gameOver;
  }
}
