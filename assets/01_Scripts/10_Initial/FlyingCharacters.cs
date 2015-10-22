using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingCharacters : MonoBehaviour {
  public PlayerMover player;
  public Transform allCharacters;
  public GameObject characterPrefab;
  public List<GameObject> characterPool;
  public int characterAmount = 20;

  public float minSpawnInterval = 1;
  public float maxSpawnInterval = 3;

  public float spawnRadius = 200;
  public int speed = 200;
  public int baseSpeed = 50;
  public int tumble = 5;

  public int minBoosterAmount = 40;
  public int maxBoosterAmonut = 120;
  public int boosterSpeedDecreaseBase = 70;
  public int boosterSpeedDecreasePerTime = 20;
  public float delayAfterMove = 0.5f;

  private bool first = true;
  private Mesh[] characterMeshes;
  private int charactersCount;

  void OnEnable () {
    if (first) return;

    reset();
  }

  void Start() {
    characterPool = new List<GameObject>();
    for (int i = 0; i < characterAmount; ++i) {
      GameObject obj = (GameObject) Instantiate(characterPrefab);
      obj.SetActive(false);
      obj.transform.parent = transform;
      characterPool.Add(obj);
    }
  }

  GameObject getCharacter() {
    for (int i = 0; i < characterPool.Count; i++) {
      if (!characterPool[i].activeInHierarchy) {
        return characterPool[i];
      }
    }

    GameObject obj = (GameObject) Instantiate(characterPrefab);
    obj.transform.parent = transform;
    characterPool.Add(obj);
    return obj;
  }

  public void reset() {
    first = false;
    characterMeshes = new Mesh[allCharacters.childCount];
    charactersCount = 0;

    foreach (Transform character in allCharacters) {
      if (DataManager.dm.getBool(character.name) && character.name != PlayerPrefs.GetString("SelectedCharacter")) {
        characterMeshes[charactersCount++] = character.GetComponent<MeshFilter>().sharedMesh;
      }
    }

    StartCoroutine("showNewCharacter");
  }

  Mesh randomMesh() {
    return characterMeshes[Random.Range(0, charactersCount)];
  }

  IEnumerator showNewCharacter() {
    while (true) {
      Vector2 screenPos = Random.insideUnitCircle;
      screenPos.Normalize();
      screenPos *= spawnRadius;

      Vector3 spawnPos = screenToWorld(screenPos);
      Vector3 direction = playerPos() - spawnPos;
      direction.Normalize();

      GameObject instance = getCharacter();
      instance.transform.parent = transform;
      instance.transform.position = spawnPos;
      instance.SetActive(true);
      instance.GetComponent<MeshFilter>().sharedMesh = randomMesh();
      instance.GetComponent<FlyingCharacterMover>().run(this, direction);

      yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
    }
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, transform.position.y, screenPos.y + player.transform.position.z);
  }

  Vector3 playerPos() {
    return player.transform.position;
    // return player.transform.position + player.getDirection() * player.getSpeed();
  }

  void OnDisable() {
    StopCoroutine("showNewCharacter");
  }
}
