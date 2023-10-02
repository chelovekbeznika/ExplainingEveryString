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
        private const String AmmoName = "AmmoCursor";
        
        private readonly InterfaceDrawController drawController;

        private SpriteData cursor;
        private SpriteData ammoArrow;

        internal CursorDisplayer(InterfaceDrawController drawController)
        {
            this.drawController = drawController;
        }

        public string[] GetSpritesNames()
        {
            return new[] { CursorName, AmmoName };
        }

        public void InitSprites(Dictionary<string, SpriteData> sprites)
        {
            cursor = TextureLoadingHelper.GetSprite(sprites, CursorName);
            ammoArrow = TextureLoadingHelper.GetSprite(sprites, AmmoName);
        }

        internal void Draw(PlayerWeaponInterfaceInfo playerWeapon)
        {
            var mousePoint = Mouse.GetState().Position;
            var position = ScreenCoordinatesHelper.ConvertToInnerScreenCoordinates(mousePoint.X, mousePoint.Y);
            var leftTopCursorCorner = position - new Vector2(cursor.Width / 2, cursor.Height / 2);
            drawController.Draw(cursor, leftTopCursorCorner, true);

            if (playerWeapon.MaxAmmo != 1)
            {
                var degrees = 270 - playerWeapon.CurrentAmmo / (Single)playerWeapon.MaxAmmo * 360;
                drawController.DrawArrow(ammoArrow, position, AngleConverter.ToRadians(degrees), true);
            }
        }
    }
}
