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
    class Menu 
    {
        //SpriteBatch ForegroundBatch;
        SpriteBatch spriteBatch;
        SpriteFont CourierNew;
        Vector2 FontPos;
        float FontRotation;
        GraphicsDeviceManager graphics;
        ContentManager content;

        public Menu(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.content = content;
            CourierNew = content.Load<SpriteFont>("Courier New");
                //ForegroundBatch = new SpriteBatch(graphics.GraphicsDevice);
            
            this.FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            FontRotation = 0;
            
        }


        public void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();

            // Draw Hello World
            string output = "Main Menu";

            // Find the center of the string
            Vector2 FontOrigin = CourierNew.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(CourierNew, output, FontPos, Color.LightGreen,
                FontRotation, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            //Note: Change "CourierNew" to "current" above to enable switching

            // Draw instructions
            Vector2 guidepos = new Vector2(50, FontPos.Y + 100);
            string guide = "Start Game";
            spriteBatch.DrawString(CourierNew, guide, guidepos, Color.Black);

            //ForegroundBatch.End();



        }



        public void Update()
        {

        }

    }
}
