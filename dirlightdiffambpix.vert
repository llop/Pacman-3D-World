#version 330

layout (std140) uniform Matrices {

	mat4 projMatrix;
	mat4 viewMatrix;
	mat4 modelMatrix;
};

in vec3 position;
in vec3 normal;
in vec2 texCoord;

out vec4 vertexPos;
out vec2 TexCoord;
out vec3 Normal;

void main()
{
	Normal = normalize(vec3(viewMatrix * modelMatrix * vec4(normal,0.0)));	
	TexCoord = vec2(texCoord);
	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(position,1.0);
}