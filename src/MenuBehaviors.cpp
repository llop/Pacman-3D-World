#include "MenuBehaviors.hpp"



//-----------------------------------------------------------------------------------
// 
// Menu behaviors impl
// 
//-----------------------------------------------------------------------------------

MenuTitleBehavior::MenuTitleBehavior(MenuScene* menu) {
  _menu = menu;
  _selectedOption = MENU_OP_START;
}

MenuTitleBehavior::~MenuTitleBehavior() {

}

void MenuTitleBehavior::init() {

}

void MenuTitleBehavior::fixedUpdate() {
  auto mogl = actor().game().getPlugin<MultimediaOGL>();
  if (mogl->input().isKeyDown(sf::Keyboard::Down)) {
    _selectedOption = MenuOption((_selectedOption+1)%MENU_OP_SIZE);
  }
  if (mogl->input().isKeyDown(sf::Keyboard::Up)) {
    _selectedOption = MenuOption((_selectedOption-1+MENU_OP_SIZE)%MENU_OP_SIZE);
  }
}

void MenuTitleBehavior::onActivate() {

}

void MenuTitleBehavior::onDeactivate() {

}

void MenuTitleBehavior::onDestroy() {

}

