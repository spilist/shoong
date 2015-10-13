using UnityEngine;
using System.Collections;

public class PartsToBeCollected : MonoBehaviour {
  public Superheat superheat;
  public int rotatingSpeed = -20;
  public Transform normalParts;
  public Material inactiveMat;
  public Material activeMat;
	public int numPartsToCollect = 5;
  public float guagePerCollect = 200;
  public float pitchIncreasePerCollect = 0.05f;

  public AudioSource collectSound;
  public AudioSource collectCompleteSound;

  private Mesh[] partsMeshes;
  private int collectCount = 0;

	void Awake () {
    partsMeshes = new Mesh[normalParts.childCount];
    int count = 0;
    foreach (Transform tr in normalParts) {
      partsMeshes[count++] = tr.GetComponent<MeshFilter>().sharedMesh;
    }
	}

  public void show(bool val) {
    gameObject.SetActive(val);
  }

  public void generateNew() {
    if (!superheat.isOnSuperheat()) gameObject.SetActive(true);

    int count = 0;
    foreach (Transform tr in transform.Find("Parts")) {
      if (count < numPartsToCollect) {
        tr.GetComponent<MeshFilter>().sharedMesh = getRandomMesh();
        tr.GetComponent<Renderer>().sharedMaterial = inactiveMat;
        tr.GetComponent<ParticleSystem>().Stop();
      } else {
        tr.gameObject.SetActive(false);
      }
      count++;
    }

    collectCount = 0;
  }

  Mesh getRandomMesh() {
    return partsMeshes[Random.Range(0, partsMeshes.Length)];
  }

  public void checkCollected(Mesh mesh) {
    if (!gameObject.activeSelf) return;

    foreach (Transform tr in transform.Find("Parts")) {
      if (!tr.gameObject.activeSelf) continue;

      if (tr.GetComponent<Renderer>().sharedMaterial == inactiveMat && tr.GetComponent<MeshFilter>().sharedMesh == mesh) {
        tr.GetComponent<Renderer>().sharedMaterial = activeMat;
        tr.GetComponent<ParticleSystem>().Play();
        addCollect();
        return;
      }
    }
  }

  void addCollect() {
    collectCount++;
    if (collectCount < numPartsToCollect) {
      collectSound.Play();
      collectSound.pitch += pitchIncreasePerCollect;
    } else {
      collectCompleteSound.Play();
      completeCollect();
    }
  }

  void completeCollect() {
    superheat.addGuageWithEffect(guagePerCollect);
    generateNew();
    collectSound.pitch -= pitchIncreasePerCollect * numPartsToCollect;
  }

	void Update () {
    if (gameObject.activeSelf) {
      transform.Rotate(0, 0, Time.deltaTime * rotatingSpeed);
    }
	}
}
