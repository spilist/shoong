using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforeIdle : MonoBehaviour {
  public FlyingCharacters flyingCharacters;
  public SpawnManager spawnManager;
  public MeshRenderer titleFilter;
  public GameObject title;
  // public GameObject powerBoostUI;
  public Text copyright;
  private Color copyrightColor;
  private float copyrightChangeTo;

  public GameObject character;
  private Color characterColor;
  public float characterMovingDuration = 0.6f;
  private float characterPosX;
  public float characterMoveStart = -800;
  private float characterMoveDistance;
  private bool characterMoving = false;
  public float characterStayDuration = 0.2f;
  private float characterStayCount = 0;
  private bool boosterPlayed = false;
  private float boosterStayCount = 0;

  public float stayDuration = 0.5f;
  private float stayCount = 0;

  public float filterChangingDuration = 0.3f;
  private bool filterChanging = false;
  private Color filterColor;
  private float filterChangeTo;

  public float movingDuration = 0.6f;
  private bool titleMoving = false;
  private float titlePosX;
  private float titleMoveTo;
  private float distance;

  public float boosterStayDuration = 0.3f;

	void Start() {
    filterColor = titleFilter.material.color;
    filterChanging = true;
    filterChangeTo = 0;
    stayCount = 0;

    characterColor = character.GetComponent<Renderer>().material.color;

    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));

    string[] rots = PlayerPrefs.GetString("CharacterRotation").Split(',');
    character.transform.rotation = Quaternion.Euler(float.Parse(rots[0]), float.Parse(rots[1]), float.Parse(rots[2]));

    string[] angVals = PlayerPrefs.GetString("CharacterAngVal").Split(',');
    character.GetComponent<Rigidbody>().angularVelocity = new Vector3(float.Parse(angVals[0]), float.Parse(angVals[1]), float.Parse(angVals[2]));
  }

  void Update() {
    if (filterChanging) {
      if (stayCount < stayDuration) {
        stayCount += Time.deltaTime;
      } else {
        filterColor.a = Mathf.MoveTowards(filterColor.a, filterChangeTo, Time.deltaTime / filterChangingDuration);
        titleFilter.material.color = filterColor;

        if (filterChangeTo == 0) {
          characterColor.a = Mathf.MoveTowards(characterColor.a, filterChangeTo, Time.deltaTime / filterChangingDuration);
          character.GetComponent<Renderer>().material.color = characterColor;
        }

        if (filterColor.a == filterChangeTo) {
          filterChanging = false;
          if (filterChangeTo == 0) {
            titleFilter.gameObject.SetActive(false);
            character.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            character.transform.localRotation = Quaternion.Euler(0, 90, 0);
            if (flyingCharacters != null) flyingCharacters.reset();
          }
        }
      }
    }

    if (titleMoving) {
      titlePosX = Mathf.MoveTowards(titlePosX, titleMoveTo, Time.deltaTime / movingDuration * distance);
      title.GetComponent<RectTransform>().anchoredPosition = new Vector2(titlePosX, title.GetComponent<RectTransform>().anchoredPosition.y);

      if (titleMoveTo == -characterMoveStart) {
        copyrightColor.a = Mathf.MoveTowards(copyrightColor.a, copyrightChangeTo, Time.deltaTime / movingDuration);
        copyright.color = copyrightColor;
      }

      if (titlePosX == titleMoveTo) {
        titleMoving = false;

        // if (copyright.color.a == 0) powerBoostUI.SetActive(true);
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
        copyrightColor.a = Mathf.MoveTowards(copyrightColor.a, copyrightChangeTo, Time.deltaTime / movingDuration);
        copyright.color = copyrightColor;

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
    }
  }

  public bool isLoading() {
    return filterChanging || characterMoving;
  }

  public void moveTitle(bool hiding = true) {
    if (hiding) {
      titleMoveTo = -characterMoveStart;
      titlePosX = title.GetComponent<RectTransform>().anchoredPosition.x;
      copyrightColor = copyright.color;
      copyrightChangeTo = 0;
    } else {
      titleMoveTo = 0;
      titlePosX = -title.GetComponent<RectTransform>().anchoredPosition.x;
    }
    titleMoving = true;
    distance = Mathf.Abs(titlePosX - titleMoveTo);

  }

  public void playAgain() {
    titleFilter.gameObject.SetActive(true);
    filterColor = titleFilter.material.color;
    filterChanging = true;
    filterChangeTo = 1;
    copyrightColor = copyright.color;
    copyrightChangeTo = 1;
    moveTitle(false);

    changeCharacter(PlayerPrefs.GetString("SelectedCharacter"));
    characterMoving = true;
    characterPosX = characterMoveStart;
    characterPosX = characterMoveStart;
    characterMoveDistance = Mathf.Abs(characterMoveStart);
    character.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    character.GetComponent<RectTransform>().anchoredPosition = new Vector2(characterPosX, character.GetComponent<RectTransform>().anchoredPosition.y);
  }

  void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    character.GetComponent<MeshFilter>().sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;
  }
}
