using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterCreateButton : MenusBehavior {
  public Transform characters;

  public GameObject characterCube;
  public ParticleSystem gathering;
  public ParticleSystem explosion;
  public ParticleSystem collect;
  public ParticleSystem congraturation;
  public GameObject createdCharacter;
  public Text characterName;
  public GameObject isNewCharacter;
  public AudioSource nextChance;

  public GameObject backButton;
  public GameObject cubesYouHave;
  public GameObject goldenCubesYouHave;
  public Color notAffordableTextColor;
  public Mesh originalMesh;
  public Mesh blinkingMesh;
  public float blinkingSeconds = 0.4f;

  public int originalSize = 200;
  public int shrinkSize = 180;
  public float totalSeconds = 3f;
  public float startInterval = 0.3f;
  public float showUIsAfter = 1f;
  public int characterRotatingSpeed = 150;
  private int createPrice;

  private bool affordable = false;
  private bool running = false;
  private Text priceText;
  private CharacterCreateMenu menu;

  void OnEnable() {
    menu = transform.parent.GetComponent<CharacterCreateMenu>();
    priceText = transform.Find("PriceText").GetComponent<Text>();
    resetAll();
    checkAffordable();
  }

  void resetAll() {
    createdCharacter.SetActive(false);
    characterName.enabled = false;
    characterCube.SetActive(true);
    congraturation.GetComponent<ParticleSystem>().Stop();
    congraturation.gameObject.SetActive(false);
    isNewCharacter.SetActive(false);
  }

  void checkAffordable() {
    createPrice = menu.price();
    priceText.text = createPrice.ToString("N0");
    transform.Find("CubeIcon").GetComponent<BuyButtonsCubeIconPosition>().adjust(priceText);

    if (cubesYouHave.GetComponent<CubesYouHave>().youHave() < createPrice) {
      affordable = false;
      priceText.color = notAffordableTextColor;
      GetComponent<Collider>().enabled = false;
      StopCoroutine("blinkButton");
      GetComponent<MeshFilter>().sharedMesh = originalMesh;
    } else {
      affordable = true;
      priceText.color = new Color(255, 255, 255);
      GetComponent<Collider>().enabled = true;
      StartCoroutine("blinkButton");
    }
  }

  IEnumerator blinkButton() {
    while(true) {
      GetComponent<MeshFilter>().sharedMesh = originalMesh;

      yield return new WaitForSeconds(blinkingSeconds);

      GetComponent<MeshFilter>().sharedMesh = blinkingMesh;

      yield return new WaitForSeconds(blinkingSeconds);
    }
  }

  override public void activateSelf() {
    if (!affordable || running) return;

    running = true;
    resetAll();
    turnOnOff(false);
    gathering.Play();
    gathering.GetComponent<AudioSource>().Play();
    explosion.Clear();
    explosion.Play();
    collect.Play();
    StopCoroutine("blinkButton");
    GetComponent<MeshFilter>().sharedMesh = originalMesh;

    StartCoroutine("shake");
  }

  void turnOnOff(bool val) {
    backButton.SetActive(val);
    cubesYouHave.SetActive(val);
    goldenCubesYouHave.SetActive(val);
    GetComponent<Renderer>().enabled = val;
    GetComponent<Collider>().enabled = val;
    transform.Find("CubeIcon").GetComponent<Renderer>().enabled = val;
    transform.Find("PriceText").GetComponent<Text>().enabled = val;
  }

  void Update() {
    if (createdCharacter.activeSelf) {
      createdCharacter.transform.Rotate(-Vector3.up * Time.deltaTime * characterRotatingSpeed);
    }
  }

  IEnumerator shake() {
    Vector3 originalScale = new Vector3(originalSize, originalSize, originalSize);
    Vector3 shrinkScale = new Vector3(shrinkSize, shrinkSize, shrinkSize);

    int random = Random.Range(1, characters.childCount);
    string createdCharacterName = characters.GetChild(random).name;
    bool newCharacter = !DataManager.dm.getBool(createdCharacterName);

    float duration = totalSeconds;
    float interval = startInterval;
    while (duration > 0) {
      characterCube.transform.localScale = shrinkScale;
      yield return new WaitForSeconds(interval);
      characterCube.transform.localScale = originalScale;
      yield return new WaitForSeconds(interval);

      duration -= interval * 2;

      if (interval >= 0.1f) interval -= 0.05f;

      if (newCharacter && duration < 0.5f) {
        congraturation.gameObject.SetActive(true);
      }
    }
    characterCube.SetActive(false);

    cubesYouHave.GetComponent<CubesYouHave>().buy(createPrice);

    createdCharacter.GetComponent<MeshFilter>().sharedMesh = Resources.Load<GameObject>("_characters/play_characters").transform.FindChild(createdCharacterName).GetComponent<MeshFilter>().sharedMesh;
    createdCharacter.SetActive(true);

    characterName.text = getCharacterName(createdCharacterName);
    characterName.enabled = true;

    if (newCharacter) {
      isNewCharacter.SetActive(true);
      DataManager.dm.setBool(createdCharacterName, true);
      DataManager.dm.increment("NumCharactersHave");
    } else {
      nextChance.Play();
    }

    yield return new WaitForSeconds(showUIsAfter);

    turnOnOff(true);
    running = false;
    checkAffordable();
  }

  void OnDisable() {
    cubesYouHave.SetActive(false);
    goldenCubesYouHave.SetActive(false);
  }

  public bool isAffordable() {
    return cubesYouHave.GetComponent<CubesYouHave>().youHave() < createPrice;
  }

  string getCharacterName(string characterCode) {
    foreach (Transform tr in characters) {
      UICharacters cha = tr.GetComponent<UICharacters>();
      if (cha.name == characterCode) return cha.characterName;
    }

    return "";
  }
}
