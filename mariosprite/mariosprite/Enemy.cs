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

namespace RPC
{
    class Enemy : AnimatedSprite
    {
        private bool alive = true;
        float elapsedMilliseconds = 0;

        public Enemy(int posx, int posy)
        {
            this.setPosx(posx);
            this.setPosy(posy);
        }


        public void startDirection(bool direc)
        {
            this.direction = direc;
        }

        public void Update(float elapsed)
        {
            elapsedMilliseconds -= elapsed;

            if (alive)
            {
                if (elapsedMilliseconds <= 0)
                {
                    if (direction)
                    {
                        currentColumn = 0;
                        this.x.Add(5);
                        elapsedMilliseconds = 600;
                    }
                    else
                    {
                        currentColumn = 1;
                        this.x.Add(-5);
                        elapsedMilliseconds = 600;
                    }

                    CheckCorner();
                }
            }
        }

        private void CheckCorner()
        {
            if (this.getGraphics() != null)
                if (this.getPosx() >= getGraphics().GraphicsDevice.Viewport.Width || this.getPosx() <= 0)
                    Directionchange();
        }

        private void Directionchange()
        {
            if (this.direction)
            {
                this.direction = false;
                this.setPosx(this.getPosx() - 5);
                return;
            }
            else
            {
                this.direction = true;
                this.setPosx(this.getPosx() + 5);
                return;
            }
        }

        public void kill(AnimatedSprite player)
        {
            //kill player, restart level
            alive = false;

            //remove enemy from playfield though bottom of screen and direction of hitting
            if (player.getPosx() < this.getPosx())
            {
                var intlist = new int[] { 3, 3, 2, 2, 1, 1 };
                foreach (var integ in intlist)
                    this.x.Add(integ);
            }
            else if (player.getPosx() > this.getPosx() - player.getWidth() / 2)
            {
                var intlist = new int[] { -3, -3, -2, -2, -1, -1 };
                foreach (var integ in intlist)
                    this.x.Add(integ);
            }
            var intliste = new int[] { 20, 15, 4, 3, 2, 1, 0, 0, -1, -1, -2, -2, -3, -4, -5, -6, -7 ,-8,-9,-10,-999};
            this.y.AddRange(intliste);

            //reset player
            //this.posx = startposx;
            //this.posy = startposy;
        }


    }
}
