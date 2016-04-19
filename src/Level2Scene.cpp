#include "LevelScene.hpp"



Level2Scene::Level2Scene(SceneId id, Game* game) : LevelScene(id, game) {

}

Level2Scene::~Level2Scene() {
 
}

void Level2Scene::init() {
  LevelScene::init();

  // add a bunch of actors
  // + they colliders and behaviors?
  // the ground, walls, other shits on the map

  auto actor = _game->makeActor();
  actor->addBehavior<Rectangle>(5, 5, sf::Color::Magenta);
  _actors.insert(_actors.end(), actor);
}

void Level2Scene::destroy() {
  LevelScene::destroy();
}

void Level2Scene::logic() {
  if (_clk.getTime().asNanoseconds()>1000000000) {
    _state = SCENE_DONE;
  }
}



