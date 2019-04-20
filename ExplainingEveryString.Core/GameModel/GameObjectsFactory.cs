using ExplainingEveryString.Core.GameModel.Enemies;
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
        private Dictionary<String, Func<Vector2, IGameObject>> enemyConstruction;
        internal Level Level { get; set; }

        internal GameObjectsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
            this.enemyConstruction = new Dictionary<String, Func<Vector2, IGameObject>>
            {
                { "Mine", (pos) => Construct<Mine, EnemyBlueprint>(pos) },
                { "Hunter", (pos) => Construct<Hunter, HunterBlueprint>(pos) },
                { "FixedCannon", (pos) => Construct<FixedCannon, FixedCannonBlueprint>(pos) }
            };
        }

        internal Player ConstructPlayer(Vector2 position)
        {
            return Construct<Player, PlayerBlueprint>(position);
        }

        internal List<Wall> ConstructWalls(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Wall, Blueprint>(name, positions);
        }

        internal List<IGameObject> ConstructEnemies(String name, IEnumerable<Vector2> positions)
        {
            return positions.Select(pos => enemyConstruction[name](pos)).ToList();
        }

        private List<TGameObject> Construct<TGameObject, TBlueprint>(IEnumerable<Vector2> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, positions);
        }

        private List<TGameObject> Construct<TGameObject, TBlueprint>(String name, IEnumerable<Vector2> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TGameObject, TBlueprint>(name, position)).ToList();
        }

        private TGameObject Construct<TGameObject, TBlueprint>(Vector2 position)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, position);
        }

        private TGameObject Construct<TGameObject, TBlueprint>(String name, Vector2 position)
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
