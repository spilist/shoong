using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AbilityData;
using UnityEngine.Purchasing;

public class UICharacters : MonoBehaviour {
  public CharacterStat stat;
  private string productId;
  private Product bProduct;
  private string price;

  private CharactersMenu charactersMenu;
  private Vector3 originalPosition;
  private Quaternion originalRotation;
  private Vector3 originalScale;
  private float scaleChanging;
  private bool soundPlayed = false;

  void OnEnable() {
    if (charactersMenu == null) return;

    checkBought(false);
  }

  void Start () {
    charactersMenu = transform.parent.parent.GetComponent<CharactersMenu>();
    originalPosition = transform.localPosition;
    originalRotation = transform.localRotation;
    originalScale = transform.localScale;
    scaleChanging = transform.localScale.x;

    productId = "toy_" + name;
    bProduct = BillingManager.bm.getProduct(productId);

    checkBought(false);
	}

	void Update () {
    if (Mathf.Abs(transform.parent.localPosition.x - originalPosition.x) < charactersMenu.selectWidth) {
      select();
    } else {
      unselect();
    }
	}

  public void setRarity(Text text) {
    Rarity rarity = CharacterManager.cm.character(name).rarity;
    if (charactersMenu == null) {
      charactersMenu = transform.parent.parent.GetComponent<CharactersMenu>();
    }

    if (rarity == Rarity.Common) {
      text.text = "Common";
      text.color = charactersMenu.colorsPerRarity[0];
    } else if (rarity == Rarity.Rare) {
      text.text = "Rare";
      text.color = charactersMenu.colorsPerRarity[1];
    } else if (rarity == Rarity.Epic) {
      text.text = "Epic";
      text.color = charactersMenu.colorsPerRarity[2];
    } else if (rarity == Rarity.Legendary) {
      text.text = "Legendary";
      text.color = charactersMenu.colorsPerRarity[3];
    }
  }

  void select() {
    if (!soundPlayed) {
      soundPlayed = true;
      if (charactersMenu.isJustOpened()) {
        charactersMenu.setOpened();
      } else {
        charactersMenu.characterSelectSound.Play();
      }

      stat = CharacterManager.cm.character(name);

      charactersMenu.characterName.text = stat.characterName;
      setRarity(charactersMenu.rarity);

      charactersMenu.description.text = stat.skillName;
      if (stat.skillName != "") {
        charactersMenu.description.text += ": " + SkillManager.sm.getSkill(stat.skillCode()).description;
      }

      checkBought();
    }

    transform.localPosition = new Vector3(transform.parent.localPosition.x, charactersMenu.selectedOffset_y, charactersMenu.selectedOffset_z);

    transform.Rotate(-Vector3.up * Time.deltaTime * charactersMenu.selectedCharacterRotationSpeed);

    if (scaleChanging != originalScale.x * 2) {
      scaleChanging = Mathf.MoveTowards(scaleChanging, originalScale.x * 2, Time.deltaTime * charactersMenu.scaleChangingSpeed);
      transform.localScale = new Vector3(scaleChanging, scaleChanging, scaleChanging);
    }

    Transform aura = transform.Find("aura");
    if (aura != null) aura.gameObject.SetActive(true);
  }

  public void unselect() {
    soundPlayed = false;

    transform.localPosition = originalPosition;
    transform.localRotation = originalRotation;

    if (scaleChanging != originalScale.x) {
      scaleChanging = Mathf.MoveTowards(scaleChanging, originalScale.x, Time.deltaTime * charactersMenu.scaleChangingSpeed);
      transform.localScale = new Vector3(scaleChanging, scaleChanging, scaleChanging);
    }

    Transform aura = transform.Find("aura");
    if (aura != null) aura.gameObject.SetActive(false);
  }

  public void checkBought(bool buttons = true) {
    if (DataManager.dm.getBool(name)) {
      if (buttons) {
        charactersMenu.selectButton.gameObject.SetActive(true);
        charactersMenu.selectButton.setCharacter(name);
        charactersMenu.buyButton.gameObject.SetActive(false);
      }
      GetComponent<Renderer>().sharedMaterial = charactersMenu.activeCharactersMaterial;
    } else {

      if (buttons) {
        charactersMenu.buyButton.gameObject.SetActive(true);
        charactersMenu.selectButton.gameObject.SetActive(false);

        if (bProduct == null) {
          price = "NOT LOADED";
        } else {
          //Debug.Log("Price localization info: code:" + bProduct.metadata.isoCurrencyCode + ", localDesc:" + bProduct.metadata.localizedDescription + ", localPrice:" + 
          //  bProduct.metadata.localizedPrice + ", localPriceStr:" + bProduct.metadata.localizedPriceString + ", localTitle:" + bProduct.metadata.localizedTitle);
          price = bProduct.metadata.isoCurrencyCode + "\n" + bProduct.metadata.localizedPrice;
        }

        charactersMenu.buyButton.setCharacter(name, price);
      }
    }
  }

  public void buy() {
    BillingManager.bm.BuyProductID(productId);
    TrackingManager.tm.initiateCheckout(bProduct, stat.rarity.ToString());
  }

  public void buyComplete(string transactionId, bool bought) {
    if (bought) {
      GetComponent<Renderer>().sharedMaterial = charactersMenu.activeCharactersMaterial;
      charactersMenu.characterBuySound.Play();

      TrackingManager.tm.purchase(transactionId, bProduct, stat.rarity.ToString());
    }

    DataManager.dm.setBool(name, true);
    DataManager.dm.increment("NumCharactersHave");
    DataManager.dm.save();
  }
}
