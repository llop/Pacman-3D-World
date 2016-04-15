# pac-man
Dependencies
============
* SFML 2.3.2

Compiling & running
===================
```
# Make directory for project
mkdir pac-man
cd pac-man

# Clone necessary repos
git clone https://github.com/Galbar/hummingbird.git
git clone https://github.com/Galbar/hummingbird-MOGL.git MOGL
git clone https://github.com/g-truc/glm.git
git clone https://github.com/llop/pac-man.git

# Compile framework
cd hummingbird
make

cd ../MOGL
make

# Compile game
cd ../pac-man
make

# Run game
./pac-man
```
 
