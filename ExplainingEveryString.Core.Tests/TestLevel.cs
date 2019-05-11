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
        internal ActiveActorsStorage ActorsStorage { get; private set; }
        private readonly ActorsFactory testFactory;

        internal TestLevel()
        {
            ConfigurationAccess.InitializeConfig();
            LevelData levelData = new LevelData
            {
                PlayerPosition = new ActorStartPosition
                {
                    Position = new Vector2 { X = 0, Y = 0 },
                    Angle = 0
                },
                EnemiesPositions = new Dictionary<String, List<ActorStartPosition>>
                {
                    { "Mine", new List<ActorStartPosition>
                        {
                            new ActorStartPosition
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
            testFactory = new ActorsFactory(loader);
            Level = new Level(testFactory, levelData);
            testFactory.Level = Level;
            ActorsStorage = new ActiveActorsStorage(testFactory, levelData);
            ActorsStorage.InitializeActors();
            
        }
    }
}
