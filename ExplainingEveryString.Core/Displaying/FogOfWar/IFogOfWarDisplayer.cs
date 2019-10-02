using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal interface IFogOfWarDisplayer : GameModel.IUpdateable
    {
        FogOfWarSpecification Specification { get; }
        void Construct(FogOfWarSpecification specification, SpriteDataBuilder spriteDataBuilder);
        void Draw(SpriteBatch spriteBatch, FogOfWarSpriteEntry[] fogOfWarSpriteEntries);
    }
}