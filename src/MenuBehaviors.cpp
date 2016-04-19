#include "MenuBehaviors.hpp"



//-----------------------------------------------------------------------------------
// 
// Menu behaviors impl
// 
//-----------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------
// title screen behavior
//-----------------------------------------------------------------------------------

MenuTitleBehavior::MenuTitleBehavior(MenuScene* menu) {
  _menu = menu;
  _selectedOption = MENU_OP_START;
  _lastKeyAccept = _menu->clock().getTime().asNanoseconds();
}

MenuTitleBehavior::~MenuTitleBehavior() {

}
#include <iostream>
using namespace std;

void MenuTitleBehavior::init() {
  _lastKeyAccept = _menu->clock().getTime().asNanoseconds();
}



// just handles keyboard input for now
void MenuTitleBehavior::fixedUpdate() {

  long now = _menu->clock().getTime().asNanoseconds();
  long delta = now - _lastKeyAccept;
  if (delta < MENU_KEY_ACCEPT_DELAY_NANOS) return;

  bool keyPressed = false;

  // move thru the different options
  auto mogl = actor().game().getPlugin<MultimediaOGL>();
  if (mogl->input().isKeyDown(sf::Keyboard::Down)) {
    keyPressed = true;
    _selectedOption = MenuOption((_selectedOption+1)%MENU_OP_SIZE);
  }
  if (mogl->input().isKeyDown(sf::Keyboard::Up)) {
    keyPressed = true;
    _selectedOption = MenuOption((_selectedOption-1+MENU_OP_SIZE)%MENU_OP_SIZE);
  }

  // select one've 'em
  if (mogl->input().isKeyDown(sf::Keyboard::Return) ||
      mogl->input().isKeyDown(sf::Keyboard::Space)) {
    keyPressed = true;
    switch (_selectedOption) {
      case MENU_OP_START:
        _menu->startGame();
        break;
      case MENU_OP_CONTROLS:
        _menu->screen(MENU_SCR_CONTROLS);
        break;
      case MENU_OP_CREDITS:
        _menu->screen(MENU_SCR_CREDITS);
        break;
      case MENU_OP_HI_SCORE:
        _menu->screen(MENU_SCR_HI_SCORE);
        break;
      default:
        // just let it go
        break;
    }
  }

  if (keyPressed) {
    _lastKeyAccept = now;
  }
}

void MenuTitleBehavior::onActivate() {

}

void MenuTitleBehavior::onDeactivate() {

}

void MenuTitleBehavior::onDestroy() {

}


//-----------------------------------------------------------------------------------
// any-screen-that-returns-to-the-title-screen behavior
//-----------------------------------------------------------------------------------

MenuBackToTitleBehavior::MenuBackToTitleBehavior(MenuScene* menu) {
  _menu = menu;
  _lastKeyAccept = _menu->clock().getTime().asNanoseconds();
}

MenuBackToTitleBehavior::~MenuBackToTitleBehavior() {

}

void MenuBackToTitleBehavior::init() {
  _lastKeyAccept = _menu->clock().getTime().asNanoseconds();
}

void MenuBackToTitleBehavior::fixedUpdate() {
  long now = _menu->clock().getTime().asNanoseconds();
  long delta = now - _lastKeyAccept;
  if (delta < MENU_KEY_ACCEPT_DELAY_NANOS) return;

  auto mogl = actor().game().getPlugin<MultimediaOGL>();
  if (mogl->input().isKeyDown(sf::Keyboard::Return) ||
      mogl->input().isKeyDown(sf::Keyboard::Space)) {
    _menu->screen(MENU_SCR_TITLE);
  }
}

void MenuBackToTitleBehavior::onActivate() {

}

void MenuBackToTitleBehavior::onDeactivate() {

}

void MenuBackToTitleBehavior::onDestroy() {

}
















