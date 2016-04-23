#pragma once


#include "GameGlobals.hpp"


enum SceneId {
  SCENE_MENU, SCENE_LEVEL1, SCENE_LEVEL2, SCENE_LEVEL3
};

enum SceneState {
  SCENE_FRESH, SCENE_LOADED, SCENE_CODA, SCENE_DONE
};


class Scene {
protected:

  Game* _game;
  MultimediaOGL* _mogl;

  SceneId _id;
  SceneState _state;
  
  Clock _clk;


public:

  Scene(SceneId id, Game* game);
  virtual ~Scene();
  
  virtual SceneId id() const;
  virtual SceneState state() const;
  virtual const Clock &clock() const;


  virtual void init();
  virtual void destroy();

  virtual void logic();

};


