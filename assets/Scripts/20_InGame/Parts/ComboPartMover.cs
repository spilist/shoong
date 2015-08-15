﻿using UnityEngine;
using System.Collections;

public class ComboPartMover : ObjectsMover {
  ComboPartsManager cpm;

  override protected void initializeRest() {
    cpm = GameObject.Find("Field Objects").GetComponent<ComboPartsManager>();
    canBeMagnetized = false;
  }

  override public void destroyObject() {
    cpm.destroyInstances();
  }
}
