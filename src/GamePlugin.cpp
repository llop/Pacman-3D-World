#include "GamePlugin.hpp"
#include "MenuScene.hpp"



GamePlugin::GamePlugin() {
  _scene = nullptr;
}

GamePlugin::~GamePlugin() { 
  delScene();
}

void GamePlugin::gameStart() {
  // start off at the menu
  _scene = new MenuScene(SCENE_MENU, &game());
}


void GamePlugin::delScene() {
  if (_scene != nullptr) {
    _scene->destroy();
    delete _scene;
    _scene = nullptr;
  }
}

Scene* GamePlugin::nextScene() {
  SceneId sceneId = _scene->id();
  switch (sceneId) {
    case SCENE_MENU:
      return new MenuScene(SCENE_MENU, &game());
    case SCENE_LEVEL1:
      //return new Level1Scene(SCENE_LEVEL1, &game());
      return new MenuScene(SCENE_MENU, &game());
    case SCENE_LEVEL2:
      //return new Level2Scene(SCENE_LEVEL2, &game());
      return new MenuScene(SCENE_MENU, &game());
    case SCENE_LEVEL3:
      //return new Level3Scene(SCENE_LEVEL3, &game());
      return new MenuScene(SCENE_MENU, &game());
  }
  return nullptr;
}

void GamePlugin::scenePreUpdate() {

  // switch scenes if done with the current one
  if (_scene->state() == SCENE_DONE) {
    Scene* next = nextScene();
    delScene();
    _scene = next;
  }
  
  // load new scenes
  if (_scene->state() == SCENE_FRESH) {
    _scene->init();
  }

}

void GamePlugin::preUpdate() {

  scenePreUpdate();

}

void GamePlugin::preFixedUpdate() {

}

void GamePlugin::postFixedUpdate() {

}

void GamePlugin::postUpdate() {

}

void GamePlugin::gameEnd() {

}





