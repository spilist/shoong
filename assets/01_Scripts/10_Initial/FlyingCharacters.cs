using UnityEngine;
using System.Collections;

public class FlyingCharacters : MonoBehaviour {
  public PlayerMover player;
  public Transform allCharacters;
  public GameObject characterPrefab;

  public float spawnRadius = 200;
  public int speed = 200;
  public int tumble = 5;

  private bool first = true;
  private Mesh[] characterMeshes;
  private int charactersCount;

  void OnEnable () {
    if (first) return;

    reset();
  }

  public void reset() {
    first = false;
    characterMeshes = new Mesh[allCharacters.childCount];
    charactersCount = 0;

    foreach (Transform character in allCharacters) {
      if (DataManager.dm.getBool(character.name) && character.name != PlayerPrefs.GetString("SelectedCharacter")) {
        characterMeshes[charactersCount++] = character.GetComponent<MeshFilter>().sharedMesh;
        charactersCount++;
      }
    }

    showNewCharacter();
  }

  Mesh randomMesh() {
    return characterMeshes[Random.Range(0, charactersCount)];
  }

  public void showNewCharacter() {
    Vector2 screenPos = Random.insideUnitCircle;
    screenPos.Normalize();
    screenPos *= spawnRadius;

    Vector3 spawnPos = screenToWorld(screenPos);
    Vector3 direction = playerPos() - spawnPos;
    direction.Normalize();

    GameObject instance = (GameObject) Instantiate(characterPrefab, spawnPos, Quaternion.identity);
    instance.transform.parent = transform;
    instance.GetComponent<MeshFilter>().sharedMesh = randomMesh();
    instance.GetComponent<FlyingCharacterMover>().run(direction, speed, tumble);
  }

  Vector3 screenToWorld(Vector3 screenPos) {
    return new Vector3(screenPos.x + player.transform.position.x, player.transform.position.y, screenPos.y + player.transform.position.z);
  }

  Vector3 playerPos() {
    return player.transform.position + player.getDirection() * player.getSpeed();
  }
}
