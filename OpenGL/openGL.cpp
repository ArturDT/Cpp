#include <SDL/SDL.h>
#include <SDL/SDL_opengl.h>
#include <iostream>
#include "Renderer.h"
 
void Renderer::LoadTexture(const std::string & filename) {
  // za�aduj bitmap� z pliku
  SDL_Surface* surface = SDL_LoadBMP(filename.c_str());
  if (!surface) {
    std::cerr << "�adowanie pliku " + filename + " FAILED: " 
                 + SDL_GetError() + "\n";
    exit(1);
  }
 
  // sprawd� wymiary - czy s� pot�g� 2
  const int width = surface->w;
  const int height = surface->h;
  if (((width & (width - 1)) != 0) || ((height & (height - 1)) != 0)) {
    std::cerr << "Obrazek " + filename 
                 + " ma nieprawid�owe wymiary (powinny by� pot�g� 2): "
              << width << "x" << height << "\n";
    exit(1);
  }
 
  GLenum format;
  switch (surface->format->BytesPerPixel) {
     case 3:  format = GL_BGR;   break;
     case 4:  format = GL_BGRA;  break;
     default:  std::cerr << "Nieznany format pliku " + filename + "\n";
               exit(1);
  }
 
  glGenTextures(1, &m_texture);
  glBindTexture(GL_TEXTURE_2D, m_texture);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
  glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
 
  glTexImage2D(GL_TEXTURE_2D, 0, surface->format->BytesPerPixel,
               width, height, 0, format, GL_UNSIGNED_BYTE, surface->pixels);
  if (surface) {
    SDL_FreeSurface(surface);
  }
}
 
void Renderer::DrawSprite(double tex_x, double tex_y, 
                          double tex_w, double tex_h, 
                          double pos_x, double pos_y,
                          double width, double height, 
                          size_t level) {
  const double texture_w = 256.0;    // szeroko�� atlasu
  const double texture_h = 128.0;    // wysoko�� atlasu
 
  const double left = tex_x / texture_w;
  const double right = left + tex_w / texture_w;
  const double bottom = tex_y / texture_h;
  const double top = bottom - tex_h / texture_h;
  /* Obrazek �adowany jest do g�ry nogami, wi�c punkt (0,0) 
   * jest w lewym g�rnym rogu tekstury.
   * St�d wynika, �e w powy�szym wzorze top jest poni�ej bottom
   */
 
  glPushMatrix();  {
    glTranslatef(0, 0, -static_cast<int>(level));
    glBegin(GL_QUADS);  {
      glColor3f(1, 1, 1);
      glTexCoord2f(right, top);     glVertex2f(pos_x+width, pos_y+height);
      glTexCoord2f(left, top);      glVertex2f(pos_x,       pos_y+height);
      glTexCoord2f(left, bottom);   glVertex2f(pos_x,       pos_y);
      glTexCoord2f(right, bottom);  glVertex2f(pos_x+width, pos_y);
    }
    glEnd();
  }
  glPopMatrix();
}