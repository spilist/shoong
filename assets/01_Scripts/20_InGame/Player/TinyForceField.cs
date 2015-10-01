using UnityEngine;
using System.Collections;

public class TinyForceField : MonoBehaviour {
  public bool byPlayer = false;
  DoppleManager dpm;

  private float duration;
  private Transform waves;
  private Transform main;
  private GameObject[] wavesObj;
  private bool startEnlarge = false;

  private float enlargeWavesUntil = 2;
  private float wavesScale = 1;
  private float enlargeMainUntil;
  private float enlargeDiff;
  private float mainScale;

	void Start () {
    dpm = GameObject.Find("Field Objects").GetComponent<DoppleManager>();

    transform.localScale = Vector3.one * dpm.getTargetSize(byPlayer);
    duration = dpm.waveAwakeDuration;

    main = transform.Find("Main");

    waves = transform.Find("Waves");
    wavesObj = new GameObject[waves.childCount];
    int count = 0;
    foreach (Transform tr in waves) {
      wavesObj[count++] = tr.gameObject;
    }

    mainScale = main.transform.localScale.x;
    enlargeMainUntil = wavesObj[count - 1].transform.localScale.x /2f * mainScale;
    enlargeDiff = enlargeMainUntil - mainScale;

    StartCoroutine("turnOnInConsquence");
	}

  IEnumerator turnOnInConsquence() {
    startEnlarge = true;
    main.gameObject.SetActive(true);

    for (int i = 0; i <= waves.childCount + 2; i++) {
      if (i < wavesObj.Length) wavesObj[i].SetActive(true);
      if (i > 2) wavesObj[i - 3].SetActive(false);
      yield return new WaitForSeconds(duration);
    }
    Destroy(gameObject);
  }

  void Update() {
    if (startEnlarge) {
      mainScale = Mathf.MoveTowards(mainScale, enlargeMainUntil, Time.deltaTime * enlargeDiff / (duration * (waves.childCount + 2)));
      main.localScale = mainScale * Vector3.one;

      wavesScale = Mathf.MoveTowards(wavesScale, enlargeWavesUntil, Time.deltaTime / (duration * (waves.childCount + 2)));
      waves.localScale = wavesScale * Vector3.one;
    }
  }
}
