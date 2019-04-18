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
                PlayerPosition = new Vector2 { X = 0, Y = 0 },
                EnemiesPositions = new Dictionary<String, List<Vector2>>
                {
                    { "Mine", new List<Vector2> { new Vector2 (-100, 200)} }
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
