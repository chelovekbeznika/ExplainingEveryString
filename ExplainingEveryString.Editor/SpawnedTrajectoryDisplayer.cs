using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class SpawnedTrajectoryDisplayer : IEditableDisplayer
    {
        internal Dictionary<String, Texture2D> spawnPointsMarkers;

        internal SpawnedTrajectoryDisplayer(ContentManager content)
        {
            spawnPointsMarkers = Enumerable.Range(0, 10).Select(digit => digit.ToString())
                .ToDictionary(digit => digit, digit => content.Load<Texture2D>($@"Sprites/Editor/SpawnPointMarkers/{digit}"));
        }

        public void Draw(SpriteBatch spriteBatch, String type, Vector2 positionOnScreen, Boolean selected)
        {
            var digit = type[type.Length - 1].ToString();
            var sprite = spawnPointsMarkers[digit];
            var centerOfSprite = new Vector2(sprite.Width / 2, sprite.Height / 2);
            spriteBatch.Draw(sprite, positionOnScreen, null, selected ? Color.Black : Color.White,
                rotation: 0, origin: centerOfSprite, scale: 1, effects: SpriteEffects.None, layerDepth: 0);
        }
    }
}
