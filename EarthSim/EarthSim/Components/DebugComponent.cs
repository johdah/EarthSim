﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EarthSim.Components
{
    public class DebugComponent : DrawableGameComponent
    {
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public Vector3 cameraPos { get; set; }
        public String mode { get; set; }
        public Vector2 playerPos { get; set; }
        public float playerSpeed { get; set; }
        public float seaLevel { get; set; }

        private int frameRate = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        
        string camPos = "";
        string debug = "";
        string fps = "";
        string playPos = "";
        string playSpeed = "";
        string seaLvl = "";

        public DebugComponent(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";

            mode = "Unknown";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>("Fonts/gamefont");
        }

        protected override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            frameCounter++;

            camPos = string.Format("Camera Position: {0}", cameraPos.ToString());
            debug = string.Format("Mode: {0}", mode);
            fps = string.Format("FPS: {0}", frameRate);
            playPos = string.Format("Player Position: {0}", playerPos.ToString());
            playSpeed = string.Format("Player Speed: {0}", playerSpeed.ToString());
            seaLvl = string.Format("Sea Level: {0}", seaLevel.ToString());
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            int leftIndent = 10;
            int count = 11;

            spriteBatch.DrawString(spriteFont, fps, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(leftIndent, count), Color.White);
            count += 21;

            spriteBatch.DrawString(spriteFont, debug, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, debug, new Vector2(leftIndent, count), Color.White);
            count += 21;

            spriteBatch.DrawString(spriteFont, camPos, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, camPos, new Vector2(leftIndent, count), Color.White);
            count += 21;

            spriteBatch.DrawString(spriteFont, playPos, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, playPos, new Vector2(leftIndent, count), Color.White);
            count += 21;

            spriteBatch.DrawString(spriteFont, playSpeed, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, playSpeed, new Vector2(leftIndent, count), Color.White);
            count += 21;

            spriteBatch.DrawString(spriteFont, seaLvl, new Vector2(leftIndent + 1, count--), Color.Black);
            spriteBatch.DrawString(spriteFont, seaLvl, new Vector2(leftIndent, count), Color.White);

            spriteBatch.End();
        }
    }
}
