using UnityEngine;
using System.Collections;

public class Skill_Superheat : MonoBehaviour {
  private Collider sCollider;
  private ParticleSystem particle;
  private AudioSource sound;

  void Awake() {
    sCollider = GetComponent<Collider>();
    particle = GetComponent<ParticleSystem>();
    sound = GetComponent<AudioSource>();
  }

  public void getReady() {
    sCollider.enabled = true;
    particle.Play();
    sound.Play();
  }

  void OnPointerDown() {
    SuperheatManager.sm.startSuperheat();
    sCollider.enabled = false;
    particle.Stop();
  }
}
