#include "LevelScene.hpp"



Level1Scene::Level1Scene(SceneId id, Game* game) : LevelScene(id, game) {

}

Level1Scene::~Level1Scene() {
 
}

void Level1Scene::init() {
  loadLevel("bench.obj");
  LevelScene::init();
}

void Level1Scene::destroy() {
  LevelScene::destroy();
}

void Level1Scene::logic() {
  LevelScene::logic();

  if (_state < SCENE_CODA) {
    if (_clk.getTime().asNanoseconds()>3000000000) {
      signalDone();
    }
  }
}



