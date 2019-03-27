using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class GameObjectsFactory
    {
        private Dictionary<Type, Blueprint> blueprintsStorage = new Dictionary<Type, Blueprint>();

        internal GameObjectsFactory(IBlueprintsLoader blueprintsLoader)
        {
            List<Blueprint> blueprints = blueprintsLoader.GetBlueprints();
            foreach (Blueprint blueprint in blueprints)
            {
                blueprintsStorage.Add(blueprint.GetType(), blueprint);
            }
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
            TBlueprint blueprint = blueprintsStorage[typeof(TBlueprint)] as TBlueprint;
            TGameObject gameObject = new TGameObject();
            gameObject.Initialize(blueprint, position);
            return gameObject;
        }

        internal void Example()
        {
            Construct<Player, PlayerBlueprint>(new Vector2(0, 0));
        }
    }
}
