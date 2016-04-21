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

  bool _active;
  MenuScene* _menu;
  MenuOption _selectedOption;
  long _lastKeyAccept;


public:

  MenuTitleBehavior(MenuScene* menu);
  ~MenuTitleBehavior();
  void init();
  void fixedUpdate();
  void onActivate();
  void onDeactivate();
  void onDestroy();

};


class MenuBackToTitleBehavior : public Behavior {
protected:

  bool _active;
  MenuScene* _menu;
  long _lastKeyAccept;

public:

  MenuBackToTitleBehavior(MenuScene* menu);
  ~MenuBackToTitleBehavior();
  void init();
  void fixedUpdate();
  void onActivate();
  void onDeactivate();
  void onDestroy();

};
