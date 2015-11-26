using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataValidator {
  private static Dictionary<string, byte[]> dataDict = new Dictionary<string, byte[]>();

  // Store data from DataManager
  /*
  public static void storeIntData (string id) {
    if (dataDict.ContainsKey(id))
      dataDict.Remove(id);
    int data = DataManager.dm.loadInt(id);
    if (data != -1)
      dataDict.Add(id, System.Text.Encoding.Unicode.GetBytes (data + ""));
  }
  */

  // Store given data
  public static void storeIntData (string id, int data) {
    if (dataDict.ContainsKey(id))
      dataDict.Remove(id);
    dataDict.Add(id, System.Text.Encoding.Unicode.GetBytes (data + ""));
  }

  public static bool validateIntData (string id, int data) {
    // If the stored data to compare does not exist, just return true
    if (dataDict.ContainsKey(id) == false)
      return true;

    if (data != getStoredIntData(id)) {
      // TODO: Send facebook event
      NPBinding.UI.ShowToast("Hacking attempt to " + id + " is detected!!, stored: " + getStoredIntData(id) + ", " + data,
                             VoxelBusters.NativePlugins.eToastMessageLength.LONG);
      return false;
    } else {
      return true;
    }
  }

  public static int getStoredIntData (string id) {
    if (dataDict.ContainsKey(id) == false)
      return -1;

    return int.Parse(System.Text.Encoding.Unicode.GetString (dataDict[id]));
  }
}
