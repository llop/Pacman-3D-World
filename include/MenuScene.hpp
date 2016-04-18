#pragma once


#include "Scene.hpp"


//-----------------------------------------------------------------------------------
// 
// Menu manager
// 
//-----------------------------------------------------------------------------------

// basic set of menu screens
enum MenuScreen {
  MENU_SCR_TITLE, MENU_SCR_CONTROLS, MENU_SCR_CREDITS, MENU_SCR_HI_SCORE
};


class MenuScene : public Scene {
protected:
  
  MenuScreen _screen;
  Actor* _screenAct;

  void delScreenAct();

public:

  MenuScene(SceneId id, Game* game);
  ~MenuScene();

  void init();
  void destroy();

  void screen(MenuScreen scr);
  void startGame();

};



