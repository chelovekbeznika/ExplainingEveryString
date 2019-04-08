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
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, positions);
        }

        internal List<TGameObject> Construct<TGameObject, TBlueprint>(String name, IEnumerable<Vector2> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TGameObject, TBlueprint>(name, position)).ToList();
        }

        internal TGameObject Construct<TGameObject, TBlueprint>(Vector2 position)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, position);
        }

        internal TGameObject Construct<TGameObject, TBlueprint>(String name, Vector2 position)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            TBlueprint blueprint = blueprintsStorage[name] as TBlueprint;
            TGameObject gameObject = new TGameObject();
            gameObject.Initialize(blueprint, Level, position);
            return gameObject;
        }
    }
}
