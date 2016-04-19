#pragma once


#include "Scene.hpp"
#include <list>
using namespace std;


//-----------------------------------------------------------------------------------
// 
// base level scene
// 
//-----------------------------------------------------------------------------------

class LevelScene : public Scene {
protected:

  list<Actor*> _actors;

  void delActors() {
    for (auto actor : _actors) {
      _game->destroy(actor);
    }
    _actors.clear();
  }

public:

  LevelScene(SceneId id, Game* game) : Scene(id, game) {};
  virtual ~LevelScene() {
    delActors();
  };

  virtual void init() {
    _clk.reset();
    _state = SCENE_LOADED;
  };
  virtual void destroy() {
    delActors();
  };

};


//-----------------------------------------------------------------------------------
// 
// level 1
// 
//-----------------------------------------------------------------------------------

class Level1Scene : public LevelScene {
protected:



public:

  Level1Scene(SceneId id, Game* game);
  ~Level1Scene();

  void init();
  void destroy();

  void logic();

};


//-----------------------------------------------------------------------------------
// 
// level 2
// 
//-----------------------------------------------------------------------------------

class Level2Scene : public LevelScene {
protected:


public:

  Level2Scene(SceneId id, Game* game);
  ~Level2Scene();

  void init();
  void destroy();

  void logic();

};


//-----------------------------------------------------------------------------------
// 
// level 3
// 
//-----------------------------------------------------------------------------------

class Level3Scene : public LevelScene {
protected:
  


public:

  Level3Scene(SceneId id, Game* game);
  ~Level3Scene();

  void init();
  void destroy();

  void logic();

};



