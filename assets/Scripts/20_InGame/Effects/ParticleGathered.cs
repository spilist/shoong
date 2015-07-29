using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleGathered : MonoBehaviour {
  ParticleSystem m_System;
  ParticleSystem.Particle[] m_Particles;
  PartsCollector partsCollector;

  void Start() {
    InitializeIfNeeded();
  }

	void LateUpdate () {
    int numParticlesAlive = m_System.GetParticles(m_Particles);
    for (int i = 0; i < numParticlesAlive; i++) {
      if (m_Particles[i].lifetime < 0.5f) {
        Vector3 heading =  partsCollector.transform.position - m_Particles[i].position;
        heading /= heading.magnitude;
        m_Particles[i].velocity = heading * GameObject.Find("Player").GetComponent<Rigidbody>().velocity.magnitude * 3;
      }
    }

    m_System.SetParticles(m_Particles, numParticlesAlive);
	}

  void InitializeIfNeeded() {
    if (m_System == null)
      m_System = GetComponent<ParticleSystem>();

    if (m_Particles == null || m_Particles.Length < m_System.maxParticles)
      m_Particles = new ParticleSystem.Particle[m_System.maxParticles];

    if (partsCollector == null)
      partsCollector = GameObject.Find("PartsCollector").GetComponent<PartsCollector>();
  }
}
