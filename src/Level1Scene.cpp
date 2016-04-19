#include "LevelScene.hpp"



Level1Scene::Level1Scene(SceneId id, Game* game) : LevelScene(id, game) {

}

Level1Scene::~Level1Scene() {
 
}

void Level1Scene::init() {
  LevelScene::init();

  // add a bunch of actors
  // + they colliders and behaviors?
  // the ground, walls, other shits on the map

  auto actor = _game->makeActor();
  actor->addBehavior<Rectangle>(5, 5, sf::Color::Cyan);
  _actors.insert(_actors.end(), actor);
}

void Level1Scene::destroy() {
  LevelScene::destroy();
}

void Level1Scene::logic() {
  if (_clk.getTime().asNanoseconds()>1000000000) {
    _state = SCENE_DONE;
  }
}



