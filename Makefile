CC = g++
EXE := pac-man
CFLAGS = -Wall -std=c++11 -O3
ODIR   := obj
SDIR   := src
INC    := -Iinclude -I../Hum2D/include -I../MOGL/include -I../glm
LIBS   := -lsfml-audio -lsfml-window -lsfml-system -lsfml-graphics -lGL -lGLEW -ldl
STATIC := ../Hum2D/lib/libhum2d.a ../MOGL/lib/libmogl.a

SOURCES = $(shell find ./$(SDIR) -name '*.cpp')
OBJS = $(patsubst $(SDIR)/%,$(ODIR)/%,$(SOURCES:./%.cpp=%.o))

all: $(OBJS)
	$(CC) -o $(EXE) $(OBJS) $(CFLAGS) $(INC) $(STATIC) $(LIBS)

$(ODIR)/%.o: $(SDIR)/%.cpp
	$(CC) -c $(INC) -o $@ $< $(CFLAGS)

.PHONY: clean

clean:
	rm -rf $(ODIR)/*.o $(EXE)
