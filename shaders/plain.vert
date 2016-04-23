R"(
#version 330

uniform mat4 projection, view, model;
in vec2 position;

void main()
{
    gl_Position = projection * view * model * vec4(position, 0.0, 1.0);
}
)"
