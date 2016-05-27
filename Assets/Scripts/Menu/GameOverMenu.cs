using UnityEngine;
using System.Collections;



public class GameOverMenu : MonoBehaviour {

	
  private bool buttonPressable = true;


  // yes: go to last played level
  public void advance() {
    if (buttonPressable) {
      buttonPressable = false;
      GameManager.Instance.doContinueGame();
    }
  }

  // no: back to main menu
  public void quit() {
    if (buttonPressable) {
      buttonPressable = false;
      GameManager.Instance.transitionToScene(Tags.MenuScene);
    }
  }



}
