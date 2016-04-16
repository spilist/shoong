using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AbilityData;

public class CharacterCreateButton : MenusBehavior {
  public int rareChance = 40;
  public int epicChance = 10;
  public int legendChance = 1;
  public int chanceBase = 200;

  private List<UICharacters> commons;
  private List<UICharacters> rares;
  private List<UICharacters> epics;
  private List<UICharacters> legends;

  public Transform characters;
  public GameObject characterCube;
  public ParticleSystem gathering;
  public ParticleSystem explosion;
  public ParticleSystem collect;
  public ParticleSystem congraturation;
  public GameObject createdCharacter;
  public Text characterName;
  public Text rarity;
  public GameObject isNewCharacter;
  public GameObject nextChance;

  public ShareButton shareButton;
  public CharacterSelectButton selectButton;
  public GameObject backButton;
  public GameObject goldenCubesYouHave;
  public GameOverGoldCubes gameOverGoldCubes;
  public Color notAffordableTextColor;

  public int originalSize = 200;
  public int shrinkSize = 180;
  public float totalSeconds = 3f;
  public float startInterval = 0.3f;
  public float showUIsAfter = 1f;
  public int characterRotatingSpeed = 150;
  private int createPrice;

  private bool affordable = false;
  public bool running = false;
  private Text priceText;
  private CharacterCreateMenu menu;
  private string randomResult;
  public bool first;

  override public void initializeRest() {
    menu = transform.parent.GetComponent<CharacterCreateMenu>();
    priceText = transform.Find("PriceText").GetComponent<Text>();

    commons = new List<UICharacters>();
    rares = new List<UICharacters>();
    epics = new List<UICharacters>();
    legends = new List<UICharacters>();
    foreach (Transform tr in characters) {
      UICharacters uic = tr.GetComponent<UICharacters>();
      if (uic.name == "robotcogi") continue;
      CharacterStat stat = CharacterManager.cm.character(uic.name);

      if (stat.rarity == Rarity.Common) commons.Add(uic);
      else if (stat.rarity == Rarity.Rare) rares.Add(uic);
      else if (stat.rarity == Rarity.Epic) epics.Add(uic);
      else if (stat.rarity == Rarity.Legendary) legends.Add(uic);
    }
  }

  UICharacters getRandom() {
    List<UICharacters> list;
    int random = Random.Range(0, chanceBase);
    if (random < legendChance) {
      list = legends;
      randomResult = "Legendary";
    } else if (random < legendChance + epicChance) {
      list = epics;
      randomResult = "Epic";
    } else if (random < legendChance + epicChance + rareChance) {
      list = rares;
      randomResult = "Rare";
    } else {
      list = commons;
      randomResult = "Common";
    }

    return list[Random.Range(0, list.Count)];
  }

  void OnEnable() {
    resetAll();
    checkAffordable();
  }

  void resetAll() {
    createdCharacter.SetActive(false);
    characterName.enabled = false;
    characterCube.SetActive(true);
    congraturation.GetComponent<ParticleSystem>().Stop();
    congraturation.gameObject.SetActive(false);
    isNewCharacter.GetComponent<Text>().enabled = false;
    nextChance.GetComponent<Text>().enabled = false;
    shareButton.gameObject.SetActive(false);
    selectButton.gameObject.SetActive(false);
    goldenCubesYouHave.SetActive(true);
    rarity.text = "";
    GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetComponent<RectTransform>().anchoredPosition.y);
  }

  void checkAffordable() {
    createPrice = menu.createPrice;
    priceText.text = createPrice.ToString("N0");
    transform.Find("Icon").GetComponent<BuyButtonsCubeIconPosition>().adjust(priceText);

    if (GoldManager.gm.getCount() < createPrice) {
      affordable = false;
      priceText.color = notAffordableTextColor;
      GetComponent<Collider>().enabled = false;
      stopBlink();
    } else {
      affordable = true;
      priceText.color = new Color(255, 255, 255);
      GetComponent<Collider>().enabled = true;
      startBlink();
    }
  }

  override public void activateSelf() {
    if (!affordable || running) return;

    DataManager.dm.increment("NumCharacterCreate");

    if (first) TrackingManager.tm.firstPlayLog("7_FirstToyWin");

    running = true;
    resetAll();
    turnOnOff(false);
    gathering.Clear();
    gathering.Play();
    gathering.GetComponent<AudioSource>().Play();
    explosion.Clear();
    explosion.Play();
    collect.Play();
    stopBlink();

    StartCoroutine("shake");
  }

  void turnOnOff(bool val) {
    // shareButton.gameObject.SetActive(val);
    selectButton.gameObject.SetActive(val);
    backButton.GetComponent<Renderer>().enabled = val;
    backButton.GetComponent<Collider>().enabled = val;
    goldenCubesYouHave.SetActive(val);
    GetComponent<Renderer>().enabled = val;
    GetComponent<Collider>().enabled = val;
    transform.Find("Icon").GetComponent<Renderer>().enabled = val;
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

    UICharacters randomCharacter = getRandom();
    bool newCharacter = !DataManager.dm.getBool(randomCharacter.name);
    TrackingManager.tm.createToy(DataManager.dm.getInt("NumCharacterCreate"), randomResult, randomCharacter.name, newCharacter);

    GoldManager.gm.buy(createPrice);
    goldenCubesYouHave.GetComponent<CubesYouHave>().buy(createPrice);
    gameOverGoldCubes.change(-createPrice, false);

    if (newCharacter) {
      DataManager.dm.setBool(randomCharacter.name, true);
      DataManager.dm.increment("NumCharactersHave");
    }

    DataManager.dm.save();

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

    createdCharacter.GetComponent<MeshFilter>().sharedMesh = randomCharacter.GetComponent<MeshFilter>().sharedMesh;
    createdCharacter.SetActive(true);

    characterName.text = CharacterManager.cm.character(randomCharacter.name).characterName;
    characterName.enabled = true;
    randomCharacter.setRarity(rarity);
    selectButton.setCharacter(randomCharacter.name);

    if (newCharacter) {
      isNewCharacter.GetComponent<Text>().enabled = true;
      nextChance.GetComponent<Text>().enabled = false;
      isNewCharacter.GetComponent<AudioSource>().Play();
    } else {
      isNewCharacter.GetComponent<Text>().enabled = false;
      nextChance.GetComponent<Text>().enabled = true;
      nextChance.GetComponent<AudioSource>().Play();
    }

    yield return new WaitForSeconds(showUIsAfter);

    turnOnOff(true);
    GetComponent<RectTransform>().anchoredPosition = new Vector2(240, GetComponent<RectTransform>().anchoredPosition.y);

    checkAffordable();

    running = false;
  }

  void OnDisable() {
    goldenCubesYouHave.SetActive(false);
  }
}
