#include "hummingbird/hum.hpp"
#include "MOGL/MOGL.hpp"
#include "GamePlugin.hpp"


#define TILE_SIZE 48.f
#define WINDOW_WIDTH 1024.f
#define WINDOW_HEIGHT 720.f
#define ORTHO_WIDTH (WINDOW_WIDTH / TILE_SIZE)
#define ORTHO_HEIGHT (WINDOW_HEIGHT / TILE_SIZE)


int main(void) {
    hum::Game g;

    // add media manager
    sf::ContextSettings settings;
    settings.antialiasingLevel = 2;
    settings.depthBits = 24;
    settings.majorVersion = 3;
    settings.minorVersion = 3;
    auto mogl = g.addPlugin<mogl::MultimediaOGL>(sf::VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT),
        "Pac-man", sf::Style::Default, settings);

    // default camera
    mogl->getCamera().setPerspective(90, WINDOW_WIDTH / WINDOW_HEIGHT);
    mogl->getCamera().setPosition(glm::vec3(0, 0, -1));
    mogl->getCamera().setCenter(glm::vec3(0, 0, 1));
    mogl->getCamera().setUp(glm::vec3(0, 1, 0));

    // add game manager
    //g.addPlugin<GamePlugin>();
    //

    auto a = g.makeActor();
    mogl::Model model;
    model.loadFromString(R"(
v 0 0 1
v 1 0 1
v 1 1 1
v 0 1 1
f 1 2 3
f 1 3 4
)");
    mogl::VertexArray va;
    va.loadFromModel(model);
    mogl::Shader vs, fs;
    vs.loadFromSource(mogl::Shader::Type::VERTEX_SHADER, R"(
#version 330

uniform mat4 projection, view, model;
in vec3 position;

void main()
{
    gl_Position = projection * view * model * vec4(position, 1.0);
}
)");
    hum::assert_msg(vs.isCompiled(), "Error compiling plain.vert\n" + vs.log());
    fs.loadFromSource(mogl::Shader::Type::FRAGMENT_SHADER, R"(
#version 330

uniform vec4 color;
out vec4 out_color;

void main()
{
    out_color = color;
}
)");
    hum::assert_msg(fs.isCompiled(), "Error compiling plain.vert\n" + fs.log());

    mogl->shaderPrograms().load("teapot", mogl::ShaderProgramDef{vs, fs, "out_color"});
    auto mesh = a->addBehavior<mogl::Mesh>(va);
    mesh->setShaderProgram(mogl->shaderPrograms().get("teapot"));

    mesh->setOrigin((va.getBoundingBox().first + va.getBoundingBox().second)/2.f);

    a->transform().position.z = 1000.f;

    // loop
    g.run();
    return 0;
}
