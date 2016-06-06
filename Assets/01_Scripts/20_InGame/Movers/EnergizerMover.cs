using UnityEngine;
using System.Collections;

public class EnergizerMover : ObjectsMover {
  EnergizerManager egm;

  override public string getManager() {
    return "EnergizerManager";
  }

  protected override void initializeRest() {
    canBeMagnetized = false;
    egm = (EnergizerManager)objectsManager;
  }
}
