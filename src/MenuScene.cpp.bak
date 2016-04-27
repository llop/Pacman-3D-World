#include "MenuScene.hpp"
#include "MenuBehaviors.hpp"


MenuScene::MenuScene(SceneId id, Game* game) : Scene(id, game) { 
  _screenAct = nullptr;
}

MenuScene::~MenuScene() {
  delScreenAct();  // Al cerrar la app: Intentando eliminar un actor inexistente
}

void MenuScene::init() {
  _clk.reset();
  _state = SCENE_LOADED;
  screen(MENU_SCR_TITLE); 
}

void MenuScene::destroy() {
  delScreenAct();  // Al cerrar la app: Intentando eliminar un actor inexistente
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
      // TODO: set behaviors to render an honest to goodness screen
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Red);
      _screenAct->addBehavior<MenuTitleBehavior>(this);
      break;
    case MENU_SCR_CONTROLS:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Green);
      _screenAct->addBehavior<MenuBackToTitleBehavior>(this);
      break;
    case MENU_SCR_CREDITS:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Blue);
      _screenAct->addBehavior<MenuBackToTitleBehavior>(this);
      break;
    case MENU_SCR_HI_SCORE:
      _screenAct->addBehavior<Rectangle>(5, 5, sf::Color::Yellow);
      _screenAct->addBehavior<MenuBackToTitleBehavior>(this);
      break;
    default:
      // remove actor if we failed to set a behavior
      delScreenAct();
      break;
  }
}

void MenuScene::startGame() {
  _state = SCENE_DONE;
}


void MenuScene::logic() {

}
