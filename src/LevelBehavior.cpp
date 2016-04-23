#include "LevelBehavior.hpp"


LevelBehavior::LevelBehavior(const string &levelObjFile) : 
  _levelObjFile(levelObjFile) {

}

LevelBehavior::~LevelBehavior() {
  cleanup();
}

void LevelBehavior::set_float4(float f[4], float a, float b, float c, float d) {
  f[0] = a;
  f[1] = b;
  f[2] = c;
  f[3] = d;
}

void LevelBehavior::color4_to_float4(const aiColor4D *c, float f[4]) {
  f[0] = c->r;
  f[1] = c->g;
  f[2] = c->b;
  f[3] = c->a;
}

void LevelBehavior::get_bounding_box_for_node (const aiNode* nd, aiVector3D* min, aiVector3D* max) {
  aiMatrix4x4 prev;
  unsigned int n = 0, t;

  for (; n < nd->mNumMeshes; ++n) {
    const aiMesh* mesh = _scene->mMeshes[nd->mMeshes[n]];
    for (t = 0; t < mesh->mNumVertices; ++t) {

      aiVector3D tmp = mesh->mVertices[t];

      min->x = aisgl_min(min->x,tmp.x);
      min->y = aisgl_min(min->y,tmp.y);
      min->z = aisgl_min(min->z,tmp.z);

      max->x = aisgl_max(max->x,tmp.x);
      max->y = aisgl_max(max->y,tmp.y);
      max->z = aisgl_max(max->z,tmp.z);
    }
  }

  for (n = 0; n < nd->mNumChildren; ++n) {
    get_bounding_box_for_node(nd->mChildren[n],min,max);
  }
}

void LevelBehavior::get_bounding_box(aiVector3D* min, aiVector3D* max) {
  min->x = min->y = min->z =  1e10f;
  max->x = max->y = max->z = -1e10f;
  get_bounding_box_for_node(_scene->mRootNode,min,max);
}

void LevelBehavior::genVaos() {
  struct MyMesh aMesh;
  struct MyMaterial aMat; 
  GLuint buffer;
  
  // For each mesh
  for (unsigned int n = 0; n < _scene->mNumMeshes; ++n) {
    const aiMesh* mesh = _scene->mMeshes[n];

    // create array with faces
    // have to convert from Assimp format to array
    unsigned int *faceArray;
    faceArray = (unsigned int *)malloc(sizeof(unsigned int) * mesh->mNumFaces * 3);
    unsigned int faceIndex = 0;

    for (unsigned int t = 0; t < mesh->mNumFaces; ++t) {
      const aiFace* face = &mesh->mFaces[t];

      memcpy(&faceArray[faceIndex], face->mIndices,3 * sizeof(unsigned int));
      faceIndex += 3;
    }
    aMesh.numFaces = _scene->mMeshes[n]->mNumFaces;

    // generate Vertex Array for mesh
    glGenVertexArrays(1,&(aMesh.vao));
    glBindVertexArray(aMesh.vao);

    // buffer for faces
    glGenBuffers(1, &buffer);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, buffer);
    glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(unsigned int) * mesh->mNumFaces * 3, faceArray, GL_STATIC_DRAW);

    // buffer for vertex positions
    if (mesh->HasPositions()) {
      glGenBuffers(1, &buffer);
      glBindBuffer(GL_ARRAY_BUFFER, buffer);
      glBufferData(GL_ARRAY_BUFFER, sizeof(float)*3*mesh->mNumVertices, mesh->mVertices, GL_STATIC_DRAW);
      glEnableVertexAttribArray(vertexLoc);
      glVertexAttribPointer(vertexLoc, 3, GL_FLOAT, 0, 0, 0);
    }

    // buffer for vertex normals
    if (mesh->HasNormals()) {
      glGenBuffers(1, &buffer);
      glBindBuffer(GL_ARRAY_BUFFER, buffer);
      glBufferData(GL_ARRAY_BUFFER, sizeof(float)*3*mesh->mNumVertices, mesh->mNormals, GL_STATIC_DRAW);
      glEnableVertexAttribArray(normalLoc);
      glVertexAttribPointer(normalLoc, 3, GL_FLOAT, 0, 0, 0);
    }

    // buffer for vertex texture coordinates
    if (mesh->HasTextureCoords(0)) {
      float *texCoords = (float *)malloc(sizeof(float)*2*mesh->mNumVertices);
      for (unsigned int k = 0; k < mesh->mNumVertices; ++k) {

        texCoords[k*2]   = mesh->mTextureCoords[0][k].x;
        texCoords[k*2+1] = mesh->mTextureCoords[0][k].y; 
        
      }
      glGenBuffers(1, &buffer);
      glBindBuffer(GL_ARRAY_BUFFER, buffer);
      glBufferData(GL_ARRAY_BUFFER, sizeof(float)*2*mesh->mNumVertices, texCoords, GL_STATIC_DRAW);
      glEnableVertexAttribArray(texCoordLoc);
      glVertexAttribPointer(texCoordLoc, 2, GL_FLOAT, 0, 0, 0);
    }

    // unbind buffers
    glBindVertexArray(0);
    glBindBuffer(GL_ARRAY_BUFFER,0);
    glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
  
    // create material uniform buffer
    aiMaterial *mtl = _scene->mMaterials[mesh->mMaterialIndex];
      
    aiString texPath; //contains filename of texture
    if(AI_SUCCESS == mtl->GetTexture(aiTextureType_DIFFUSE, 0, &texPath)){
        //bind texture
        unsigned int texId = textureIdMap[texPath.data];
        aMesh.texIndex = texId;
        aMat.texCount = 1;
      }
    else
      aMat.texCount = 0;

    float c[4];
    set_float4(c, 0.8f, 0.8f, 0.8f, 1.0f);
    aiColor4D diffuse;
    if(AI_SUCCESS == aiGetMaterialColor(mtl, AI_MATKEY_COLOR_DIFFUSE, &diffuse))
      color4_to_float4(&diffuse, c);
    memcpy(aMat.diffuse, c, sizeof(c));

    set_float4(c, 0.2f, 0.2f, 0.2f, 1.0f);
    aiColor4D ambient;
    if(AI_SUCCESS == aiGetMaterialColor(mtl, AI_MATKEY_COLOR_AMBIENT, &ambient))
      color4_to_float4(&ambient, c);
    memcpy(aMat.ambient, c, sizeof(c));

    set_float4(c, 0.0f, 0.0f, 0.0f, 1.0f);
    aiColor4D specular;
    if(AI_SUCCESS == aiGetMaterialColor(mtl, AI_MATKEY_COLOR_SPECULAR, &specular))
      color4_to_float4(&specular, c);
    memcpy(aMat.specular, c, sizeof(c));

    set_float4(c, 0.0f, 0.0f, 0.0f, 1.0f);
    aiColor4D emission;
    if(AI_SUCCESS == aiGetMaterialColor(mtl, AI_MATKEY_COLOR_EMISSIVE, &emission))
      color4_to_float4(&emission, c);
    memcpy(aMat.emissive, c, sizeof(c));

    float shininess = 0.0;
    unsigned int max;
    aiGetMaterialFloatArray(mtl, AI_MATKEY_SHININESS, &shininess, &max);
    aMat.shininess = shininess;

    glGenBuffers(1,&(aMesh.uniformBlockIndex));
    glBindBuffer(GL_UNIFORM_BUFFER,aMesh.uniformBlockIndex);
    glBufferData(GL_UNIFORM_BUFFER, sizeof(aMat), (void *)(&aMat), GL_STATIC_DRAW);

    myMeshes.push_back(aMesh);
  }
}

void LevelBehavior::cleanup() {
  // cleaning up
  textureIdMap.clear();  
  // clear myMeshes stuff
  for (unsigned int i = 0; i < myMeshes.size(); ++i) {   
    glDeleteVertexArrays(1,&(myMeshes[i].vao));
    glDeleteTextures(1,&(myMeshes[i].texIndex));
    glDeleteBuffers(1,&(myMeshes[i].uniformBlockIndex));
  }
}

void LevelBehavior::init() {

  cout<<"Vendor: "<<glGetString (GL_VENDOR)<<endl;
  cout<<"Renderer: "<<glGetString (GL_RENDERER)<<endl;
  cout<<"Version: "<<glGetString (GL_VERSION)<<endl;
  cout<<"GLSL: "<<glGetString (GL_SHADING_LANGUAGE_VERSION)<<endl;

  _actor = &actor();
  _game = &_actor->game();
  _mogl = _game->getPlugin<MultimediaOGL>();
  _cam = &_mogl->getCamera();
  _shader = _mogl->shaderPrograms().get("_pac_basic");
  setShaderProgram(_shader);

  // blue clearcolor
  glClearColor(1, 1, 1, 1);
  glEnable(GL_DEPTH_TEST);

  _scene = _importer.ReadFile(_levelObjFile, aiProcess_CalcTangentSpace       |  
                               aiProcess_Triangulate            | 
                               aiProcess_JoinIdenticalVertices  | 
                               aiProcess_SortByPType);
  genVaos();
  Drawable::init();

  aiVector3D scene_min, scene_max, scene_center;
  get_bounding_box(&scene_min, &scene_max);
  cout<<scene_min.x<<' '<<scene_min.y<<' '<<scene_min.z<<endl;
  cout<<scene_max.x<<' '<<scene_max.y<<' '<<scene_max.z<<endl;

  _cam->setOrthogonal(-5, 5, -5, 5);
  _cam->setPosition(glm::vec3(-70, 20, 70));
  _cam->setCenter(glm::vec3(0, 0, 0));
  _cam->setUp(glm::vec3(0, 1, 0));
}

void LevelBehavior::onDestroy() {
  Drawable::onDestroy();
  cleanup();
}

void LevelBehavior::setShaderProgram(ShaderProgram* shader) {
  Drawable::setShaderProgram(shader);

  GLuint programId = shader->getProgramId();
  glBindFragDataLocation(programId, 0, "out_color");
  glBindAttribLocation(programId, vertexLoc, "position");
  glBindAttribLocation(programId, normalLoc, "normal");
  glBindAttribLocation(programId, texCoordLoc, "texCoord");

  texUnit = glGetUniformLocation(programId, "texUnit");
}


void LevelBehavior::setIdentityMatrix(float *mat, int size) {
  // fill matrix with 0s
  for (int i = 0; i < size * size; ++i)
      mat[i] = 0.0f;
  // fill diagonal with 1s
  for (int i = 0; i < size; ++i)
    mat[i + i * size] = 1.0f;
}

void LevelBehavior::multMatrix(float *a, float *b) {
  float res[16];
  for (int i = 0; i < 4; ++i) {
    for (int j = 0; j < 4; ++j) {
      res[j*4 + i] = 0.0f;
      for (int k = 0; k < 4; ++k) {
        res[j*4 + i] += a[k*4 + i] * b[j*4 + k]; 
      }
    }
  }
  memcpy(a, res, 16 * sizeof(float));
}

void LevelBehavior::pushMatrix() {
  float *aux = (float *)malloc(sizeof(float) * 16);
  memcpy(aux, modelMatrix, sizeof(float) * 16);
  matrixStack.push_back(aux);
}

void LevelBehavior::popMatrix() {
  float *m = matrixStack[matrixStack.size()-1];
  memcpy(modelMatrix, m, sizeof(float) * 16);
  matrixStack.pop_back();
  free(m);
}

void LevelBehavior::setModelMatrix() {
  //glm::mat4 model(1.0);
  //model = glm::translate(model, glm::vec3(transform().position.x, transform().position.y, transform().position.z));
  //model = glm::rotate(model, glm::radians(static_cast<float>(transform().rotation.x)), glm::vec3(1., 0., 0.));
  //model = glm::rotate(model, glm::radians(static_cast<float>(transform().rotation.y)), glm::vec3(0., 1., 0.));
  //model = glm::rotate(model, glm::radians(static_cast<float>(transform().rotation.z)), glm::vec3(0., 0., 1.));
  //model = glm::scale(model, glm::vec3(transform().scale.x, transform().scale.y, transform().scale.z));
  //glm::vec3 origin(getOrigin().x, getOrigin().y, getOrigin().z);
  //model = glm::translate(model, -origin);
  
  glm::mat4 model;
  for (int i=0; i<4; ++i) {
    model[i].x=modelMatrix[i];
    model[i].y=modelMatrix[4+i];
    model[i].z=modelMatrix[8+i];
    model[i].w=modelMatrix[12+i];
  }
  _shader->setUniformMatrix4f("model", model);
}

void LevelBehavior::recursive_render(const aiScene *sc, const aiNode* nd) {
  // Get node transformation matrix
  aiMatrix4x4 m = nd->mTransformation;
  // OpenGL matrices are column major
  m.Transpose();

  // save model matrix and apply node transformation
  pushMatrix();

  float aux[16];
  memcpy(aux,&m,sizeof(float) * 16);
  multMatrix(modelMatrix, aux);
  setModelMatrix();


  // draw all meshes assigned to this node
  for (unsigned int n=0; n < nd->mNumMeshes; ++n) {
    // bind material uniform
    glBindBufferRange(GL_UNIFORM_BUFFER, materialUniLoc, myMeshes[nd->mMeshes[n]].uniformBlockIndex, 0, sizeof(struct MyMaterial)); 
    // bind texture
    glBindTexture(GL_TEXTURE_2D, myMeshes[nd->mMeshes[n]].texIndex);
    // bind VAO
    glBindVertexArray(myMeshes[nd->mMeshes[n]].vao);
    // draw
    glDrawElements(GL_TRIANGLES,myMeshes[nd->mMeshes[n]].numFaces*3,GL_UNSIGNED_INT, 0);
  }

  // draw all children
  for (unsigned int n=0; n < nd->mNumChildren; ++n) {
    recursive_render(sc, nd->mChildren[n]);
  }
  popMatrix();
}


void LevelBehavior::draw() {
  
  _shader->setUniformMatrix4f("projection", _cam->getProjection());
  _shader->setUniformMatrix4f("view", _cam->getView());

  GLuint programId = _shader->getProgramId();
  glUniformBlockBinding(programId, glGetUniformBlockIndex(programId, "Material"), materialUniLoc);
  glUniform1i(texUnit, 0);

  setIdentityMatrix(modelMatrix, 4);
  recursive_render(_scene, _scene->mRootNode);
}

const char* LevelBehavior::behaviorName() {
  return "pac::Level";
}






