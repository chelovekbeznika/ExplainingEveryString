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
        private Dictionary<String, Func<ActorStartInfo, IActor>> enemyConstruction;
        internal Level Level { get; set; }

        internal ActorsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
            this.enemyConstruction = new Dictionary<String, Func<ActorStartInfo, IActor>>
            {
                { "Enemy", (pos) => Construct<Enemy<EnemyBlueprint>, EnemyBlueprint>(pos) },
                { "ShadowEnemy", (pos) => Construct<ShadowEnemy, ShadowEnemyBlueprint>(pos) }
            };
        }

        internal Player ConstructPlayer(ActorStartInfo position)
        {
            return Construct<Player, PlayerBlueprint>(position);
        }

        internal List<Wall> ConstructWalls(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Wall, Blueprint>(name, 
                positions.Select(pos => new ActorStartInfo { BlueprintType = name, Position = pos, Angle = 0 }));
        }

        internal List<IActor> ConstructEnemies(IEnumerable<ActorStartInfo> positions)
        {
            return positions.Select(pos => 
            {
                String type = (blueprintsStorage[pos.BlueprintType] as EnemyBlueprint).Type;
                return enemyConstruction[type](pos);
            }).ToList();
        }

        private List<TActor> Construct<TActor, TBlueprint>(String name, IEnumerable<ActorStartInfo> positions)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            return positions.Select(position => Construct<TActor, TBlueprint>(position)).ToList();
        }

        private TActor Construct<TActor, TBlueprint>(ActorStartInfo position)
            where TActor : Actor<TBlueprint>, new()
            where TBlueprint : Blueprint
        {
            String name = position.BlueprintType;
            TBlueprint blueprint = blueprintsStorage[name] as TBlueprint;
            TActor actor = new TActor();
            actor.Initialize(blueprint, Level, position);
            return actor;
        }
    }
}
