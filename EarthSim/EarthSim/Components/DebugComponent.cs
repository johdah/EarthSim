using System;
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

        public String mode { get; set; }
        public Vector3 cameraPos { get; set; }
        public Vector2 playerPos { get; set; }

        private int frameRate = 0;
        private int frameCounter = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        
        string camPos = "";
        string debug = "";
        string fps = "";
        string playPos = "";
        //public PlayerObject.Directions playerDir { get; set; }

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
            fps = string.Format("FPS: {0}", frameRate);

            /*string direction = "";
            switch (playerDir)
            {
                case PlayerObject.Directions.North:
                    direction = "North";
                    break;
                case PlayerObject.Directions.East:
                    direction = "East";
                    break;
                case PlayerObject.Directions.South:
                    direction = "South";
                    break;
                case PlayerObject.Directions.West:
                    direction = "West";
                    break;
            }*/

            debug = string.Format("Mode: {0}", mode);
            camPos = string.Format("Camera Position: {0}", cameraPos.ToString());
            playPos = string.Format("Player Position: {0}", playerPos.ToString());
            //string playDir = string.Format("Player Direction: {0}", direction);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 13), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 12), Color.White);

            spriteBatch.DrawString(spriteFont, debug, new Vector2(33, 33), Color.Black);
            spriteBatch.DrawString(spriteFont, debug, new Vector2(32, 32), Color.White);

            spriteBatch.DrawString(spriteFont, camPos, new Vector2(33, 53), Color.Black);
            spriteBatch.DrawString(spriteFont, camPos, new Vector2(32, 52), Color.White);

            spriteBatch.DrawString(spriteFont, playPos, new Vector2(33, 73), Color.Black);
            spriteBatch.DrawString(spriteFont, playPos, new Vector2(32, 72), Color.White);

            /*spriteBatch.DrawString(spriteFont, playDir, new Vector2(33, 93), Color.Black);
            spriteBatch.DrawString(spriteFont, playDir, new Vector2(32, 92), Color.White);*/

            spriteBatch.End();
        }
    }
}
