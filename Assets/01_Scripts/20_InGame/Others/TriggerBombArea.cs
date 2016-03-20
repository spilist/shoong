using UnityEngine;
using System.Collections;

public class TriggerBombArea : MonoBehaviour {
  Renderer areaRenderer;
  Color areaColor;
  float originalAlpha;
  AudioSource audio;
  bool isTriggered = false;

  void Awake() {
    areaRenderer = GetComponent<Renderer>();
    areaColor = areaRenderer.sharedMaterial.GetColor("_TintColor");
    originalAlpha = areaColor.a;
    audio = GetComponent<AudioSource>();
  }

  void OnEnable() {
    isTriggered = false;
    areaColor.a = originalAlpha;
    areaRenderer.sharedMaterial.SetColor("_TintColor", areaColor);
  }

  void OnTriggerEnter(Collider other) {
    if (!isTriggered && other.tag == "Player") {
      transform.parent.GetComponent<DangerousEMPMover>().unstabilize();
      areaColor.a = originalAlpha * 2;
      areaRenderer.material.SetColor("_TintColor", areaColor);
      audio.Play();
    }
  }

  void OnDisable() {
    audio.Stop();
  }
}
