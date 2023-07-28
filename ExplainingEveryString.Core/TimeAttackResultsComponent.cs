using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.AssetsMetadata;
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
        private SpriteData newRecordNotice;
        private Dictionary<String, Texture2D> levelButtons;
        private String newRecordLevel = null;

        private CustomFont TimeFont => (Game as EesGame).FontsStorage.LevelTime;

        public TimeAttackResultsComponent(EesGame game, LevelSequenceSpecification levelSequenceSpecification) : 
            base(game, 1.0F, 60F, 1)
        {
            this.levelSequenceSpecification = levelSequenceSpecification;

            this.DrawOrder = ComponentsOrder.Cutscene;
            this.UpdateOrder = ComponentsOrder.Cutscene;
        }

        internal void NotifyNewRecord(String levelName)
        {
            newRecordLevel = levelName;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");

            levelButtons = levelSequenceSpecification.Levels.ToDictionary(
                keySelector: l => l.LevelData, 
                elementSelector: l => Game.Content.Load<Texture2D>(l.ButtonSprite));

            var metadataLoader = AssetsMetadataAccess.GetLoader();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, metadataLoader);
            var recordSpriteName = @"Sprites/Menu/NewRecordNotice";
            var sprites = spriteDataBuilder.Build(new[] { recordSpriteName });
            newRecordNotice = sprites[recordSpriteName];
        }

        protected override void DrawImage(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            var currentResult = GetCurrentProgress().LevelRecords;
            var nextButtonPlaceholder = new Vector2(Displaying.Constants.TargetWidth / 2, UpperPartHeight);
            foreach (var level in levelSequenceSpecification.Levels)
            {
                var levelName = level.LevelData;
                if (currentResult.ContainsKey(levelName))
                {
                    var levelResult = currentResult[levelName];
                    DrawLevelResults(spriteBatch, levelName, levelResult, ref nextButtonPlaceholder);
                }
            }
        }

        private void DrawLevelResults(SpriteBatch spriteBatch, String levelName, 
            Single levelResult, ref Vector2 nextButtonPlaceholder)
        {
            var isNewRecord = levelName == newRecordLevel;
            var timeString = GameTimeHelper.ToTimeString(levelResult);
            var textSize = TimeFont.GetSize(timeString);
                    
            var currentButton = levelButtons[levelName];
            var wholeWidth = textSize.X + BetweenElements + currentButton.Width;
            var currentButtonPosition = nextButtonPlaceholder + new Vector2(-wholeWidth / 2, 0);
            var currentTextPosition = new Vector2(
                x: currentButtonPosition.X + currentButton.Width + BetweenElements,
                y: currentButtonPosition.Y + currentButton.Height / 2 - textSize.Y / 2);

            spriteBatch.Draw(currentButton, currentButtonPosition, Color.White);
            TimeFont.Draw(spriteBatch, currentTextPosition, timeString);

            if (isNewRecord)
            {
                var recordNoticePosition = new Vector2(
                    x: currentTextPosition.X + textSize.X + BetweenElements, 
                    y: nextButtonPlaceholder.Y);
                var partToDraw = AnimationHelper.GetDrawPart(newRecordNotice, FrameTime);
                spriteBatch.Draw(newRecordNotice.Sprite, recordNoticePosition, partToDraw, Color.White);
            }

            nextButtonPlaceholder += new Vector2(0, BetweenRows + currentButton.Height);
        }

        private GameProgress GetCurrentProgress()
        {
            var profileNumber = ConfigurationAccess.GetCurrentConfig().SaveProfile;
            return GameProgressAccess.Load(profileNumber);
        }
    }
}
