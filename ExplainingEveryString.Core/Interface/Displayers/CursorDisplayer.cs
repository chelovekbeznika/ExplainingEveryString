using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class CursorDisplayer : IDisplayer
    {
        private const String CursorName = "Target";
        
        private readonly InterfaceDrawController drawController;

        private SpriteData cursor;

        internal CursorDisplayer(InterfaceDrawController drawController)
        {
            this.drawController = drawController;
        }

        public string[] GetSpritesNames()
        {
            return new[] { CursorName };
        }

        public void InitSprites(Dictionary<string, SpriteData> sprites)
        {
            cursor = TextureLoadingHelper.GetSprite(sprites, CursorName);
        }

        internal void Draw()
        {
            var mousePoint = Mouse.GetState().Position;
            var position = ScreenCoordinatesHelper.ConvertToInnerScreenCoordinates(mousePoint.X, mousePoint.Y);
            position -= new Vector2(cursor.Width / 2, cursor.Height / 2);
            drawController.Draw(cursor, position);
        }
    }
}
