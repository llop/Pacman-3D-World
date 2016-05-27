using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class MenuManager : MonoBehaviour {



  public MenuAnimator currentMenu;



  public void Start() {
    showMenu(currentMenu);

    hiScoreName1Text.text = PlayerPrefs.GetString(Tags.HiScoreName1);
    hiScoreName2Text.text = PlayerPrefs.GetString(Tags.HiScoreName2);
    hiScoreName3Text.text = PlayerPrefs.GetString(Tags.HiScoreName3);
    hiScoreValue1Text.text = PlayerPrefs.GetString(Tags.HiScoreValue1);
    hiScoreValue2Text.text = PlayerPrefs.GetString(Tags.HiScoreValue2);
    hiScoreValue3Text.text = PlayerPrefs.GetString(Tags.HiScoreValue3);
  }


  // switch menus
  public void showMenu(MenuAnimator menu) {
    if (currentMenu != null) currentMenu.open = false;

    currentMenu = menu;
    currentMenu.open = true;
  }

  public void play() {
    GameManager.Instance.doStartGame();
  }



  public Text hiScoreName1Text;
  public Text hiScoreName2Text;
  public Text hiScoreName3Text;
  public Text hiScoreValue1Text;
  public Text hiScoreValue2Text;
  public Text hiScoreValue3Text;



}
