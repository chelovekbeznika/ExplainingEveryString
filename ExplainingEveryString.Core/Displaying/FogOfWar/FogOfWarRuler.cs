﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

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
            var levelFogOfWar = extractor.GetFogOfWarRegions();
            var screenFogOfWar = screenDetector.GetFogOfWarRegions(levelFogOfWar);
            var fogOfWarSprites = screenFogOfWar
                .Select(region => filler.Fill(region, displayer.Specification))
                .SelectMany(spriteEntries => spriteEntries).ToArray();
            displayer.Draw(spriteBatch, fogOfWarSprites);
        }

        public void Update(Single elapsedSeconds)
        {
            displayer.Update(elapsedSeconds);
        }
    }
}
