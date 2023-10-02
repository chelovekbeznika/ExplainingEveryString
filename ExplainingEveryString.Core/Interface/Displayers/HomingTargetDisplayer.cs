using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class HomingTargetDisplayer : IDisplayer
    {
        private SpriteData target;
        private InterfaceDrawController displayer;

        internal HomingTargetDisplayer(InterfaceDrawController displayer)
        {
            this.displayer = displayer;
        }

        public String[] GetSpritesNames() => new[] { "HomingTarget" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            target = TextureLoadingHelper.GetSprite(sprites, "HomingTarget");
        }

        public void Draw(EnemyInterfaceInfo info)
        {
            var center = info.PositionOnScreen.Center;
            displayer.Draw(target, new Vector2(center.X - target.Width / 2, center.Y - target.Height /2));
        }
    }
}
