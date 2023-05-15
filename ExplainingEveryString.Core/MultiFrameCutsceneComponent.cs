﻿using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ExplainingEveryString.Core
{
    internal class MultiFrameCutsceneComponent : CutSceneComponent
    {
        private Texture2D[] frames;
        private String name;

        public MultiFrameCutsceneComponent(Game game, String name, CutsceneSpecification specification) 
            : base(game, specification.MinFrameTime, specification.MaxFrameTime, specification.FramesCount)
        {
            this.name = name;
            this.DrawOrder = ComponentsOrder.Cutscene;
            this.UpdateOrder = ComponentsOrder.Cutscene;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            frames = Enumerable.Range(0, FramesCount).Select(frameNumber =>
            {
                var path = @$"Sprites/Cutscenes/{name}/{frameNumber}";
                return Game.Content.Load<Texture2D>(path);
            }).ToArray();
        }

        protected override void DrawCutScene(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            spriteBatch.Draw(frames[frameNumber], Vector2.Zero, Color.White);
        }
    }
}
