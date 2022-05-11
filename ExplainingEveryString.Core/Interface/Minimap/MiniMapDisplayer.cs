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
        private readonly TiledMapRenderer renderer;
        private readonly GraphicsDevice graphicsDevice;
        private readonly TiledMap tiledMap;
        private SpriteData playerDot;
        private SpriteData enemyDot;
        private SpriteData bossDot;

        internal MiniMapDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, TileWrapper map, Game gameApp)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.graphicsDevice = gameApp.GraphicsDevice;
            this.renderer = new TiledMapRenderer(graphicsDevice, map.TiledMap);
            this.tiledMap = map.TiledMap;
            this.minimapCoordinatesMaster = new MinimapCoordinatesMaster(map);
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        internal void Draw(InterfaceInfo info)
        {
            DrawMap();
            DrawDots(info);
        }

        private void DrawDots(InterfaceInfo info)
        {
            var playerPosition = minimapCoordinatesMaster.ToScreenMinimap(info.Player.LevelPosition);
            interfaceSpriteDisplayer.Draw(playerDot, playerPosition - new Vector2(playerDot.Width / 2, playerDot.Height / 2));

            foreach (var enemy in info.Enemies)
            {
                var enemyPosition = minimapCoordinatesMaster.ToScreenMinimap(enemy.LevelPosition);
                interfaceSpriteDisplayer.Draw(enemyDot, enemyPosition - new Vector2(enemyDot.Width / 2, enemyDot.Height / 2));
            }

            foreach (var boss in info.Bosses ?? Enumerable.Empty<EnemyInterfaceInfo>())
            {
                var bossPosition = minimapCoordinatesMaster.ToScreenMinimap(boss.LevelPosition);
                interfaceSpriteDisplayer.Draw(bossDot, bossPosition - new Vector2(bossDot.Width / 2, bossDot.Height / 2));
            }
        }

        private void DrawMap()
        {
            var savedOpacity = new Dictionary<String, Single>();
            foreach (var layer in tiledMap.Layers)
            {
                savedOpacity.Add(layer.Name, layer.Opacity);
                layer.Opacity = 0.5F;
            }

            renderer.Draw(minimapCoordinatesMaster.PositioningMatrix);

            foreach (var layer in tiledMap.Layers)
                layer.Opacity = savedOpacity[layer.Name];
        }

        public String[] GetSpritesNames() => new[] { "MinimapPlayer", "MinimapEnemy", "MinimapBoss" };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.playerDot = TextureLoadingHelper.GetSprite(sprites, "MinimapPlayer");
            this.enemyDot = TextureLoadingHelper.GetSprite(sprites, "MinimapEnemy");
            this.bossDot = TextureLoadingHelper.GetSprite(sprites, "MinimapBoss");
        }
    }
}
