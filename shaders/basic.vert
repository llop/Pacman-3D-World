R"(
#version 330

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

in vec3 position;
in vec3 normal;
in vec2 texCoord;

out vec4 vertexPos;
out vec2 TexCoord;
out vec3 Normal;

void main() {
	Normal = normalize(vec3(view * model * vec4(normal, 0.0)));	
	TexCoord = vec2(texCoord);
	gl_Position = projection * view * model * vec4(position, 1.0);
}
)"
