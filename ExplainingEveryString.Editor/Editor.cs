using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Editor
{
    internal class Editor
    {
        private List<IEditorMode> modes;
        private Int32 modeIndex = 0;
        private IEditorMode CurrentMode => modes[modeIndex];
        private CoordinatesConverter coordinatesConverter;
        private LevelData levelData;
        private SpriteFont font;
        private Texture2D cursor;

        internal event EventHandler<LevelChangedEventArgs> LevelChanged;

        internal Editor(LevelData levelData, ContentManager content, IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper map)
        {
            this.levelData = levelData;
            InputProcessor.Instance.MouseScrolled += MouseScrolled;
            InputProcessor.Instance.MouseButtonPressed += MouseButtonPressed;
            InputProcessor.Instance.KeyPressed += KeyPressed;
            this.font = content.Load<SpriteFont>(@"TimeFont");
            this.cursor = content.Load<Texture2D>(@"Sprites/Editor/Cursor");

            this.coordinatesConverter = new CoordinatesConverter(screenCoordinatesMaster, map);
            this.modes = InitEditorModes(content);
        }

        private void KeyPressed(Object sender, KeyPressedEventArgs e)
        {
            if (e.PressedKey == Keys.U)
                CurrentMode?.Unselect();
            if (e.PressedKey == Keys.Q)
                CurrentMode?.SelectedEditableChange(-1);
            if (e.PressedKey == Keys.E)
                CurrentMode?.SelectedEditableChange(+1);
            if (e.PressedKey == Keys.Delete && CurrentMode != null)
            {
                CurrentMode.DeleteCurrentlySelected();
                levelData = CurrentMode.SaveChanges();
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }

            if (e.PressedKey == Keys.Up)
                if (CurrentMode?.ParentModes != null)
                    modes = CurrentMode.ParentModes;
            if (e.PressedKey == Keys.Down)
                if (CurrentMode?.CurrentDerivativeModes != null)
                    modes = CurrentMode.CurrentDerivativeModes;
            if (e.PressedKey == Keys.Left)
                if (modeIndex > 0)
                    modeIndex -= 1;
            if (e.PressedKey == Keys.Right)
                if (modeIndex < modes.Count - 1)
                    modeIndex += 1;
        }

        private void MouseButtonPressed(Object sender, MouseButtonPressedEventArgs e)
        {
            if (e.PressedButton == MouseButtons.Left && CurrentMode != null)
            {
                if (CurrentMode.SelectedEditableIndex == null)
                    CurrentMode.Add(e.MouseScreenPosition);
                else
                    CurrentMode.MoveSelected(e.MouseScreenPosition);

                levelData = CurrentMode.SaveChanges();
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawString(spriteBatch, $"We now in {CurrentMode?.ModeName} mode", 1);
            DrawString(spriteBatch, $"{CurrentMode?.CurrentEditableType}", 2);
            if (CurrentMode?.SelectedEditableIndex != null)
                DrawString(spriteBatch, $"Selected #{CurrentMode.SelectedEditableIndex}", 3);

            var mousePosition = InputProcessor.Instance.MousePosition;
            if (CurrentMode != null)
            {
                var levelPosition = coordinatesConverter.GetLevelPosition(mousePosition);
                DrawString(spriteBatch, $"({levelPosition.X}, {levelPosition.Y}) + {levelPosition.Offset})", 4);
            }

            CurrentMode?.Draw(spriteBatch);

            
            spriteBatch.Draw(cursor, mousePosition, null, Color.White, 0, 
                new Vector2(cursor.Width / 2, cursor.Height / 2), 1, SpriteEffects.None, 0);
        }

        private void DrawString(SpriteBatch spriteBatch, String message, Int32 line) =>
            spriteBatch.DrawString(font, message, new Vector2(16, 16 * line), Color.Black);

        private void MouseScrolled(Object sender, MouseScrolledEventArgs e)
        {
            CurrentMode?.EditableTypeChange(e.ScrollDifference);
        }

        private List<IEditorMode> InitEditorModes(ContentManager content)
        {
            var blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var blueprintDisplayer = new BlueprintDisplayer(content, blueprintsLoader, AssetsMetadataAccess.GetLoader().Load());
            var rectangleCornersDisplayer = new RectangleCornersDisplayer(content);
            var result = new List<IEditorMode>();
            result.Add(new ObstaclesEditorMode(levelData, coordinatesConverter, blueprintDisplayer, blueprintsLoader));
            result.Add(new EnemyWavesEditorMode(levelData, result, coordinatesConverter, 
                rectangleCornersDisplayer, blueprintDisplayer, blueprintsLoader));
            return result;
        }
    }

    internal class LevelChangedEventArgs
    {
        internal LevelData UpdatedLevel { get; set; }
    }
}
