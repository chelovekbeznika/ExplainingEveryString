using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class FogOfWarRuler : GameModel.IUpdateable
    {
        private ILevelFogOfWarExtractor extractor;
        private IScreenFogOfWarDetector screenDetector;
        private IFogOfWarFiller filler;
        private IFogOfWarDisplayer displayer;

        internal FogOfWarRuler(ILevelFogOfWarExtractor extractor, IScreenFogOfWarDetector screenDetector,
            IFogOfWarFiller filler, IFogOfWarDisplayer displayer)
        {
            this.extractor = extractor;
            this.screenDetector = screenDetector;
            this.filler = filler;
            this.displayer = displayer;
        }

        internal void DrawFogOfWar(SpriteBatch spriteBatch)
        {
            Rectangle[] levelFogOfWar = extractor.GetFogOfWarRegions();
            FogOfWarScreenRegion[] screenFogOfWar = screenDetector.GetFogOfWarRegions(levelFogOfWar);
            FogOfWarSpriteEntry[] fogOfWarSprites = screenFogOfWar
                .Select(region => filler.Fill(region, displayer.SpritesNumber, displayer.SpriteWidth, displayer.SpriteHeight))
                .SelectMany(spriteEntries => spriteEntries).ToArray();
            displayer.Draw(spriteBatch, fogOfWarSprites);
        }

        public void Update(Single elapsedSeconds)
        {
            displayer.Update(elapsedSeconds);
        }
    }
}
