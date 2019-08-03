using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel
{
    internal class TileWall : ICollidable
    {
        private Hitbox hitbox;

        public TileWall(Hitbox hitbox, CollidableMode mode)
        {
            this.hitbox = hitbox;
            this.Mode = mode;
        }

        public Vector2 Position { get => GetHitboxCenter(); set { } }

        public Vector2 OldPosition => GetHitboxCenter();

        public CollidableMode Mode { get; }

        public Hitbox GetCurrentHitbox()
        {
            return hitbox;
        }

        public Hitbox GetOldHitbox()
        {
            return hitbox;
        }

        private Vector2 GetHitboxCenter()
        {
            return new Vector2 { X = (hitbox.Left + hitbox.Right / 2), Y = (hitbox.Bottom + hitbox.Top) / 2 };
        }
    }
}
