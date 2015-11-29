using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforeMain : MonoBehaviour {
  public Text copyright;
  private Color copyrightColor;
  public float copyrightShowAfter = 0.3f;
  public float copyrightAlphaChangeDuration = 0.2f;
  private float copyrightHideCount = 0;

  public GameObject title;
  public float movingDuration = 0.6f;
  private bool titleMoving = false;
  private float titlePosX;
  private float distance;

  public GameObject character;
  public float characterMovingDuration = 0.6f;
  private float characterPosX;
  private float characterMoveDistance;
  private bool characterMoving = false;
  public float characterStayDuration = 0.2f;
  private float characterStayCount = 0;
  private bool boosterPlayed = false;
  private float boosterStayCount = 0;

  public float boosterStayDuration = 0.5f;

	void Start () {
    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));
    titlePosX = title.GetComponent<RectTransform>().anchoredPosition.x;

    titleMoving = true;
    distance = Mathf.Abs(titlePosX);

    characterMoving = true;
    characterPosX = titlePosX;
    characterMoveDistance = distance;

    copyrightColor = new Color(1, 1, 1, 0);
	}

	void Update () {
    if (titleMoving) {
      titlePosX = Mathf.MoveTowards(titlePosX, 0, Time.deltaTime / movingDuration * distance);
      title.GetComponent<RectTransform>().anchoredPosition = new Vector2(titlePosX, title.GetComponent<RectTransform>().anchoredPosition.y);

      if (titlePosX == 0) {
        titleMoving = false;
      }
    }

    if (characterMoving) {
      if (characterStayCount < characterStayDuration) {
        characterStayCount += Time.deltaTime;
      } else {
        if (!boosterPlayed) {
          boosterPlayed = true;
          character.transform.Find("Booster").GetComponent<AudioSource>().Play();
          character.transform.Find("Booster").GetComponent<ParticleSystem>().Play();
        }

        characterPosX = Mathf.MoveTowards(characterPosX, 0, Time.deltaTime / characterMovingDuration * characterMoveDistance);
        character.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosX, character.GetComponent<RectTransform>().anchoredPosition.y);

        if (characterPosX == 0) {
          if (character.GetComponent<Rigidbody>().angularVelocity == Vector3.zero) {
            character.GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * 4;
          }

          if (boosterStayCount < boosterStayDuration) {
            boosterStayCount += Time.deltaTime;
          } else {
            characterMoving = false;
            Vector3 rot = character.transform.rotation.eulerAngles;
            Vector3 angVal = character.GetComponent<Rigidbody>().angularVelocity;
            PlayerPrefs.SetString("CharacterRotation", rot.ToString().TrimStart('(').TrimEnd(')'));
            PlayerPrefs.SetString("CharacterAngVal", angVal.ToString().TrimStart('(').TrimEnd(')'));

            Application.LoadLevelAsync("5_Main");
          }
        }
      }

      if (copyrightHideCount < copyrightShowAfter) {
        copyrightHideCount += Time.deltaTime;
      } else {
        copyrightColor.a = Mathf.MoveTowards(copyrightColor.a, 1, Time.deltaTime / copyrightAlphaChangeDuration);
        copyright.color = copyrightColor;
      }
    }
	}

  void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    character.GetComponent<MeshFilter>().sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;
  }
}
