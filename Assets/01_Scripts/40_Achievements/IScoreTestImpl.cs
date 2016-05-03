using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System;

public class IScoreTestImpl : IScore {
  string leaderBoardId = "CgkIubjEkcMWEAIQBg";
  long valueTest = 0;
  string userIdTest = "";
  int rankTest = 0;
  
  public IScoreTestImpl(int score, int rank, string userIdTest) {
    this.valueTest = score;
    this.userIdTest = userIdTest;
    this.rankTest = rank;
  }

  public DateTime date {
    get {
      return DateTime.Now;
    }
  }

  public string formattedValue {
    get {
      return valueTest + "HO!";
    }
  }

  public string leaderboardID {
    get {
      return leaderBoardId;
    }

    set {
      leaderBoardId = value;
    }
  }

  public int rank {
    get {
      return rankTest;
    }
  }

  public string userID {
    get {
      return userIdTest;
    }
  }

  public long value {
    get {
      return valueTest;
    }

    set {
      valueTest = value;
    }
  }

  public void ReportScore(Action<bool> callback) {
    return;
  }
}
