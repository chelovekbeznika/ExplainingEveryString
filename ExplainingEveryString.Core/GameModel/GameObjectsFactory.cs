using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class GameObjectsFactory
    {
        private Dictionary<String, Blueprint> blueprintsStorage = new Dictionary<String, Blueprint>();
        internal Level Level { get; set; }

        internal GameObjectsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
        }

        internal List<TGameObject> Construct<TGameObject, TBlueprint>(IEnumerable<Vector2> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TGameObject, TBlueprint>(position)).ToList();
        }

        internal TGameObject Construct<TGameObject, TBlueprint>(Vector2 position)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            TBlueprint blueprint = blueprintsStorage[typeof(TGameObject).Name] as TBlueprint;
            TGameObject gameObject = new TGameObject();
            gameObject.Initialize(blueprint, Level, position);
            return gameObject;
        }

        internal void Example()
        {
            Construct<Player, PlayerBlueprint>(new Vector2(0, 0));
        }
    }
}
