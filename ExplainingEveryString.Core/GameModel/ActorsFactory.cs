using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorsFactory
    {
        private Dictionary<String, Blueprint> blueprintsStorage = new Dictionary<String, Blueprint>();
        private Dictionary<String, Func<ActorStartInfo, IEnemy>> enemyConstruction;
        internal Level Level { get; set; }

        internal ActorsFactory(IBlueprintsLoader blueprintsLoader)
        {
            this.blueprintsStorage = blueprintsLoader.GetBlueprints();
            this.enemyConstruction = new Dictionary<String, Func<ActorStartInfo, IEnemy>>
            {
                { "Enemy", (pos) => Construct<Enemy<EnemyBlueprint>, EnemyBlueprint>(pos) },
                { "ShadowEnemy", (pos) => Construct<ShadowEnemy, ShadowEnemyBlueprint>(pos) },
                { "FirstBoss", (pos) => Construct<FirstBoss, FirstBossBlueprint>(pos) }
            };
        }

        internal Player ConstructPlayer(ActorStartInfo position)
        {
            return Construct<Player, PlayerBlueprint>(position);
        }

        internal Door ConstructDoor(ActorStartInfo position, Int32 openingWaveNumber)
        {
            Door door = Construct<Door, DoorBlueprint>(position);
            door.OpeningWaveNumber = openingWaveNumber;
            return door;
        }

        internal List<Obstacle> ConstructObstacles(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Obstacle, Blueprint>(name, 
                positions.Select(pos => new ActorStartInfo { BlueprintType = name, Position = pos }));
        }

        internal List<IEnemy> ConstructEnemies(IEnumerable<ActorStartInfo> positions)
        {
            return positions.Select(ConstructEnemy).ToList();
        }

        internal IEnemy ConstructEnemy(ActorStartInfo position)
        {
            String type = (blueprintsStorage[position.BlueprintType] as EnemyBlueprint).Type;
            return enemyConstruction[type](position);
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
            actor.Initialize(blueprint, Level, position, this);
            return actor;
        }
    }
}
