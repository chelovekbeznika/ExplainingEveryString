using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Interface.Displayers;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MiniMapDisplayer : IDisplayer
    {
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly MinimapCoordinatesMaster minimapCoordinatesMaster;
        private readonly TiledMapRenderer minimapRenderer;
        private readonly GraphicsDevice graphicsDevice;
        private SpriteData playerDot;
        private SpriteData enemyDot;
        private SpriteData bossDot;
        private SpriteData background;

        internal MiniMapDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, TileWrapper map, Game gameApp)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.graphicsDevice = gameApp.GraphicsDevice;
            this.minimapRenderer = new TiledMapRenderer(graphicsDevice, map.TiledMap);
            this.minimapCoordinatesMaster = new MinimapCoordinatesMaster(map);
        }

        internal void Update(GameTime gameTime)
        {
            minimapRenderer.Update(gameTime);
        }

        internal void Draw(InterfaceInfo info)
        {
            DrawBackground();
            minimapRenderer.Draw(minimapCoordinatesMaster.PositioningMatrix);
            DrawDots(info);
        }

        private void DrawBackground()
        {
            //We draw only parts which do not overlap minimap. TiledMap messes with drawing order, unfortunately.
            var backgroundPosition = new Vector2(Constants.TargetWidth - Constants.MinimapSize, Constants.TargetHeight - Constants.MinimapSize);
            var (firstHalf, secondHalf, coeff) = minimapCoordinatesMaster.BackgroundPartsToDraw;
            interfaceSpriteDisplayer.Draw(background, backgroundPosition, firstHalf, coeff, true);
            interfaceSpriteDisplayer.Draw(background, backgroundPosition, secondHalf, coeff, true);
        }

        private void DrawDots(InterfaceInfo info)
        {
            DrawDot(info.Player.LevelPosition, playerDot);
            foreach (var enemy in info.EnemiesLevelPositions)
                DrawDot(enemy, enemyDot);
            foreach (var boss in info.BossesLevelPositions ?? Enumerable.Empty<Vector2>())
                DrawDot(boss, bossDot);
        }

        private void DrawDot(Vector2 fighterPosition, SpriteData dot)
        {
            var position = minimapCoordinatesMaster.ToScreenMinimap(fighterPosition);
            interfaceSpriteDisplayer.Draw(dot, position - new Vector2(dot.Width / 2, dot.Height / 2), true);
        }

        public String[] GetSpritesNames() => new[] { "MinimapPlayer", "MinimapEnemy", "MinimapBoss", "MinimapBackground" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.playerDot = TextureLoadingHelper.GetSprite(sprites, "MinimapPlayer");
            this.enemyDot = TextureLoadingHelper.GetSprite(sprites, "MinimapEnemy");
            this.bossDot = TextureLoadingHelper.GetSprite(sprites, "MinimapBoss");
            this.background = TextureLoadingHelper.GetSprite(sprites, "MinimapBackground");
        }
    }
}
