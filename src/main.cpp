#include "hummingbird/hum.hpp"
#include "MOGL/MOGL.hpp"

#define TILE_SIZE 48.f
#define WINDOW_WIDTH 1024.f
#define WINDOW_HEIGHT 720.f
#define ORTHO_WIDTH (WINDOW_WIDTH / TILE_SIZE)
#define ORTHO_HEIGHT (WINDOW_HEIGHT / TILE_SIZE)

int main(void)
{
    sf::ContextSettings settings;
    settings.antialiasingLevel = 2;
    settings.depthBits = 24;
    settings.majorVersion = 3;
    settings.minorVersion = 3;
    hum::Game game;
    mogl::MultimediaOGL* mogl = game.addPlugin<mogl::MultimediaOGL>(
            sf::VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Test",
            sf::Style::Default, settings);
    mogl->getCamera().setOrthogonal(0, -ORTHO_WIDTH, ORTHO_HEIGHT, 0);
    mogl->getCamera().setPosition(glm::vec3(0, 0, -1));
    mogl->getCamera().setCenter(glm::vec3(0, 0, 1));
    mogl->getCamera().setUp(glm::vec3(0, 1, 0));
    game.addPlugin<hum::KinematicWorld>();
    auto a = game.makeActor();
    auto k = a->addBehavior<hum::Kinematic>();
    k->velocity().position.x = 5;

    a->addBehavior<mogl::Rectangle>(5,5, sf::Color::Red);

    game.run();
    return 0;
}
