#include "LevelScene.hpp"
#include "FadeBehavior.hpp"
#include "LevelBehavior.hpp"


void LevelScene::signalDone() {
  auto actor = _game->makeActor();
  actor->addBehavior<FadeBehavior>(FADE_OUT, FADE_OUT_DURATION);

  _state = SCENE_CODA;
  _codaStart = _clk.getTime().asNanoseconds();
}

void LevelScene::delActors() {
  for (auto actor : _actors) {
    _game->destroy(actor);
  }
  _actors.clear();
}

LevelScene::LevelScene(SceneId id, Game* game) : Scene(id, game) {
 
}

LevelScene::~LevelScene() {
  delActors();
}



void LevelScene::loadLevel(const string &levelObjFile) {
  auto actor = _game->makeActor();
  actor->addBehavior<LevelBehavior>(levelObjFile);
  _actors.push_back(actor);
}

void LevelScene::init() {
  auto actor = _game->makeActor();
  actor->addBehavior<FadeBehavior>(FADE_IN, FADE_IN_DURATION);

  _clk.reset();
  _state = SCENE_LOADED;
}

void LevelScene::destroy() {
  delActors();
}

void LevelScene::logic() {
  if (_state == SCENE_CODA) {
    long t = _clk.getTime().asNanoseconds();
    long delta = t-_codaStart;
    if (delta>=CODA_DURATION) _state = SCENE_DONE;
  }
}



void LevelScene::applyPhysics() {

}

void LevelScene::checkCollisions() {

}




