using UnityEngine;
using System.Collections;

public class CharacterChangeManager : MonoBehaviour {
  public Transform characters;
  public DoppleManager dpm;
  public Material redMaterial;

  public Mesh monsterMesh;
  public Material monsterMaterial;
  public Material metalMaterial;
  public Material blinkMaterial;
  public Material ghostMaterial;
  public Material solarMaterial;
  public float redDuration = 1;
  private float redCount = 0;
  private bool beingRed = false;

  public Material originalMaterial;
  private Mesh originalMesh;

  private MeshFilter mFilter;
  private Renderer mRenderer;

  public Transform playerParticlesParent;
  public ParticleSystem booster;
  public ParticleSystem afterStrengthenEffect;
  public Collider contactCollider;

  public float teleportingDuration;
  private int teleportingStatus = 0;
  private Vector3 teleportTo;
  private Color color;
  private float alphaOrigin;
  private float alpha = 0;
  private float stayCount = 0;

  void OnEnable() {
    mFilter = GetComponent<MeshFilter>();
    mRenderer = GetComponent<Renderer>();
    color = mRenderer.sharedMaterial.color;
    alphaOrigin = color.a;
  }

  public void teleport(Vector3 pos) {
    stayCount = 0;
    teleportTo = pos;
    alpha = alphaOrigin;
    teleportingStatus = 1;

    Camera.main.GetComponent<CameraMover>().setSlowly(true);
  }

  void Update() {
    if (beingRed) {
      if (redCount > 0) redCount -= Time.deltaTime;
      else {
        beingRed = false;
        changeCharacterToOriginal();
      }
    }

    if (teleportingStatus == 1) {
      alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime * alphaOrigin / teleportingDuration);
      color.a = alpha;
      mRenderer.sharedMaterial.color = color;
      if (alpha == 0) {
        transform.parent.position = teleportTo;
        alpha = alphaOrigin / 2;
        color.a = alpha;
        mRenderer.sharedMaterial.color = color;
        teleportingStatus++;
      }
    } else if (teleportingStatus == 2) {
      if (stayCount < teleportingDuration) stayCount += Time.deltaTime;
      else {
        alpha = 0;
        color.a = alpha;
        mRenderer.sharedMaterial.color = color;
        dpm.goodFieldAt();
        Camera.main.GetComponent<CameraMover>().setSlowly(false);
        teleportingStatus++;
      }
    } else if (teleportingStatus == 3) {
      alpha = Mathf.MoveTowards(alpha, alphaOrigin, Time.deltaTime * alphaOrigin / teleportingDuration);
      color.a = alpha;
      mRenderer.sharedMaterial.color = color;
      if (alpha == alphaOrigin) {
        teleportingStatus = 0;
        Player.pl.afterStrengthenStart();
      }
    }
  }

  public bool isTeleporting() {
    return teleportingStatus > 0;
  }

  public void changeCharacterTo(string changeTo) {
    if (changeTo == "Monster") {
      changeCharacter(monsterMesh, monsterMaterial);
    } else if (changeTo == "Metal") {
      changeCharacter(originalMesh, metalMaterial);
    } else if (changeTo == "Blink") {
      changeCharacter(originalMesh, blinkMaterial);
    } else if (changeTo == "Ghost") {
      changeCharacter(originalMesh, ghostMaterial);
    } else if (changeTo == "Solar") {
      changeCharacter(originalMesh, solarMaterial);
    }
  }

  public void changeCharacter(Mesh mesh, Material material) {
    mFilter.sharedMesh = mesh;
    mRenderer.sharedMaterial = material;
    beingRed = false;
    redCount = 0;
  }

  public void changeRed() {
    mRenderer.sharedMaterial = redMaterial;
    beingRed = true;
    redCount = redDuration;
  }

  public void changeCharacterToOriginal() {
    teleportingStatus = 0;
    changeCharacter(originalMesh, originalMaterial);
    Player.pl.scaleBack();

    StopCoroutine("characterBlinking");
  }

  public void setMesh(Mesh mesh) {
    mFilter.sharedMesh = mesh;
    originalMesh = mesh;
  }
}
