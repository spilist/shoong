using UnityEngine;
using System.Collections;

public class CharacterChangeManager : MonoBehaviour {
  public Mesh monsterMesh;
  public Material monsterMaterial;
  public Material metalMat;
  public Material originalMaterial;

  private Mesh originalMesh;

  private MeshFilter mFilter;
  private Renderer mRenderer;

  public Transform playerParticlesParent;
  public ParticleSystem ridingEffect;
  public ParticleSystem monsterEffect;
  public ParticleSystem booster;
  public ParticleSystem getEnergy;
  public ParticleSystem unstoppableEffect;
  public ParticleSystem unstoppableEffect_two;
  public ParticleSystem getSpecialEnergyEffect;
  public ParticleSystem getComboParts;
  public ParticleSystem getBlackholeEffect;
  public ParticleSystem rainbowEffect;
  public ParticleSystem chargedEffect;
  public ParticleSystem afterStrengthenEffect;

  void OnEnable() {
    mFilter = GetComponent<MeshFilter>();
    mRenderer = GetComponent<Renderer>();
  }

  public void changeCharacter(Mesh mesh, Material material) {
    mFilter.sharedMesh = mesh;
    mRenderer.sharedMaterial = material;
  }

  public void changeCharacterToMonster() {
    changeCharacter(monsterMesh, monsterMaterial);
    monsterEffect.Play();
    monsterEffect.GetComponent<AudioSource>().Play();
  }

  public void changeCharacterToMetal() {
    changeCharacter(originalMesh, metalMat);
    unstoppableEffect.Play();
    unstoppableEffect.GetComponent<AudioSource>().Play ();
      // unstoppableEffect_two.Play();
    unstoppableEffect_two.GetComponent<AudioSource>().Play ();
  }

  public void changeCharacterToOriginal() {
    changeCharacter(originalMesh, originalMaterial);
  }

  public void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    mFilter.sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;

    booster = Instantiate(Resources.Load(characterName + "/Booster", typeof(ParticleSystem))) as ParticleSystem;
    booster.transform.parent = playerParticlesParent;
    booster.transform.localScale = Vector3.one;
    booster.transform.localPosition = Vector3.zero;
    booster.transform.localRotation = Quaternion.identity;

    originalMesh = mFilter.sharedMesh;
  }
}
