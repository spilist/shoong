﻿using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {
  public static GameController control;

  public int numPlays;
  public int numBoosters;

  public Hashtable cubes;
  public Hashtable cubes_by;
  public Hashtable goldenCubes;
  public Hashtable times;
  public Hashtable num_deaths_by;
  public Hashtable num_destroys;
  public Hashtable num_use_objects;

  private string datapath;

	void Start () {
    if (control == null) {
      DontDestroyOnLoad(gameObject);
      control = this;
      datapath = Application.persistentDataPath + "/GameData.dat";

      cubes = new Hashtable();
      cubes_by = new Hashtable();
      goldenCubes = new Hashtable();
      times = new Hashtable();
      num_deaths_by = new Hashtable();
      num_destroys = new Hashtable();
      num_use_objects = new Hashtable();
      load();
      // reset();
    } else if (control != this) {
      Destroy(gameObject);
    }
	}

  public void save() {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(datapath);

    PlayerData data = new PlayerData();

    data.numPlays = numPlays;
    data.numBoosters = numBoosters;
    data.cubes = cubes;
    data.cubes_by = cubes_by;
    data.goldenCubes = goldenCubes;
    data.times = times;
    data.num_deaths_by = num_deaths_by;
    data.num_destroys = num_destroys;
    data.num_use_objects = num_use_objects;

    bf.Serialize(file, data);
    file.Close();
  }

  public void load() {
    if (File.Exists(datapath)) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(datapath, FileMode.Open);
      PlayerData data = (PlayerData) bf.Deserialize(file);
      file.Close();

      numPlays = data.numPlays;
      numBoosters = data.numBoosters;
      cubes = data.cubes;
      cubes_by = data.cubes_by;
      goldenCubes = data.goldenCubes;
      times = data.times;
      num_deaths_by = data.num_deaths_by;
      num_destroys = data.num_destroys;
      num_use_objects = data.num_use_objects;
    } else {
      reset();
    }
  }

  void reset() {
    File.Delete(datapath);
    numPlays = 0;
    numBoosters = 0;

    cubes.Add("now", 0);
    cubes.Add("used", 0);
    cubes.Add("total", 0);
    cubes.Add("highscore", 0);

    cubes_by.Add("part", 0);
    cubes_by.Add("comboPart", 0);
    cubes_by.Add("destroying_obstacle", 0);
    cubes_by.Add("destroying_monster", 0);
    cubes_by.Add("cubeDispenser", 0);

    goldenCubes.Add("now", 0);
    goldenCubes.Add("used", 0);
    goldenCubes.Add("total", 0);

    times.Add("total", 0);
    times.Add("highscore", 0);

    num_deaths_by.Add("no_energy", 0);
    num_deaths_by.Add("target_obstacle", 0);
    num_deaths_by.Add("big_obstacle", 0);
    num_deaths_by.Add("blackhole", 0);
    num_deaths_by.Add("monster", 0);

    num_destroys.Add("blackhole", 0);
    num_destroys.Add("monster", 0);
    num_destroys.Add("obstacle", 0);

    num_use_objects.Add("exit_blackhole", 0);
    num_use_objects.Add("rebound_by_blackhole", 0);
    num_use_objects.Add("absorb_monster", 0);
    num_use_objects.Add("ride_weakened_monster", 0);
    num_use_objects.Add("unstoppable", 0);
    num_use_objects.Add("unstoppable_with_absorb", 0);
    num_use_objects.Add("unstoppable_with_monster", 0);
    num_use_objects.Add("unstoppable_with_absorb_and_monster", 0);
    num_use_objects.Add("absorb_with_monster", 0);
    num_use_objects.Add("combopart_maxcombo", 0);
    num_use_objects.Add("cubedispenser_maxcombo", 0);
  }
}

[Serializable]
class PlayerData {
  public int numPlays;
  public int numBoosters;
  public Hashtable cubes;
  public Hashtable cubes_by;
  public Hashtable goldenCubes;
  public Hashtable times;
  public Hashtable num_deaths_by;
  public Hashtable num_destroys;
  public Hashtable num_use_objects;
}
