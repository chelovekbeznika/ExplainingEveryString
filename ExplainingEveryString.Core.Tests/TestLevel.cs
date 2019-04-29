using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests
{
    internal class TestLevel
    {
        internal Level Level { get; private set; }
        internal ActiveGameObjectsStorage GameObjectsStorage { get; private set; }
        private readonly GameObjectsFactory testFactory;

        internal TestLevel()
        {
            ConfigurationAccess.InitializeConfig();
            LevelData levelData = new LevelData
            {
                PlayerPosition = new GameObjectStartPosition
                {
                    Position = new Vector2 { X = 0, Y = 0 },
                    Angle = 0
                },
                EnemiesPositions = new Dictionary<String, List<GameObjectStartPosition>>
                {
                    { "Mine", new List<GameObjectStartPosition>
                        {
                            new GameObjectStartPosition
                            {
                                Position = new Vector2 { X = -100, Y = 200 },
                                Angle = 0
                            }
                        }
                    }
                },
                WallsPositions = new Dictionary<String, List<Vector2>>()
            };
            HardCodeBlueprintsLoader loader = new HardCodeBlueprintsLoader();
            loader.Load();
            testFactory = new GameObjectsFactory(loader);
            Level = new Level(testFactory, levelData);
            testFactory.Level = Level;
            GameObjectsStorage = new ActiveGameObjectsStorage(testFactory, levelData);
            GameObjectsStorage.InitializeGameObjects();
            
        }
    }
}
