using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core
{
    internal class TimeAttackResultsComponent : StaticImagesSequenceComponent
    {
        private const Int32 UpperPartHeight = 128;
        private const Int32 BetweenRows = 12;
        private const Int32 BetweenElements = 12;

        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private Texture2D background;
        private Dictionary<String, Texture2D> levelButtons;
        private CustomFont timeFont => (Game as EesGame).FontsStorage.LevelTime;

        public TimeAttackResultsComponent(EesGame game, LevelSequenceSpecification levelSequenceSpecification) : 
            base(game, 1.0F, 60F, 1)
        {
            this.levelSequenceSpecification = levelSequenceSpecification;

            this.DrawOrder = ComponentsOrder.Cutscene;
            this.UpdateOrder = ComponentsOrder.Cutscene;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");
            levelButtons = levelSequenceSpecification.Levels.ToDictionary(
                keySelector: l => l.LevelData, 
                elementSelector: l => Game.Content.Load<Texture2D>(l.ButtonSprite));
        }

        protected override void DrawImage(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            DrawLevelResults(spriteBatch);
        }

        private void DrawLevelResults(SpriteBatch spriteBatch)
        {
            var currentResult = GetCurrentProgress().LevelRecords;
            var nextButtonPlaceholder = new Vector2(Constants.TargetWidth / 2, UpperPartHeight);
            foreach (var level in levelSequenceSpecification.Levels)
            {
                var levelName = level.LevelData;
                if (currentResult.ContainsKey(levelName))
                {
                    var levelResult = currentResult[levelName];
                    var timeSpan = TimeSpan.FromSeconds(levelResult);
                    var timeString = $"{timeSpan:h\\:mm\\:ss\\.ff}";
                    var textSize = timeFont.GetSize(timeString);
                    

                    var currentButton = levelButtons[levelName];
                    var wholeWidth = textSize.X + BetweenElements + currentButton.Width;
                    var currentButtonPosition = nextButtonPlaceholder + new Vector2(-wholeWidth / 2, 0);
                    var currentTextPosition = new Vector2(
                        x: currentButtonPosition.X + currentButton.Width + BetweenElements,
                        y: currentButtonPosition.Y + currentButton.Height / 2 - textSize.Y / 2);

                    spriteBatch.Draw(currentButton, currentButtonPosition, Color.White);
                    timeFont.Draw(spriteBatch, currentTextPosition, timeString);

                    nextButtonPlaceholder += new Vector2(0, BetweenRows + currentButton.Height);
                }
            }
        }

        private GameProgress GetCurrentProgress()
        {
            var profileNumber = ConfigurationAccess.GetCurrentConfig().SaveProfile;
            return GameProgressAccess.Load(profileNumber);
        }
    }
}
