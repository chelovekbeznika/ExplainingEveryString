using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorsFactory
    {
        private Dictionary<String, Blueprint> blueprintsStorage = new Dictionary<String, Blueprint>();
        private Dictionary<String, Func<ActorStartPosition, String, IActor>> enemyConstruction;
        internal Level Level { get; set; }

        internal ActorsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
            this.enemyConstruction = new Dictionary<String, Func<ActorStartPosition, String, IActor>>
            {
                { "Mine", (pos, name) => Construct<Mine, EnemyBlueprint>(name, pos) },
                { "Hunter", (pos, name) => Construct<Hunter, HunterBlueprint>(name, pos) },
                { "FixedCannon", (pos, name) => Construct<FixedCannon, FixedCannonBlueprint>(name, pos) },
            };
        }

        internal Player ConstructPlayer(ActorStartPosition position)
        {
            return Construct<Player, PlayerBlueprint>(position);
        }

        internal List<Wall> ConstructWalls(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Wall, Blueprint>(name, 
                positions.Select(pos => new ActorStartPosition { Position = pos, Angle = 0 }));
        }

        internal List<IActor> ConstructEnemies(String name, IEnumerable<ActorStartPosition> positions)
        {
            String type = (blueprintsStorage[name] as EnemyBlueprint).Type;
            return positions.Select(pos => enemyConstruction[type](pos, name)).ToList();
        }

        private List<TActor> Construct<TActor, TBlueprint>(IEnumerable<ActorStartPosition> positions)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TActor, TBlueprint>(typeof(TActor).Name, positions);
        }

        private List<TActor> Construct<TActor, TBlueprint>(String name, IEnumerable<ActorStartPosition> positions)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TActor, TBlueprint>(name, position)).ToList();
        }

        private TActor Construct<TActor, TBlueprint>(ActorStartPosition position)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return Construct<TActor, TBlueprint>(typeof(TActor).Name, position);
        }

        private TActor Construct<TActor, TBlueprint>(String name, ActorStartPosition position)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            TBlueprint blueprint = blueprintsStorage[name] as TBlueprint;
            TActor actor = new TActor();
            actor.Initialize(blueprint, Level, position);
            return actor;
        }
    }
}
