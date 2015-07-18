using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
  public GameObject restartText;
  public GameObject player;
  public GameObject energyBarCanvas;
  public ParticleSystem playerExplosion;
  public TextInput timeScoring;
  public SphereInputHandler inputHandler;
  public StartBtnHandler startHandler;
	public ParticleSystem over;

  private bool gameOver = false;

  void Update() {
    if (gameOver) {
      if (Input.GetMouseButtonDown(0)) {
        Application.LoadLevel(Application.loadedLevel);
      }
    }
  }

  public void GameOver() {
    restartText.GetComponent<MeshRenderer>().enabled = true;
		over.Play ();

    //Instantiate(playerExplosion, player.transform.position, player.transform.rotation);
    timeScoring.stopScoring();
    player.GetComponent<MeshRenderer>().enabled = false;
    player.GetComponent<SphereCollider>().enabled = false;

	player.GetComponent<TrailRenderer>().enabled = false;
    energyBarCanvas.GetComponent<Canvas>().enabled = false;
    inputHandler.stopReact();
    startHandler.setRestart();
    gameOver = true;
  }
}
