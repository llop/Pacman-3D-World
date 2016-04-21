#include "LevelScene.hpp"



Level3Scene::Level3Scene(SceneId id, Game* game) : LevelScene(id, game) {

}

Level3Scene::~Level3Scene() {
 
}

void Level3Scene::init() {
  LevelScene::init();

  // add a bunch of actors
  // + they colliders and behaviors?
  // the ground, walls, other shits on the map

  auto actor = _game->makeActor();
  actor->addBehavior<Rectangle>(5, 5, sf::Color::White);
  _actors.insert(_actors.end(), actor);
}

void Level3Scene::destroy() {
  LevelScene::destroy();
}

void Level3Scene::logic() {
  LevelScene::logic();

  if (_state < SCENE_CODA) {
    if (_clk.getTime().asNanoseconds()>3000000000) {
      signalDone();
    }
  }
}



