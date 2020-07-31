using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class CheckpointsManagerTests
    {
        private readonly ITileCoordinatesMaster tilePositionConverter = new TilePositionConverterMock();
        private readonly LevelData levelData = InitLevelData();
        private const String PhonyCheckpoint = "PhonyCheckpoint";

        [Test]
        public void TestGetStartWave()
        {
            var checkpointsManager = new CheckpointsManager(tilePositionConverter, levelData);
            checkpointsManager.InitializeCheckpoints();
            Assert.That(checkpointsManager.GetStartWave(CheckpointSpecification.StartCheckpointName), Is.EqualTo(0));
            Assert.That(checkpointsManager.GetStartWave("SecondCheckpoint"), Is.EqualTo(1));
            Assert.That(checkpointsManager.GetStartWave("ThirdCheckpoint"), Is.EqualTo(4));
            Assert.That(checkpointsManager.GetStartWave("LastCheckpoint"), Is.EqualTo(6));
            Assert.Throws<ArgumentException>(() => checkpointsManager.GetStartWave(PhonyCheckpoint));
        }

        [Test]
        public void TestCheckForCheckpoint()
        {
            var checkpointsManager = new CheckpointsManager(tilePositionConverter, levelData);
            checkpointsManager.InitializeCheckpoints();
            Assert.That(checkpointsManager.CheckForCheckpoint(0), Is.EqualTo(CheckpointSpecification.StartCheckpointName));
            Assert.That(checkpointsManager.CheckForCheckpoint(1), Is.EqualTo("SecondCheckpoint"));
            Assert.That(checkpointsManager.CheckForCheckpoint(2), Is.Null);
            Assert.That(checkpointsManager.CheckForCheckpoint(3), Is.Null);
            Assert.That(checkpointsManager.CheckForCheckpoint(4), Is.EqualTo("ThirdCheckpoint"));
            Assert.That(checkpointsManager.CheckForCheckpoint(5), Is.Null);
            Assert.That(checkpointsManager.CheckForCheckpoint(6), Is.EqualTo("LastCheckpoint"));
        }

        [Test]
        public void TestGetPlayerPosition()
        {
            var checkpointsManager = new CheckpointsManager(tilePositionConverter, levelData);
            checkpointsManager.InitializeCheckpoints();
            AssertPlayerPositionAt(checkpointsManager, CheckpointSpecification.StartCheckpointName, 0);
            AssertPlayerPositionAt(checkpointsManager, "SecondCheckpoint", 1);
            AssertPlayerPositionAt(checkpointsManager, "ThirdCheckpoint", 2);
            AssertPlayerPositionAt(checkpointsManager, "LastCheckpoint", 3);
            Assert.Throws<ArgumentException>(() => checkpointsManager.GetPlayerPosition(PhonyCheckpoint));
        }

        private void AssertPlayerPositionAt(CheckpointsManager checkpointsManager, String checkpointName, Int32 position)
        {
            GameModel.ActorStartInfo playerPosition = checkpointsManager.GetPlayerPosition(checkpointName);
            Assert.That(playerPosition.BlueprintType, Is.EqualTo(Player.BlueprintType));
            Assert.That(playerPosition.Position.X, Is.EqualTo(position));
            Assert.That(playerPosition.Position.Y, Is.EqualTo(position));
        }

        private static LevelData InitLevelData()
        {
            var phonyDefaultCheckpoint = new CheckpointSpecification
            {
                Name = PhonyCheckpoint,
                PlayerPosition = new PositionOnTileMap { X = Int32.MaxValue, Y = Int32.MaxValue }
            };
            var secondWaveCheckpoint = new CheckpointSpecification
            {
                Name = "SecondCheckpoint",
                PlayerPosition = new PositionOnTileMap { X = 1, Y = 1 }
            };
            var fifthWaveCheckpoint = new CheckpointSpecification
            {
                Name = "ThirdCheckpoint",
                PlayerPosition = new PositionOnTileMap { X = 2, Y = 2 }
            };
            var seventhWaveCheckpont = new CheckpointSpecification
            {
                Name = "LastCheckpoint",
                PlayerPosition = new PositionOnTileMap { X = 3, Y = 3 }
            };
            return new LevelData
            {
                StartCheckpoint = new CheckpointSpecification
                {
                    Name = "Default",
                    PlayerPosition = new PositionOnTileMap { X = 0, Y = 0 }
                },
                EnemyWaves = new List<EnemyWave>
                {
                    new EnemyWave { Checkpoint = phonyDefaultCheckpoint },
                    new EnemyWave { Checkpoint = secondWaveCheckpoint },
                    new EnemyWave { Checkpoint = null },
                    new EnemyWave { Checkpoint = null },
                    new EnemyWave { Checkpoint = fifthWaveCheckpoint },
                    new EnemyWave { Checkpoint = null },
                    new EnemyWave { Checkpoint = seventhWaveCheckpont }
                },
                ObstaclesTilePositions = new Dictionary<String, PositionOnTileMap[]>(),
                TileMap = "SomeTileMap"
            };
        }
    }

    internal class TilePositionConverterMock : ITileCoordinatesMaster
    {
        public Rectangle Bounds => Rectangle.Empty;

        public Vector2 GetLevelPosition(PositionOnTileMap tilePosition)
        {
            return new Vector2 { X = tilePosition.X, Y = tilePosition.Y };
        }
    }
}
