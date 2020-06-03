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
        private IEditorMode EditorMode => modes[modeIndex];
        private ICustomParameterEditor CustomParameterEditor => EditorMode as ICustomParameterEditor;
        private CoordinatesConverter coordinatesConverter;
        private LevelData levelData;
        private SpriteFont font;
        private Texture2D cursor;

        internal event EventHandler<LevelChangedEventArgs> LevelChanged;

        internal Editor(LevelData levelData, ContentManager content, CoordinatesConverter coordinatesConverter)
        {
            this.levelData = levelData;
            InputProcessor.Instance.MouseScrolled += MouseScrolled;
            InputProcessor.Instance.MouseButtonPressed += MouseButtonPressed;
            InputProcessor.Instance.KeyPressed += KeyPressed;
            this.font = content.Load<SpriteFont>(@"TimeFont");
            this.cursor = content.Load<Texture2D>(@"Sprites/Editor/Cursor");

            this.coordinatesConverter = coordinatesConverter;
            this.modes = InitEditorModes(content);
        }

        private void KeyPressed(Object sender, KeyPressedEventArgs e)
        {
            var levelChanged = false;
            switch (e.PressedKey)
            {
                case Keys.U: EditorMode?.Unselect(); break;
                case Keys.Q: EditorMode?.SelectedEditableChange(-1); break;
                case Keys.E: EditorMode?.SelectedEditableChange(+1); break;
                case Keys.Delete: EditorMode.DeleteCurrentlySelected(); levelChanged = true; break;
                case Keys.F: CustomParameterEditor?.ToPreviousValue(); levelChanged = true; break;
                case Keys.R: CustomParameterEditor?.ToNextValue(); levelChanged = true; break;

                case Keys.D1: (EditorMode as DoorsEditorMode)?.PushSelectedUp(1); levelChanged = true; break;
                case Keys.D2: (EditorMode as DoorsEditorMode)?.PushSelectedUp(2); levelChanged = true; break;
                case Keys.D3: (EditorMode as DoorsEditorMode)?.PushSelectedUp(3); levelChanged = true; break;
                case Keys.D4: (EditorMode as DoorsEditorMode)?.PushSelectedUp(4); levelChanged = true; break;
                case Keys.D5: (EditorMode as DoorsEditorMode)?.PushSelectedUp(5); levelChanged = true; break;
                case Keys.D6: (EditorMode as DoorsEditorMode)?.PushSelectedUp(6); levelChanged = true; break;
                case Keys.D7: (EditorMode as DoorsEditorMode)?.PushSelectedUp(7); levelChanged = true; break;
                case Keys.D8: (EditorMode as DoorsEditorMode)?.PushSelectedUp(8); levelChanged = true; break;
                case Keys.D9: (EditorMode as DoorsEditorMode)?.PushSelectedUp(9); levelChanged = true; break;

                case Keys.Up:
                    if (EditorMode?.ParentModes != null)
                    {
                        modes = EditorMode.ParentModes;
                        modeIndex = 0;
                    }
                    break;
                case Keys.Down:
                    if (EditorMode?.CurrentDerivativeModes != null)
                    {
                        modes = EditorMode.CurrentDerivativeModes;
                        modeIndex = 0;
                    }
                    break;
                case Keys.Left: if (modeIndex > 0) modeIndex -= 1; break;
                case Keys.Right: if (modeIndex < modes.Count - 1) modeIndex += 1; break;
            }

            if (levelChanged)
            {
                levelData = EditorMode.SaveChanges();
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }
        }

        private void MouseButtonPressed(Object sender, MouseButtonPressedEventArgs e)
        {
            if (e.PressedButton == MouseButtons.Left && EditorMode != null)
            {
                if (EditorMode.SelectedEditableIndex == null)
                    EditorMode.Add(e.MouseScreenPosition);
                else
                    EditorMode.MoveSelected(e.MouseScreenPosition);

                levelData = EditorMode.SaveChanges();
                LevelChanged?.Invoke(this, new LevelChangedEventArgs { UpdatedLevel = levelData });
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            DrawString(spriteBatch, $"We now in {EditorMode?.ModeName} mode", 1);
            DrawString(spriteBatch, $"{EditorMode?.CurrentEditableType}", 2);
            if (EditorMode?.SelectedEditableIndex != null)
                DrawString(spriteBatch, $"Selected #{EditorMode.SelectedEditableIndex}", 3);

            var mousePosition = InputProcessor.Instance.MousePosition;
            if (EditorMode != null)
            {
                var levelPosition = coordinatesConverter.ScreenToTile(mousePosition);
                DrawString(spriteBatch, $"({levelPosition.X}, {levelPosition.Y}) + {levelPosition.Offset})", 4);
            }
            if (CustomParameterEditor?.SelectedEditableIndex != null)
                DrawString(spriteBatch, $"{CustomParameterEditor.ParameterName} is {CustomParameterEditor.CurrentParameterValue}", 5);

            EditorMode?.Draw(spriteBatch);        
            spriteBatch.Draw(cursor, mousePosition, null, Color.White, 0, 
                new Vector2(cursor.Width / 2, cursor.Height / 2), 1, SpriteEffects.None, 0);
        }

        private void DrawString(SpriteBatch spriteBatch, String message, Int32 line) =>
            spriteBatch.DrawString(font, message, new Vector2(16, 16 * line), Color.Black);

        private void MouseScrolled(Object sender, MouseScrolledEventArgs e)
        {
            EditorMode?.EditableTypeChange(e.ScrollDifference);
        }

        private List<IEditorMode> InitEditorModes(ContentManager content)
        {
            var blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var assetsMetadata = AssetsMetadataAccess.GetLoader().Load();
            var editableDisplayingCenter = new EditableDisplayingCenter(content, blueprintsLoader, assetsMetadata, coordinatesConverter);
            var result = new List<IEditorMode>();
            result.Add(new EnemyWavesEditorMode(levelData, result, editableDisplayingCenter));
            result.Add(new ObstaclesEditorMode(levelData, editableDisplayingCenter));
            return result;
        }
    }

    internal class LevelChangedEventArgs
    {
        internal LevelData UpdatedLevel { get; set; }
    }
}
