using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingCharacters : MonoBehaviour {
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
  private List<Mesh> characterMeshes;
  private int charactersCount;
  public int activeCount;

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

    characterMeshes = new List<Mesh>();
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
    charactersCount = 0;

    foreach (Transform character in allCharacters) {
      if (DataManager.dm.getBool(character.name) && character.name != PlayerPrefs.GetString("SelectedCharacter")) {
        characterMeshes.Add(character.GetComponent<MeshFilter>().sharedMesh);
        charactersCount++;
      }
    }

    if (charactersCount > 0) StartCoroutine("showNewCharacter");
  }

  bool isAlreadyIn(Mesh mesh) {
    for (int i = 0; i < characterPool.Count; i++) {
      if (characterPool[i].activeInHierarchy) {
        if (characterPool[i].GetComponent<MeshFilter>().sharedMesh == mesh) return true;
      }
    }
    return false;
  }

  Mesh randomMesh() {
    Mesh mesh;
    do {
     mesh = characterMeshes[Random.Range(0, charactersCount)];
    } while(isAlreadyIn(mesh));

    return mesh;
  }

  IEnumerator showNewCharacter() {
    while (true) {
      if (activeCount < charactersCount) {
        Vector2 screenPos = Random.insideUnitCircle;
        screenPos.Normalize();
        screenPos *= spawnRadius;

        Vector3 spawnPos = screenToWorld(screenPos);
        Vector3 direction = playerPos() - spawnPos;
        direction.Normalize();

        GameObject instance = getCharacter();
        instance.transform.parent = transform;
        instance.transform.position = spawnPos;
        instance.GetComponent<MeshFilter>().sharedMesh = randomMesh();
        instance.SetActive(true);
        instance.GetComponent<FlyingCharacterMover>().run(this, direction);
        activeCount++;
      }

      yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
    }
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + Player.pl.transform.position.x, transform.position.y, screenPos.y + Player.pl.transform.position.z);
  }

  Vector3 playerPos() {
    return Player.pl.transform.position;
  }

  void OnDisable() {
    StopCoroutine("showNewCharacter");
  }
}
