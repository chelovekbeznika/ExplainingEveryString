using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Notifications;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Notifications
{
    internal class Notification : IUpdateable
    {
        internal Single TimeToLive { get; private set; }
        internal String Type { get; private set; }
        internal SpriteState Sprite { get; private set; }
        internal Int32 Priority { get; private set; }
        internal Boolean IsActive => TimeToLive > 0;

        internal Notification(NotificationSpecification specification, IAssetsStorage storage)
        {
            var typeName = specification.Type;
            Type = typeName;
            Sprite = new SpriteState(typeName, storage.GetSprite(typeName).AnimationCycle);
            Priority = specification.Priority;
            TimeToLive = specification.Duration;
        }

        public void Update(Single elapsedSeconds)
        {
            TimeToLive -= elapsedSeconds;
            Sprite.Update(elapsedSeconds);
        }
    }
}
