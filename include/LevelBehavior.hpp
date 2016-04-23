#pragma once


#include "GameGlobals.hpp"

#include "assimp/Importer.hpp"
#include "assimp/postprocess.h"
#include "assimp/scene.h"


// mesh
struct MyMesh {
  GLuint vao;
  GLuint texIndex;
  GLuint uniformBlockIndex;
  int numFaces;
};

// shader uniform block
struct MyMaterial {
  float diffuse[4];
  float ambient[4];
  float specular[4];
  float emissive[4];
  float shininess;
  int texCount;
};


class LevelBehavior : public Drawable {
protected:

  Game* _game;
  Actor* _actor;
  MultimediaOGL* _mogl;
  Camera* _cam;
  ShaderProgram* _shader;


  // Model Matrix (part of the OpenGL Model View Matrix)
  float modelMatrix[16];

  // For push and pop matrix
  vector<float *> matrixStack;

  // Vertex Attribute Locations
  GLuint vertexLoc=0, normalLoc=1, texCoordLoc=2;

  // Uniform Bindings Points
  GLuint matricesUniLoc = 1, materialUniLoc = 2;

  // The sampler uniform for textured models
  // we are assuming a single texture so this will
  //always be texture unit 0
  GLuint texUnit = 0;


  // Create an instance of the Importer class
  Assimp::Importer _importer;

  // the global Assimp scene object
  string _levelObjFile;
  const aiScene* _scene = NULL;

  // scale factor for the model to fit in the window
  float scaleFactor;

  vector<MyMesh> myMeshes;

  // images / texture
  // map image filenames to textureIds
  // pointer to texture Array
  map<string, GLuint> textureIdMap; 


  void set_float4(float f[4], float a, float b, float c, float d);
  void color4_to_float4(const aiColor4D *c, float f[4]);

  #define aisgl_min(x,y) (x<y?x:y)
  #define aisgl_max(x,y) (y>x?y:x)
  void get_bounding_box_for_node(const aiNode* nd, aiVector3D* min, aiVector3D* max);
  void get_bounding_box(aiVector3D* min, aiVector3D* max);

  void genVaos();

  void setIdentityMatrix(float *mat, int size);
  void multMatrix(float *a, float *b);
  void pushMatrix();
  void popMatrix();
  void setModelMatrix();
  void recursive_render(const aiScene *sc, const aiNode* nd);

  void cleanup();

public:


  LevelBehavior(const string &levelObjFile);
  ~LevelBehavior();

  void init() override;
  void onDestroy() override;

  void setShaderProgram(ShaderProgram* shader) override;
  void draw() override;

  static const char* behaviorName();


};


