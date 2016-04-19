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
    mogl->getCamera().setOrthogonal(0, -ORTHO_WIDTH, ORTHO_HEIGHT, 0);
    mogl->getCamera().setPosition(glm::vec3(0, 0, -1));
    mogl->getCamera().setCenter(glm::vec3(0, 0, 1));
    mogl->getCamera().setUp(glm::vec3(0, 1, 0));

    // add game manager
    g.addPlugin<GamePlugin>();

    // loop
    g.run();
    return 0;
}
