using ExplainingEveryString.Core.Displaying;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FirstBossPhase
    {
        internal SpriteState Sprite { get; set; }
        internal SpriteState OnSprite { get; set; }
        internal SpriteState OffSprite { get; set; }
        internal EnemyBehavior Behavior { get; set; }
        internal Single TurningOnTime => OnSprite.AnimationCycle;
        internal Single TurningOffTime => OffSprite.AnimationCycle;
    }
}
