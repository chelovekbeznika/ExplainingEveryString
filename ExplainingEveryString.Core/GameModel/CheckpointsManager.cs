using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class CheckpointsManager
    {
        internal const String StartCheckpointName = "Default";

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
            playerStartInfo = levelData.PlayerPosition;

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
                throw new ArgumentException("checkpointName");
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
            if (number == 0)
            {
                return new Checkpoint
                {
                    Name = StartCheckpointName,
                    StartPosition = tilePositionConverter.GetLevelPosition(playerStartInfo.TilePosition),
                    StartWave = 0
                };
            }
            else if (ew.Checkpoint != null)
            {
                return new Checkpoint
                {
                    Name = ew.Checkpoint.Name,
                    StartPosition = tilePositionConverter.GetLevelPosition(ew.Checkpoint.PlayerPosition),
                    StartWave = number
                };
            }
            else 
                return null;
        }
    }
}
