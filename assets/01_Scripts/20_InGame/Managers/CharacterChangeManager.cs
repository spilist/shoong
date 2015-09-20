using UnityEngine;
using System.Collections;

public class CharacterChangeManager : MonoBehaviour {
  public Mesh monsterMesh;
  public Material monsterMaterial;
  public Material metalMat;
  public Material originalMaterial;
  public Color originalColor;

  public Color jetpackColor;

  public float colorChangeDuration_1 = 0.2f;
  public float colorChangeDuration_2 = 0.5f;

  private Mesh originalMesh;

  private MeshFilter mFilter;
  private Renderer mRenderer;

  public Transform playerParticlesParent;
  public ParticleSystem booster;
  public ParticleSystem chargedEffect;
  public ParticleSystem afterStrengthenEffect;

  void OnEnable() {
    mFilter = GetComponent<MeshFilter>();
    mRenderer = GetComponent<Renderer>();
  }

  public void changeCharacterTo(string changeTo) {
    if (changeTo == "Monster") {
      changeCharacter(monsterMesh, monsterMaterial);
    } else if (changeTo == "Metal") {
      changeCharacter(originalMesh, metalMat);
    } else if (changeTo == "Jetpack") {
      // characterBlinkCoroutine = StartCoroutine(characterBlinkingWith(jetpackColor));
      mRenderer.sharedMaterial.SetColor("_ReflectColor", jetpackColor);
    }
  }

  IEnumerator characterBlinkingWith(Color color) {
    while (true) {
      mRenderer.sharedMaterial.SetColor("_ReflectColor", color);
      yield return new WaitForSeconds(colorChangeDuration_2);

      mRenderer.sharedMaterial.SetColor("_ReflectColor", originalColor);
      yield return new WaitForSeconds(colorChangeDuration_1);
    }
  }

  public void changeCharacter(Mesh mesh, Material material) {
    mFilter.sharedMesh = mesh;
    mRenderer.sharedMaterial = material;
  }

  public void changeCharacterToOriginal() {
    changeCharacter(originalMesh, originalMaterial);
    mRenderer.sharedMaterial.SetColor("_ReflectColor", originalColor);
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
