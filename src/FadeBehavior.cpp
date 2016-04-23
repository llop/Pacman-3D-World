#include "FadeBehavior.hpp"
#include "GameGlobals.hpp"


FadeBehavior::FadeBehavior(FadeType type, long duration, const sf::Color &color) :
  _type(type), _duration(duration), _color(color),
  _game(nullptr), _actor(nullptr) {
}

FadeBehavior::~FadeBehavior() {
  cleanup();
}


void FadeBehavior::init() {
  _actor = &actor();
  _game = &_actor->game();
  _mogl = _game->getPlugin<MultimediaOGL>();
  _cam = &_mogl->getCamera();
  _shader = _mogl->shaderPrograms().get("_mogl_plain");
  _clk.reset();

  float vert[12] = {
      0.             , 0.            ,
      WINDOW_WIDTH   , 0.            ,
      WINDOW_WIDTH   , WINDOW_HEIGHT ,
      0.             , 0.            ,
      WINDOW_WIDTH   , WINDOW_HEIGHT ,
      0.             , WINDOW_HEIGHT
  };

  glGenVertexArrays(1, &_vao);
  glBindVertexArray(_vao);
  glGenBuffers(1, &_vbo);
  glBindBuffer(GL_ARRAY_BUFFER, _vbo);
  glBufferData(GL_ARRAY_BUFFER, 12 * sizeof(float), vert, GL_STATIC_DRAW);
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindVertexArray(0);
  setShaderProgram(_shader);
  Drawable::init();
}

void FadeBehavior::onDestroy() {
  cleanup();
  Drawable::onDestroy();
  glDeleteBuffers(1, &_vbo);
  glDeleteVertexArrays(1, &_vao);
}

void FadeBehavior::setShaderProgram(ShaderProgram* shader) {
  Drawable::setShaderProgram(shader);
  glBindVertexArray(_vao);
  glBindBuffer(GL_ARRAY_BUFFER, _vbo);
  _pos = shaderProgram()->bindVertexAttribute("position", 2, 0, 0);
  glBindBuffer(GL_ARRAY_BUFFER, 0);
  glBindVertexArray(0);
}


float FadeBehavior::alpha() const {
  long t = _clk.getTime().asNanoseconds();
  float fac = float(t)/_duration;
  float a = 1;
  if (_type==FADE_IN) a = 1-fac;
  else if (_type==FADE_OUT) a = fac;
  //cout<<"alpfa "<<max(0.f, min(1.f, a))<<endl;
  return max(0.f, min(1.f, a));
}

void FadeBehavior::cleanup() {
  if (_actor!=nullptr) {
    _game->destroy(_actor);
    _actor = nullptr;
  }
  _game = nullptr;
}

void FadeBehavior::fixedUpdate() {
  long t = _clk.getTime().asNanoseconds();
  if (t>_duration) {
    cleanup();
    return;
  }
  //Drawable::fixedUpdate();
}

void FadeBehavior::draw() {
  transform().position.z = _cam->getPosition().z+1;

  glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA); 
  glEnable(GL_BLEND);

  glBindVertexArray(_vao);
  shaderProgram()->setUniform4f("color", _color.r/255.f, _color.g/255.f, _color.b/255.f, alpha());
  glEnableVertexAttribArray(_pos);
  glDrawArrays(GL_TRIANGLES, 0, 6);
  glBindVertexArray(0);

  glDisable(GL_BLEND);

}

const char* FadeBehavior::behaviorName() {
  return "pac::Fade";
}


