using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterCreateButton : MenusBehavior {
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

  public int originalSize = 200;
  public int shrinkSize = 180;
  public float totalSeconds = 3f;
  public float startInterval = 0.3f;
  public float showUIsAfter = 1f;
  public int characterRotatingSpeed = 150;
  public int createPrice = 10000;

  void OnEnable() {
    transform.Find("PriceText").GetComponent<Text>().text = createPrice.ToString("N0");
  }

  override public void activateSelf() {
    turnOnOff(false);
    createdCharacter.SetActive(false);
    characterName.enabled = false;
    characterCube.SetActive(true);
    congraturation.GetComponent<ParticleSystem>().Stop();
    congraturation.gameObject.SetActive(false);
    isNewCharacter.SetActive(false);
    gathering.Play();
    gathering.GetComponent<AudioSource>().Play();
    explosion.Play();
    collect.Play();

    StartCoroutine("shake");
  }

  void turnOnOff(bool val) {
    backButton.SetActive(val);
    cubesYouHave.SetActive(val);
    goldenCubesYouHave.SetActive(val);
    GetComponent<Renderer>().enabled = val;
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

    Dictionary<object, object> characters = GameController.control.characters.Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value);
    characters.Remove("robotcogi");
    int random = Random.Range(0, characters.Count - 1);
    string createdCharacterName = (string) characters.ElementAt(random).Key;
    bool newCharacter = !(bool)GameController.control.characters[createdCharacterName];

    float duration = totalSeconds;
    float interval = startInterval;
    while (duration > 0) {
      characterCube.transform.localScale = shrinkScale;
      yield return new WaitForSeconds(interval);
      characterCube.transform.localScale = originalScale;
      yield return new WaitForSeconds(interval);

      duration -= interval * 2;

      if (interval >= 0.1f) interval -= 0.05f;

      if (duration < 0.5f) {
        congraturation.gameObject.SetActive(true);
      }
    }
    characterCube.SetActive(false);

    cubesYouHave.GetComponent<CubesYouHave>().buy(createPrice);

    createdCharacter.GetComponent<MeshFilter>().sharedMesh = Resources.Load<GameObject>("_characters/play_characters").transform.FindChild(createdCharacterName).GetComponent<MeshFilter>().sharedMesh;
    createdCharacter.SetActive(true);

    characterName.text = PlayerPrefs.GetString(createdCharacterName);
    characterName.enabled = true;

    if (newCharacter) {
      isNewCharacter.SetActive(true);
      GameController.control.characters[createdCharacterName] = true;
    } else {
      nextChance.Play();
    }

    yield return new WaitForSeconds(showUIsAfter);

    turnOnOff(true);
  }
}
