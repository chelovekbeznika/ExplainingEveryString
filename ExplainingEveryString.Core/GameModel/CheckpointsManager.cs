using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class CheckpointsManager
    {
        private ITileCoordinatesMaster tilePositionConverter;
        private LevelData levelData;
        private Data.Level.ActorStartInfo playerStartInfo;
        private Dictionary<String, Checkpoint> checkpointsByName;
        private Dictionary<Int32, Checkpoint> checkpointsByWave;

        internal CheckpointsManager(ITileCoordinatesMaster tilePositionConverter, LevelData levelData)
        {
            this.tilePositionConverter = tilePositionConverter;
            this.levelData = levelData;
        }

        internal void InitializeCheckpoints()
        {
            playerStartInfo = new Data.Level.ActorStartInfo()
            {
                BlueprintType = Player.BlueprintType,
                TilePosition = levelData.StartCheckpoint.PlayerPosition
            };

            checkpointsByName = levelData.EnemyWaves.Select(CheckpointFromWave)
                .Where(checkpoint => checkpoint != null)
                .ToDictionary(checkpoint => checkpoint.Name, checkpoint => checkpoint);

            checkpointsByWave = checkpointsByName.Values
                .ToDictionary(checkpoint => checkpoint.StartWave, checkpoint => checkpoint);
        }

        internal ActorStartInfo GetPlayerPosition(String checkpointName)
        {
            if (checkpointsByName.ContainsKey(checkpointName))
                return new ActorStartInfo
                {
                    BlueprintType = playerStartInfo.BlueprintType,
                    Position = checkpointsByName[checkpointName].StartPosition
                };
            else
                throw new ArgumentException(nameof(checkpointName));
        }

        internal ArsenalSpecification GetPlayerArsenal(String checkpointName)
        {
            if (checkpointsByName.ContainsKey(checkpointName))
                return checkpointsByName[checkpointName].PlayerArsenal;
            else
                throw new ArgumentException(nameof(checkpointName));
        }

        internal Int32 GetStartWave(String checkpointName)
        {
            if (checkpointsByName.ContainsKey(checkpointName))
                return checkpointsByName[checkpointName].StartWave;
            else
                throw new ArgumentException("checkpointName");
        }

        internal String CheckForCheckpoint(Int32 waveNumber)
        {
            if (checkpointsByWave.TryGetValue(waveNumber, out Checkpoint result))
                return result.Name;
            else
                return null;
        }

        private Checkpoint CheckpointFromWave(EnemyWave ew, Int32 number)
        {
            var specification = number != 0 ? ew.Checkpoint : levelData.StartCheckpoint;
            return FromSpecification(specification, number);
        }

        private Checkpoint FromSpecification(CheckpointSpecification specification, Int32 number)
        {
            if (specification != null)
                return new Checkpoint
                {
                    Name = specification.Name,
                    StartPosition = tilePositionConverter.GetLevelPosition(specification.PlayerPosition),
                    PlayerArsenal = specification.Arsenal,
                    StartWave = number
                };
            else
                return null;
        }
    }
}
