#pragma once


#include "hummingbird/hum.hpp"
#include "MOGL/MOGL.hpp"
using namespace hum;
using namespace mogl;


enum SceneId {
  SCENE_MENU, SCENE_LEVEL1, SCENE_LEVEL2, SCENE_LEVEL3
};

enum SceneState {
  SCENE_FRESH, SCENE_LOADED, SCENE_DONE
};


class Scene {
protected:

  Game* _game;
  MultimediaOGL* _mogl;

  SceneId _id;
  SceneState _state;
  
  Clock _clk;

public:

  Scene(SceneId id, Game* game) {
    _id = id;
    _game = game;
    _mogl = _game->getPlugin<MultimediaOGL>();
    _state = SCENE_FRESH;
  }
  virtual ~Scene() {};
  
  virtual SceneId id() const { return _id; }
  virtual SceneState state() const { return _state; }


  virtual void init() = 0;
  virtual void destroy() = 0;

};


