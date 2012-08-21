using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Camera2DLib
{
    public class Camera2D
    {
        private SpriteBatch spriteRenderer;
        private Vector2 cameraPosition; // top left corner of the camera

        public Vector2 Position
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }

        public Camera2D(SpriteBatch renderer)
        {
            spriteRenderer = renderer;
            cameraPosition = new Vector2(0, 0);
        }

        public void DrawNode(Scene2DNode node)
        {   
            // get the screen position of the node
            Vector2 drawPosition = ApplyTransformations(node.Position);
            node.Draw(spriteRenderer, drawPosition);
        }

        private Vector2 ApplyTransformations(Vector2 nodePosition)
        {
            // apply translation
            Vector2 finalPosition = nodePosition - cameraPosition;
            // you can apply scaling and rotation here also
            //.....
            //--------------------------------------------
            return finalPosition;
        }

        public void Translate(Vector2 moveVector)
        {
            cameraPosition += moveVector;
        }
    }
}
