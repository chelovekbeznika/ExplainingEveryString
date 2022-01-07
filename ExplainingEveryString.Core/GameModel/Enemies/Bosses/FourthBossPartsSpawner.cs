using ExplainingEveryString.Core.GameModel.Weaponry;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPartsSpawner : ISpawnedActorsController
    {
        public List<IEnemy> SpawnedEnemies { get; private set; } = new List<IEnemy>();

        public int MaxSpawned { get; private set; }

        public FourthBossPartsSpawner(IFourthBossBrain bossBrain, ActorsFactory factory, String[] partsList)
        {
            foreach (var partName in partsList)
            {
                var newPart = factory.ConstructEnemy(new ActorStartInfo
                {
                    Position = bossBrain.Position,
                    BlueprintType = partName,
                    BehaviorParameters = new BehaviorParameters { }
                });
                (newPart as FourthBossPart).BossBrain = bossBrain;
                SpawnedEnemies.Add(newPart);
            }
            MaxSpawned = partsList.Length;
        }

        public void DivideAliveAndDead(List<IEnemy> avengers)
        {
            SpawnedEnemies = EnemiesDeathProcessor.DivideAliveAndDead(SpawnedEnemies, avengers);
        }

        public void TurnOff()
        {
        }

        public void TurnOn()
        {
        }

        public void Update(float elapsedSeconds)
        {
        }
    }
}
