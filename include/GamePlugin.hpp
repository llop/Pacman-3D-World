#pragma once


#include "hummingbird/hum.hpp"
#include "Scene.hpp"


class GamePlugin : public hum::Plugin {
private:

  Scene* _scene;


  void delScene();
  Scene* nextScene();
  void scenePreUpdate();
  void scenePostUpdate();
  

public:

  GamePlugin();
  ~GamePlugin();

  void gameStart() override;
  void preUpdate() override;
  void preFixedUpdate() override;
  void postFixedUpdate() override;
  void postUpdate() override;
  void gameEnd() override;

};

