﻿using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Editor
{
    internal class Editor
    {
        private IEditorMode[] modes;
        private Int32 modeIndex = 0;
        private IEditorMode CurrentMode => modes[modeIndex];
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

            this.modes = InitEditorModes(content, screenCoordinatesMaster, map);
            foreach (var mode in modes)
                mode.Load(levelData);
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
                levelData = CurrentMode.SaveChanges(levelData);
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }
            if (e.PressedKey == Keys.D0)
                modeIndex = 0;
            if (e.PressedKey == Keys.D1)
                modeIndex = 1;
        }

        private void MouseButtonPressed(Object sender, MouseButtonPressedEventArgs e)
        {
            if (e.PressedButton == MouseButtons.Left && CurrentMode != null)
            {
                if (CurrentMode.SelectedEditableIndex == null)
                    CurrentMode.Add(e.MouseScreenPosition);
                else
                    CurrentMode.MoveSelected(e.MouseScreenPosition);

                levelData = CurrentMode.SaveChanges(levelData);
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, $"We now in {CurrentMode?.ModeName} mode", new Vector2(16, 16), Color.White);
            spriteBatch.DrawString(font, $"{CurrentMode?.CurrentEditableType}", new Vector2(16, 32), Color.White);
            if (CurrentMode?.SelectedEditableIndex != null)
                spriteBatch.DrawString(font, $"Selected #{CurrentMode.SelectedEditableIndex}", new Vector2(16, 48), Color.White);
            CurrentMode?.Draw(spriteBatch);

            var mousePosition = InputProcessor.Instance.MousePosition;
            spriteBatch.Draw(cursor, mousePosition, null, Color.White, 0, 
                new Vector2(cursor.Width / 2, cursor.Height / 2), 1, SpriteEffects.None, 0);
        }

        private void MouseScrolled(Object sender, MouseScrolledEventArgs e)
        {
            CurrentMode?.EditableTypeChange(e.ScrollDifference);
        }

        private IEditorMode[] InitEditorModes(ContentManager content, IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper map)
        {
            var blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var blueprintsDisplayer = new BlueprintDisplayer(content, blueprintsLoader, AssetsMetadataAccess.GetLoader().Load());
            return new IEditorMode[]
            {
                new ObstaclesEditorMode(screenCoordinatesMaster, map, blueprintsDisplayer, blueprintsLoader),
                new EnemyPositionEditorMode(screenCoordinatesMaster, map, blueprintsDisplayer, blueprintsLoader)
            };
        }
    }

    internal class LevelChangedEventArgs
    {
        internal LevelData UpdatedLevel { get; set; }
    }
}
