﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowStats : VerticalScrollingMenu {
  public Text bestHeading;
  public Text bestContents;
  public Text averageHeading;
  public Text averageContents;
  public Text totalHeading;
  public Text totalContents;

  public int spaceBetweenHeadingAndContents = 15;
  public int spaceBetweenCategories = 20;
  private float accumulatedHeight = 0;

  override protected void initRest () {
    foreach (Transform tr in scrollTarget.transform) {
      tr.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, tr.GetComponent<RectTransform>().sizeDelta.y);
    }

    fillText("Best", bestHeading, bestContents);
    fillText("Average", averageHeading, averageContents);
    fillText("Total", totalHeading, totalContents);

    scrollTarget.sizeDelta = new Vector2(newWidth, accumulatedHeight);
	}

  void fillText(string what, Text heading, Text contents) {
    heading.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - accumulatedHeight);
    accumulatedHeight += heading.preferredHeight + spaceBetweenHeadingAndContents;
    contents.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, - accumulatedHeight);

    string data = Resources.Load(what).ToString().Trim();
    string[] lines = data.Split('\n');
    foreach (string line in lines) {
      string[] lineData = line.Split(',');
      string value = getValue(lineData);
      contents.text += lineData[2] + ": " + value + "\n";
    }
    contents.text = contents.text.Trim();
    accumulatedHeight += contents.preferredHeight + spaceBetweenCategories;
    contents.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, contents.preferredHeight);
  }

  string getValue(string[] lineData) {
    string type = lineData[0];
    string variable = lineData[1];
    string value = "";
    if (type == "int") {
      value = DataManager.dm.getInt(variable).ToString();
    } else if (type == "float") {
      value = DataManager.dm.getFloat(variable).ToString("0.00");
    } else if (type == "DateTime") {
      value = DataManager.dm.getDateTime(variable).ToString();
    }

    return value;
  }
}
