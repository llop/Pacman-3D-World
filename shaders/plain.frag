R"(
#version 330

uniform sampler2D tex;
uniform vec4 color;
out vec4 out_color;

void main()
{
    out_color = color;
}
)"
