using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class GameObjectsFactory
    {
        private Dictionary<String, Blueprint> blueprintsStorage = new Dictionary<String, Blueprint>();
        private Dictionary<String, Func<GameObjectStartPosition, String, IGameObject>> enemyConstruction;
        internal Level Level { get; set; }

        internal GameObjectsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
            this.enemyConstruction = new Dictionary<String, Func<GameObjectStartPosition, String, IGameObject>>
            {
                { "Mine", (pos, name) => Construct<Mine, EnemyBlueprint>(name, pos) },
                { "Hunter", (pos, name) => Construct<Hunter, HunterBlueprint>(name, pos) },
                { "FixedCannon", (pos, name) => Construct<FixedCannon, FixedCannonBlueprint>(name, pos) },
            };
        }

        internal Player ConstructPlayer(GameObjectStartPosition position)
        {
            return Construct<Player, PlayerBlueprint>(position);
        }

        internal List<Wall> ConstructWalls(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Wall, Blueprint>(name, 
                positions.Select(pos => new GameObjectStartPosition { Position = pos, Angle = 0 }));
        }

        internal List<IGameObject> ConstructEnemies(String name, IEnumerable<GameObjectStartPosition> positions)
        {
            String type = (blueprintsStorage[name] as EnemyBlueprint).Type;
            return positions.Select(pos => enemyConstruction[type](pos, name)).ToList();
        }

        private List<TGameObject> Construct<TGameObject, TBlueprint>(IEnumerable<GameObjectStartPosition> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, positions);
        }

        private List<TGameObject> Construct<TGameObject, TBlueprint>(String name, IEnumerable<GameObjectStartPosition> positions)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TGameObject, TBlueprint>(name, position)).ToList();
        }

        private TGameObject Construct<TGameObject, TBlueprint>(GameObjectStartPosition position)
            where TGameObject : GameObject<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TGameObject, TBlueprint>(typeof(TGameObject).Name, position);
        }

        private TGameObject Construct<TGameObject, TBlueprint>(String name, GameObjectStartPosition position)
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
