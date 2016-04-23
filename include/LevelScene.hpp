#pragma once


#include "Scene.hpp"
#include <list>
using namespace std;


#define FADE_IN_DURATION 1000000000
#define FADE_OUT_DURATION 1000000000

#define CODA_DURATION FADE_OUT_DURATION


//-----------------------------------------------------------------------------------
// 
// base level scene
// 
//-----------------------------------------------------------------------------------

class LevelScene : public Scene {
protected:

  list<Actor*> _actors;

  long _codaStart;

  virtual void signalDone();
  virtual void delActors();
  virtual void loadLevel(const string &levelObjFile);

public:

  LevelScene(SceneId id, Game* game);
  virtual ~LevelScene();

  virtual void init();
  virtual void destroy();

  virtual void logic();
  
  virtual void applyPhysics();
  virtual void checkCollisions();

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



