using UnityEngine;
using System.Collections;

public class CharacterChangeManager : MonoBehaviour {
  public Transform characters;
  public DoppleManager dpm;

  public Mesh monsterMesh;
  public Material monsterMaterial;
  public Material metalMat;
  public Material redMaterial;
  public float redDuration = 1;
  private float redCount = 0;
  private bool beingRed = false;

  public Material playerEffectMat;
  public Color[] metalColors;
  public Color[] doppleColors;
  public Color[] doppleMetalColors;

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
  }

  public void teleport(Vector3 pos) {
    stayCount = 0;
    teleportTo = pos;
    color = mRenderer.sharedMaterial.color;
    alpha = color.a;
    alphaOrigin = alpha;
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
        transform.position = teleportTo;
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
        // dpm.goodFieldAt(teleportTo);
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
      if (mRenderer.sharedMaterial == playerEffectMat) {
        changeCharacter(monsterMesh, playerEffectMat);
      } else {
        changeCharacter(monsterMesh, monsterMaterial);
      }
    } else if (changeTo == "Metal") {
      if (mRenderer.sharedMaterial.color == doppleColors[0]) {
        playerEffectMat.color = doppleMetalColors[0];
        playerEffectMat.SetColor("_Emission", doppleMetalColors[1]);
      }
      else {
        playerEffectMat.color = metalColors[0];
        playerEffectMat.SetColor("_Emission", metalColors[1]);
      }
      changeCharacter(originalMesh, playerEffectMat);
    } else if (changeTo == "Blink") {
      if (mRenderer.sharedMaterial.color == metalColors[0]) {
        playerEffectMat.color = doppleMetalColors[0];
        playerEffectMat.SetColor("_Emission", doppleMetalColors[1]);
      }
      else {
        playerEffectMat.color = doppleColors[0];
        playerEffectMat.SetColor("_Emission", doppleColors[1]);
      }
      changeCharacter(originalMesh, playerEffectMat);
    }
  }

  public void changeCharacter(Mesh mesh, Material material) {
    mFilter.sharedMesh = mesh;
    mRenderer.sharedMaterial = material;
  }

  public void changeRed() {
    mRenderer.sharedMaterial = redMaterial;
    beingRed = true;
    redCount = redDuration;
  }

  public void changeCharacterToOriginal() {
    teleportingStatus = 0;
    changeCharacter(originalMesh, originalMaterial);
    StopCoroutine("characterBlinking");
  }

  public void setMesh(Mesh mesh) {
    mFilter.sharedMesh = mesh;
    originalMesh = mesh;

    // booster = Instantiate(Resources.Load(characterName + "/Booster", typeof(ParticleSystem))) as ParticleSystem;
    // booster.transform.parent = playerParticlesParent;
    // booster.transform.localScale = Vector3.one;
    // booster.transform.localPosition = Vector3.zero;
    // booster.transform.localRotation = Quaternion.identity;
  }
}
