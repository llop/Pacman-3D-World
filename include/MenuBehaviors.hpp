#pragma once


#include "MenuScene.hpp"


//-----------------------------------------------------------------------------------
// 
// Menu behaviors
// 
//-----------------------------------------------------------------------------------

// options: start game, controls, credits, hi-score
enum MenuOption {
  MENU_OP_START, MENU_OP_CONTROLS, MENU_OP_CREDITS, MENU_OP_HI_SCORE,
  MENU_OP_SIZE
};


class MenuTitleBehavior : public Behavior {
protected:

  MenuScene* _menu;
  MenuOption _selectedOption;

public:

  MenuTitleBehavior(MenuScene* menu);
  ~MenuTitleBehavior();
  void init();
  void fixedUpdate();
  void onActivate();
  void onDeactivate();
  void onDestroy();

};
