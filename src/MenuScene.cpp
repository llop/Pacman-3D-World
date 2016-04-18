#include "MenuScene.hpp"
#include "MenuBehaviors.hpp"


MenuScene::MenuScene(SceneId id, Game* game) : Scene(id, game) {
  _screenAct = nullptr;
}

MenuScene::~MenuScene() {
  delScreenAct();
}


void MenuScene::init() { 
  _clk.reset();
  screen(MENU_SCR_TITLE);
  _state = SCENE_LOADED;
}

void MenuScene::destroy() {
  delScreenAct();
}


void MenuScene::delScreenAct() {
  if (_screenAct != nullptr) {
    _game->destroy(_screenAct);
    _screenAct = nullptr;
  }
}


void MenuScene::screen(MenuScreen scr) {

  _screen = scr;

  // fry previous actor and add new one
  delScreenAct();
  _screenAct = _game->makeActor();
  switch (_screen) {
    case MENU_SCR_TITLE:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Red);
      _screenAct->addBehavior<MenuTitleBehavior>(this);
      break;
    case MENU_SCR_CONTROLS:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Green);
      //_screenAct->addBehavior<MenuControlsBehavior>(this);
      break;
    case MENU_SCR_CREDITS:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Blue);
      //_screenAct->addBehavior<MenuCreditsBehavior>(this);
      break;
    case MENU_SCR_HI_SCORE:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Yellow);
      //_screenAct->addBehavior<MenuHiScoreBehavior>(this);
      break;
    default:
      // remove actor if we failed to set a behavior
      delScreenAct();
  }
  
}

void MenuScene::startGame() {
  _state = SCENE_DONE;
}

