#pragma once


#include "hummingbird/hum.hpp"
#include "MOGL/MOGL.hpp"
using namespace hum;
using namespace mogl;


enum FadeType {
  FADE_IN, FADE_OUT
};


class FadeBehavior : public Drawable {
protected:

  FadeType _type;
  long _duration;
  Clock _clk;

  GLuint _vao;
  GLuint _vbo;
  GLuint _pos;

  sf::Color _color;

  Game* _game;
  Actor* _actor;
  MultimediaOGL* _mogl;
  Camera* _cam;
  ShaderProgram* _shader;

  float alpha() const;

  void cleanup();

public:

  FadeBehavior(FadeType type, long duration, const sf::Color &color = sf::Color::Black);
  ~FadeBehavior();

  void init() override;
  void fixedUpdate() override;
  void onDestroy() override;

  void setShaderProgram(ShaderProgram* shader) override;
  void draw() override;

  static const char* behaviorName();

};

