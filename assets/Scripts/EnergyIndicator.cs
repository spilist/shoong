using UnityEngine;
using System.Collections;

public class EnergyIndicator : MonoBehaviour {
  public GameObject player;

  private bool isIndicating = false;
  private GameObject[] energies;
  private GameObject nearest_energy;
  private float near_distance;

  private MeshRenderer indicatorRenderer;
  private Vector3 indicatorPos;

	void Start () {
    indicatorRenderer = gameObject.GetComponent<MeshRenderer>();
    indicatorRenderer.enabled = false;
	}

	void Update () {
    if (isIndicating) {
      showIndicate();
    }
	}

  void showIndicate() {
    energies = GameObject.FindGameObjectsWithTag("Energy");
    nearest_energy = energies[0];
    near_distance = distance(nearest_energy);

    foreach (GameObject energy in energies) {
      float this_distance = distance(energy);
      if (near_distance > this_distance) {
        nearest_energy = energy;
        near_distance = this_distance;
      }
    }

    indicatorPos = Camera.main.WorldToViewportPoint(nearest_energy.transform.position);

    if (indicatorPos.x >= 0.0f && indicatorPos.x <= 1.0f && indicatorPos.y >= 0.0f && indicatorPos.y <= 1.0f) {
      indicatorRenderer.enabled = false;
      return;
    }
    else {
      indicatorRenderer.enabled = true;
      indicatorPos.x -= 0.5f;
      indicatorPos.y -= 0.5f;
      float angle = Mathf.Atan2 (indicatorPos.x, indicatorPos.y);
      transform.localEulerAngles = new Vector3(90, angle * Mathf.Rad2Deg, 0);
      indicatorPos.x = 0.5f * Mathf.Sin (angle) + 0.5f;
      indicatorPos.y = 0.5f * Mathf.Cos (angle) + 0.5f;
      indicatorPos.z = Camera.main.transform.position.y;
      transform.position = Camera.main.ViewportToWorldPoint(indicatorPos);
    }
  }

  public void startIndicate() {
    isIndicating = true;
  }

  private float distance(GameObject obj) {
    return Vector3.Distance(obj.transform.position, player.transform.position);
  }
}
