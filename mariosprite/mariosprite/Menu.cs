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
        double elapsed = 0;
        enum Entry
        {
            Start,
            Options,
            Exit
        }
        Entry entry;

        public Menu(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            entry = Entry.Start;
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.content = content;
            CourierNew = content.Load<SpriteFont>("Courier New");
                //ForegroundBatch = new SpriteBatch(graphics.GraphicsDevice);
            
            this.FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 3);
            FontRotation = 0;
            
        }


        public void Draw(GameTime gameTime)
        {
            if (Math.Abs(gameTime.TotalGameTime.Milliseconds - elapsed) > 100)
            {
                FontRotation++;
                elapsed = gameTime.TotalGameTime.Milliseconds;
            }
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteBatch.Begin();

            // Draw Hello World
            string output = "Main Menu";

            // Find the center of the string
            Vector2 FontOrigin = CourierNew.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(CourierNew, output, FontPos, Color.LightGreen,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            //Note: Change "CourierNew" to "current" above to enable switching

            // Draw instructions

            Vector2 guidepos = new Vector2(FontPos.X, FontPos.Y + 100);
            string guide = "Start Game";
            if (entry == Entry.Start)
                spriteBatch.DrawString(CourierNew, guide, guidepos, Color.Black, FontRotation, CourierNew.MeasureString(guide) / 2, 1.0f, SpriteEffects.None, 0.5f);
            else
                spriteBatch.DrawString(CourierNew, guide, guidepos, Color.Black, 0, CourierNew.MeasureString(guide) / 2, 1.0f, SpriteEffects.None, 0.5f);

            Vector2 options = new Vector2(FontPos.X, FontPos.Y + 150);
            string ops = "Options";
            if (entry == Entry.Options)
                spriteBatch.DrawString(CourierNew, ops, options, Color.Black, FontRotation, CourierNew.MeasureString(ops) / 2, 1.0f, SpriteEffects.None, 0.5f);
            else
                spriteBatch.DrawString(CourierNew, ops, options, Color.Black, 0, CourierNew.MeasureString(ops) / 2, 1.0f, SpriteEffects.None, 0.5f);


            Vector2 exit = new Vector2(FontPos.X, FontPos.Y + 200);
            string ex = "Exit";
            if (entry == Entry.Exit)
                spriteBatch.DrawString(CourierNew, ex, exit, Color.Black, FontRotation, CourierNew.MeasureString(ex) / 2, 1.0f, SpriteEffects.None, 0.5f);
            else
                spriteBatch.DrawString(CourierNew, ex, exit, Color.Black, 0, CourierNew.MeasureString(ex) / 2, 1.0f, SpriteEffects.None, 0.5f);

            //ForegroundBatch.End();
            


        }



        public void Up()
        {
            switch (entry)
            {
                case Entry.Start :
                    entry = Entry.Exit;
                    break;
                case Entry.Options:
                entry = Entry.Start;
                    break;
                case Entry.Exit:
                    entry = Entry.Options;
                    break;
            }
        }

        public void Down()
        {
            switch (entry)
            {
                case Entry.Start:
                    entry = Entry.Options;
                    break;
                case Entry.Options:
                    entry = Entry.Exit;
                    break;
                case Entry.Exit:
                    entry = Entry.Start;
                    break;
            }
        }

    }
}
