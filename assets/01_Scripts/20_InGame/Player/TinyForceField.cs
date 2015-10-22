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
  private float originalMainScale;

	void Awake () {
    dpm = GameObject.Find("Field Objects").GetComponent<DoppleManager>();

    main = transform.Find("Main");

    waves = transform.Find("Waves");
    wavesObj = new GameObject[waves.childCount];
    int count = 0;
    foreach (Transform tr in waves) {
      wavesObj[count++] = tr.gameObject;
    }

    duration = dpm.waveAwakeDuration;
    originalMainScale = main.transform.localScale.x;
    enlargeMainUntil = wavesObj[waves.childCount - 1].transform.localScale.x /2f * originalMainScale;
    enlargeDiff = enlargeMainUntil - originalMainScale;
  }

  void OnEnable() {
    transform.localScale = Vector3.one * dpm.getTargetSize(byPlayer);
    mainScale = originalMainScale;
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
    gameObject.SetActive(false);
  }

  void Update() {
    if (startEnlarge) {
      mainScale = Mathf.MoveTowards(mainScale, enlargeMainUntil, Time.deltaTime * enlargeDiff / (duration * (waves.childCount + 2)));
      main.localScale = mainScale * Vector3.one;

      wavesScale = Mathf.MoveTowards(wavesScale, enlargeWavesUntil, Time.deltaTime / (duration * (waves.childCount + 2)));
      waves.localScale = wavesScale * Vector3.one;
    }
  }

  void OnDisable() {
    startEnlarge = false;
    main.gameObject.SetActive(false);
    foreach (Transform tr in waves) {
      tr.gameObject.SetActive(false);
    }
  }
}
