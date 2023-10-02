
using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class AmmoStockDisplayer : IDisplayer
    {
        private Dictionary<Int32, SpriteData> digits;
        private const Int32 pixelsFromBottom = 64;
        private const Int32 pixelsFromRight = 32 + Constants.MinimapSize;
        private InterfaceDrawController displayer;

        internal AmmoStockDisplayer(InterfaceDrawController displayer)
        {
            this.displayer = displayer;
        }

        public String[] GetSpritesNames()
        {
            return Enumerable.Range(0,10).Select(digit => $@"Ammo/{digit}").ToArray();
        }

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            digits = Enumerable.Range(0, 10).ToDictionary(
                digit => digit, 
                digit => TextureLoadingHelper.GetSprite(sprites, $@"Ammo/{digit}"));
        }

        internal void Draw(Int32 ammoInStock)
        {
            var position = new Vector2(
                x: displayer.ScreenWidth - 32 - pixelsFromRight,
                y: displayer.ScreenHeight - pixelsFromBottom - digits[0].Height);
            if (ammoInStock != 0)
                while (ammoInStock > 0)
                {
                    displayer.Draw(digits[ammoInStock % 10], position);
                    ammoInStock /= 10;
                    position = new Vector2(position.X - 32, position.Y);
                }
            else
                displayer.Draw(digits[0], position);
        }
    }
}
