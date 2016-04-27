#include "Scene.hpp"


Scene::Scene(SceneId id, Game* game) {
  _id = id;
  _game = game;
  _mogl = _game->getPlugin<MultimediaOGL>();
  _state = SCENE_FRESH;
}

Scene::~Scene() {};
  
SceneId Scene::id() const { 
  return _id; 
}
SceneState Scene::state() const { 
  return _state; 
}

const Clock &Scene::clock() const { 
  return _clk; 
}

void Scene::init() {
}

void Scene::destroy() {
}

void Scene::logic() { 
}
