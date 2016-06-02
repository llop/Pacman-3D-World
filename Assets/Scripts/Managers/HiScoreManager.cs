using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class HiScoreManager : MonoBehaviour {


  //-------------------------------------------------------------------
  // hi-score manager singleton
  //-------------------------------------------------------------------

  private static HiScoreManager _instance;

  public static HiScoreManager Instance { 
    get {
      if (_instance == null) {
        GameObject obj = new GameObject("HiScoreManager");
        DontDestroyOnLoad(obj);
        obj.AddComponent<HiScoreManager>();
      }
      return _instance;
    }
  }


  //-------------------------------------------------------------------
  // hi-scores list
  //-------------------------------------------------------------------

  private List<KeyValuePair<string, ulong>> _hiScores;
  public List<KeyValuePair<string, ulong>> hiScores {
    get { return _hiScores; }
  }



  //-------------------------------------------------------------------
  // init hi scores list from PlayerPrefs
  //-------------------------------------------------------------------

  private void debugInit() {
    PlayerPrefs.SetString(Tags.HiScoreName1, "aaa");
    PlayerPrefs.SetString(Tags.HiScoreValue1, "1000000");

    PlayerPrefs.SetString(Tags.HiScoreName2, "bbb");
    PlayerPrefs.SetString(Tags.HiScoreValue2, "500000");

    PlayerPrefs.SetString(Tags.HiScoreName3, "ccc");
    PlayerPrefs.SetString(Tags.HiScoreValue3, "0");
  }

  public void Awake() {
    //debugInit();

    _instance = this;
    _hiScores = new List<KeyValuePair<string, ulong>>();
    _hiScores.Add(new KeyValuePair<string, ulong>(
      PlayerPrefs.GetString(Tags.HiScoreName1, "aaa"),
      System.Convert.ToUInt64(PlayerPrefs.GetString(Tags.HiScoreValue1, "1000000"))
    ));
    _hiScores.Add(new KeyValuePair<string, ulong>(
      PlayerPrefs.GetString(Tags.HiScoreName2, "bbb"),
      System.Convert.ToUInt64(PlayerPrefs.GetString(Tags.HiScoreValue2, "500000"))
    ));
    _hiScores.Add(new KeyValuePair<string, ulong>(
      PlayerPrefs.GetString(Tags.HiScoreName3, "ccc"),
      System.Convert.ToUInt64(PlayerPrefs.GetString(Tags.HiScoreValue3, "10000"))
    ));
    _hiScores.OrderByDescending(entry => entry.Value);
  }



  //-------------------------------------------------------------------
  // do stuff with scores
  //-------------------------------------------------------------------

  public bool isHiScore(ulong score) {
    return score > _hiScores[2].Value;
  }


  private void storeHiScores() {
    PlayerPrefs.SetString(Tags.HiScoreName1, _hiScores[0].Key);
    PlayerPrefs.SetString(Tags.HiScoreValue1, _hiScores[0].Value.ToString());
    PlayerPrefs.SetString(Tags.HiScoreName2, _hiScores[1].Key);
    PlayerPrefs.SetString(Tags.HiScoreValue2, _hiScores[1].Value.ToString());
    PlayerPrefs.SetString(Tags.HiScoreName3, _hiScores[2].Key);
    PlayerPrefs.SetString(Tags.HiScoreValue3, _hiScores[2].Value.ToString());
  }

  public void setHiScore(string playerName, ulong playerScore) {
    if (!isHiScore(playerScore)) return;

    _hiScores.Add(new KeyValuePair<string, ulong>(playerName, playerScore));
    _hiScores = _hiScores.OrderByDescending(entry => entry.Value).ToList();
    _hiScores.RemoveAt(_hiScores.Count - 1);

    storeHiScores();
  }


}
