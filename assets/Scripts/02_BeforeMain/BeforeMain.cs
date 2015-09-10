using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeforeMain : MonoBehaviour {
  public Text copyright;
  public Text tips;
  public float tipShowAfter = 0.3f;
  public float tipAlphaChangeDuration = 0.2f;
  private float tipHideCount = 0;
  private Color tipColor;

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

    tipColor = new Color(1, 1, 1, 0);

    Tip[] availableTips = new Tip[tips.transform.childCount];
    int availableTipsCount = 0;
    foreach (Transform tr in tips.transform) {
      Tip tip = tr.GetComponent<Tip>();
      if (tip.isAvailable()) {
        availableTips[availableTipsCount++] = tip;
      }
    }

    Tip selected = availableTips[Random.Range(0, availableTipsCount)];
    tips.text = selected.description;
    DataManager.dm.setInt("LastTipIndex", int.Parse(selected.name));
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
          if (boosterStayCount < boosterStayDuration) {
            boosterStayCount += Time.deltaTime;
          } else {
            characterMoving = false;
            Application.LoadLevel("5_Main");
          }
        }
      }

      if (tipHideCount < tipShowAfter) {
        tipHideCount += Time.deltaTime;
      } else {
        tipColor.a = Mathf.MoveTowards(tipColor.a, 1, Time.deltaTime / tipAlphaChangeDuration);
        tips.color = tipColor;
        copyright.color = tipColor;
      }
    }
	}

  void changeCharacter(string characterName) {
    GameObject play_characters = Resources.Load<GameObject>("_characters/play_characters");
    character.GetComponent<MeshFilter>().sharedMesh = play_characters.transform.FindChild(characterName).GetComponent<MeshFilter>().sharedMesh;
  }
}
