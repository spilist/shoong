using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
  public GameObject restartMessage;
  public GameObject player;
  public GameObject barsCanvas;
  public ParticleSystem playerExplosion;
  public ParticleSystem comboGlow;
  public ElapsedTime elapsedTime;
  public TouchInputHandler inputHandler;

  private bool gameOver = false;

  void Update() {
    if (gameOver) {
      if (Input.GetMouseButtonDown(0)) {
        Application.LoadLevel(Application.loadedLevel);
      }
    }
  }

  public void run() {
    gameOver = true;

    inputHandler.stopReact();
    elapsedTime.stopTime();

    playerExplosion.Play ();
    playerExplosion.GetComponent<AudioSource>().Play();
    comboGlow.Stop();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;
    player.GetComponent<TrailRenderer>().enabled = false;
    barsCanvas.SetActive(false);
    restartMessage.GetComponent<Text>().enabled = true;
  }

  public bool isOver() {
    return gameOver;
  }
}
