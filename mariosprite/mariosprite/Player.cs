using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace GayMarioGame
{
    public class Player
    {
        // Textures
        public Texture2D TextureRight;       
        public Texture2D TextureLeft;      
        public Texture2D TextureStanding;  
        public Texture2D TextureJumping;
        public Rectangle rectangle;

        //Screen Position of character
        private int posx;
        private int posy;
        public int startposx;
        public int startposy;

        public List<int> x = new List<int>();
        public List<int> y = new List<int>();

        private float totalElapsed;   // Abgelaufene Zeit

        private int rows;             // Anzahl der Zeilen
        private int columns;          // Anzahl der Spalten
        private int width;            // Breite eines Einzelbilds
        private int height;           // Höhe eines Einzelbilds
        private float animationSpeed; // Bilder pro Sekunde
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public bool direction = true;
        public bool moving = false;
        public bool jumping = false;

        public int currentRow;       // Aktuelle Zeile
        public int currentColumn;    // Aktuelle Spalte

        public void LoadGraphic(
          int rows,
          int columns,
          int width,
          int height,
          int animationSpeed
          )
        {
            this.rows = rows;
            this.columns = columns;
            this.width = width;
            this.height = height;
            this.animationSpeed = (float)1 / animationSpeed;
            this.rectangle = new Rectangle(this.posx, this.posy, width, height);

            totalElapsed = 0;
            currentRow = 0;
            currentColumn = 0;
        }
        public void LoadGraphicLeft(
          Texture2D texture
          )
        {
            this.TextureLeft = texture;
        }
        public void LoadGraphicRight(
          Texture2D texture
          )
        {
            this.TextureRight = texture;
        }

        public void LoadGraphicStanding(
          Texture2D texture
          )
        {
            this.TextureStanding = texture;
        }

        public void LoadGraphicJumping(
         Texture2D texture
         )
        {
            this.TextureJumping = texture;
        }

        public void Update(float elapsed, bool direction, bool moving, bool jumping)
        {
            this.direction = direction;
            this.moving = moving;
            this.jumping = jumping;
            if (this.y.Count == 0)
                this.y.Add(0);
            if (!moving || this.y[0] != 0)
            {
                if (direction)
                {
                    currentRow = 0;
                    currentColumn = 0;
                }
                else
                {
                    currentRow = 0;
                    currentColumn = 1;
                }
                return;

            }

            totalElapsed += elapsed;
            if (totalElapsed > animationSpeed && this.y[0] == 0)
            {
                totalElapsed -= animationSpeed;

                currentColumn += 1;
                if (currentColumn >= columns)
                {
                    currentRow += 1;
                    currentColumn = 0;

                    if (currentRow >= rows)
                    {
                        currentRow = 0;
                    }
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            //color = new Color(255, 255, 255, 0);
            //if (spriteBatch == null)
            //    return;
            //if (this.y.Count == 0)
            //    return;
            this.spriteBatch = spriteBatch;
            if (direction && moving && !jumping)
            {
                spriteBatch.Draw(
                    TextureRight,
                    new Rectangle((int)position.X, (int)position.Y, width, height),
                    new Rectangle(
                      currentColumn * width,
                      currentRow * height,
                      width, height),
                    color
                    );
            }
            else if (!direction && moving && !jumping)
            {
                spriteBatch.Draw(
                    TextureLeft,
                    new Rectangle((int)position.X, (int)position.Y, width, height),
                    new Rectangle(
                      currentColumn * width,
                      currentRow * height,
                      width, height),
                    color
                    );
            }
            if (!moving && !jumping)
            {
                if(TextureStanding == null)
                {
                    //wait for texture loading
                    return;
                }
                spriteBatch.Draw(
                    TextureStanding,
                    new Rectangle((int)position.X, (int)position.Y, width, height),
                    new Rectangle(
                        currentColumn * width,
                        currentRow * height,
                        width, height),
                    color
                    );
            }
            else if (!(this.y.Count == 0) && jumping && this.y[0] != 0)
            {
                if (TextureJumping == null)
                {
                    //wait for texture loading
                    return;
                }
                spriteBatch.Draw(
                    TextureJumping,
                    new Rectangle((int)position.X, (int)position.Y, width, height),
                    new Rectangle(
                      currentColumn * width,
                      currentRow * height,
                      width, height),
                    color
                    );
            }
            else if(!(this.y.Count == 0) && this.y[0] == 0 && jumping)
            {
                if (direction)
                {
                    spriteBatch.Draw(
                        TextureRight,
                        new Rectangle((int)position.X, (int)position.Y, width, height),
                        new Rectangle(
                          currentColumn * width,
                          currentRow * height,
                          width, height),
                        color
                        );
                }
                else if (!direction)
                {
                    spriteBatch.Draw(
                        TextureLeft,
                        new Rectangle((int)position.X, (int)position.Y, width, height),
                        new Rectangle(
                          currentColumn * width,
                          currentRow * height,
                          width, height),
                        color
                        );
                }
            }
        }
        public void setGraphics(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }
        public GraphicsDeviceManager getGraphics()
        {
            return this.graphics;
        }

        public bool Collideswith(Rectangle sprite)
        {
            if (this.rectangle.Intersects(sprite))
                return true;
            else return false;
        }

        public void kill()
        {
            //kill player, restart level

            //reset player
            this.posx = startposx;
            this.posy = startposy;
        }

        public int getWidth()
        {
            return this.width;
        }
        public void setPosx(int posx)
        {
            this.posx = posx;
            this.rectangle.X = posx;
        }
        public int getPosx()
        {
            return this.posx;
        }
        public void setPosy(int posy)
        {
            this.posy = posy;
            this.rectangle.Y = posy;
        }
        public int getPosy()
        {
            return this.posy;
        }

        public void updatePosition()
        {
            if (x.Count == 0)
                x.Add(0);
            if (y.Count == 0)
                y.Add(0);
            this.posx += x[0];
            this.posy += y[0];
            this.rectangle.X = posx;
            this.rectangle.Y = posy;
            x.RemoveAt(0);
            y.RemoveAt(0);
        }
    }
}
