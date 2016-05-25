using UnityEngine;
using System.Collections;



public class MenuManager : MonoBehaviour {



  public MenuAnimator currentMenu;



  public void Start() {
    showMenu(currentMenu);
  }


  // switch menus
  public void showMenu(MenuAnimator menu) {
    if (currentMenu != null) currentMenu.open = false;

    currentMenu = menu;
    currentMenu.open = true;
  }

  public void play() {
    GameManager.Instance.transitionToScene(Tags.Scene1);
  }



}
