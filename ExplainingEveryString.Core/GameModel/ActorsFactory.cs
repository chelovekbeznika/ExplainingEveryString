using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Core.GameModel.Enemies.Bosses;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
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
                { "Changeable", (pos) => Construct<ChangeableEnemy, ChangeableEnemyBlueprint>(pos) },
                { "FirstBoss", (pos) => Construct<FirstBoss, FirstBossBlueprint>(pos) },
                { "SecondBoss", (pos) => Construct<SecondBoss, SecondBossBlueprint>(pos) },
                { "ThirdBoss", (pos) => Construct<ThirdBoss, ThirdBossBlueprint>(pos) },
                { "FourthBoss", (pos) => Construct<FourthBossBrain, FourthBossBlueprint>(pos) },
                { "FourthBossPart", (pos) => Construct<FourthBossPart, FourthBossPartBlueprint>(pos) },
                { "FifthBoss", (pos) => Construct<FifthBoss, FifthBossBlueprint>(pos) },
                { "FifthBossHelper", (pos) => Construct<FifthBossHelper, EnemyBlueprint>(pos) },
                { "LastBossPhase", (pos) => Construct<LastBossPhase, LastBossBlueprint>(pos) }
            };
        }

        internal Player ConstructPlayer(ActorStartInfo position, ArsenalSpecification playerArsenal)
        {
            var player = Construct<Player, PlayerBlueprint>(position);
            player.SupplyWeapons(playerArsenal);
            return player;
        }

        internal Door ConstructDoor(ActorStartInfo position, Int32 openingWaveNumber)
        {
            var door = Construct<Door, DoorBlueprint>(position);
            door.OpeningWaveNumber = openingWaveNumber;
            return door;
        }

        internal List<Obstacle> ConstructObstacles(String name, IEnumerable<Vector2> positions)
        {
            return Construct<Obstacle, ObstacleBlueprint>(name, 
                positions.Select(pos => new ActorStartInfo { BlueprintType = name, Position = pos }));
        }

        internal List<IEnemy> ConstructEnemies(IEnumerable<ActorStartInfo> positions)
        {
            return positions.Select(ConstructEnemy).ToList();
        }

        internal IEnemy ConstructEnemy(ActorStartInfo position)
        {
            var type = (blueprintsStorage[position.BlueprintType] as EnemyBlueprint).Type;
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
            var name = position.BlueprintType;
            var blueprint = blueprintsStorage[name] as TBlueprint;
            var actor = new TActor();
            actor.Initialize(blueprint, Level, position, this);
            return actor;
        }
    }
}
