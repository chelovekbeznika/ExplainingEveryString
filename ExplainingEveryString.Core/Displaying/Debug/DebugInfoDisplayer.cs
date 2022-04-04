#if DEBUG
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using IUpdateable = ExplainingEveryString.Core.GameModel.IUpdateable;

namespace ExplainingEveryString.Core.Displaying.Debug
{
    internal class DebugInfoDisplayer : IUpdateable
    {
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private List<Vector2> currentDebugInfo = new List<Vector2>();
        private Texture2D debugPoint;

        //Let's pray there are only one gameplay component
        internal static DebugInfoDisplayer Instance { get; private set; }

        public DebugInfoDisplayer(IScreenCoordinatesMaster screenCoordinatesMaster, ContentManager content)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.debugPoint = content.Load<Texture2D>(@"Sprites/Editor/Waypoint");
            Instance = this;
        }

        internal void AddDebugInfo(List<Vector2> positions)
        {
            currentDebugInfo = positions.Select(p => screenCoordinatesMaster.ConvertToScreenPosition(p)).ToList();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var center = new Vector2(debugPoint.Width / 2, debugPoint.Height / 2);
            foreach (var position in currentDebugInfo)
            {
                spriteBatch.Draw(debugPoint, position, null, Color.White, 0, center, 1, SpriteEffects.None, 0);
            }
        }

        public void Update(Single elapsedSeconds)
        {
        }
    }
}
#endif