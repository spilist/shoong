using UnityEngine;
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

  public Hashtable characters;
  public Hashtable objects;

  private string datapath;

  public bool openAllCharacter = false;
  public bool resetAll = false;
  public int resetCube = 100000;
  public int resetGoldenCube = 5000;

	void OnEnable () {
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

      characters = new Hashtable();
      objects = new Hashtable();

      if (resetAll) reset();
      else load();
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
    data.characters = characters;
    data.objects = objects;

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
      characters = data.characters;
      objects = data.objects;
    } else {
      reset();
    }

    if (openAllCharacter) {
      Hashtable table = new Hashtable();
      foreach (DictionaryEntry pair in characters) {
        table.Add(pair.Key, true);
      }
      characters = table;
    }
  }

  void reset() {
    File.Delete(datapath);

    PlayerPrefs.SetString("SelectedCharacter", "robotcogi");
    PlayerPrefs.SetString("MainObjects", "");
    PlayerPrefs.SetString("SubObjects", "");

    PlayerPrefs.SetString("robotcogi", "로봇코기");
    PlayerPrefs.SetString("minimonster", "미니몬");
    PlayerPrefs.SetString("vacuumrobot", "청소로봇");
    PlayerPrefs.SetString("soju", "쐬주");
    PlayerPrefs.SetString("leonplant", "레옹화분");
    PlayerPrefs.SetString("deathstar", "데쓰스타");
    PlayerPrefs.SetString("crab", "오염된 게");
    PlayerPrefs.SetString("chameleon", "모로고로");
    PlayerPrefs.SetString("cat", "슈퍼캣");
    PlayerPrefs.SetString("butterfly", "나비");
    PlayerPrefs.SetString("bender", "벤더");

    numPlays = 0;
    numBoosters = 0;

    cubes.Add("now", resetCube);
    cubes.Add("used", 0);
    cubes.Add("total", resetCube);
    cubes.Add("highscore", 0);

    cubes_by.Add("part", 0);
    cubes_by.Add("comboPart", 0);
    cubes_by.Add("destroying_obstacle", 0);
    cubes_by.Add("destroying_monster", 0);
    cubes_by.Add("cubeDispenser", 0);

    goldenCubes.Add("now", resetGoldenCube);
    goldenCubes.Add("used", 0);
    goldenCubes.Add("total", resetGoldenCube);

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
    num_use_objects.Add("RainbowDonuts", 0);

    characters.Add("robotcogi", true);
    characters.Add("minimonster", false);
    characters.Add("vacuumrobot", false);
    characters.Add("soju", false);
    characters.Add("leonplant", false);
    characters.Add("deathstar", false);
    characters.Add("crab", false);
    characters.Add("chameleon", false);
    characters.Add("cat", false);
    characters.Add("butterfly", false);
    characters.Add("bender", false);
    characters.Add("beardedfrog", false);
    characters.Add("cottoncandy", false);
    characters.Add("tyranno", false);
    characters.Add("paperplane", false);

    objects.Add("SpecialParts", false);
    objects.Add("Blackhole", false);
    objects.Add("Monster", false);
    objects.Add("CubeDispenser", false);
    objects.Add("ComboParts", false);
    objects.Add("RainbowDonuts", false);
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

  public Hashtable characters;
  public Hashtable objects;
}
