using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Wall : ICollidable
    {
        private Hitbox hitbox;

        public Wall(Hitbox hitbox, CollidableMode mode)
        {
            this.hitbox = hitbox;
            this.CollidableMode = mode;
        }

        public Vector2 Position { get => GetHitboxCenter(); set { } }

        public CollidableMode CollidableMode { get; }

        public Hitbox GetCurrentHitbox()
        {
            return hitbox;
        }

        private Vector2 GetHitboxCenter()
        {
            return new Vector2 { X = (hitbox.Left + hitbox.Right / 2), Y = (hitbox.Bottom + hitbox.Top) / 2 };
        }
    }
}
