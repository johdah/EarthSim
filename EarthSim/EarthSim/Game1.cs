using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EarthSim.Components;
using EarthSim.Entities.Concrete;
using EarthSim.Entities;
using EarthSim.Components.Input;
using EarthSim.Entities.Abstract;

namespace EarthSim
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private DebugComponent _debug;
        private InputComponent _input;

        private Camera camera;
        private FlyingCamera fcamera;
        private BasicEffect effect;
        private Matrix projection;
        private Matrix view;
        private Matrix world;

        private EarthEntity earthEntity;
        private OceanEntity oceanEntity;
        private SkyEntity skyEntity;
        private AbstractPlayerEntity tankEntity;

        private bool isPlayerMode = false;        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = true;

            _debug = new DebugComponent(this);
            Components.Add(_debug);
            Services.AddService(typeof(DebugComponent), _debug);

            _input = new InputComponent(this);
            Components.Add(_input);
            Services.AddService(typeof(InputComponent), _input);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rs;

            fcamera = new FlyingCamera();

            InputHandler ip = new InputHandler(this);
            Components.Add(ip);
            Services.AddService(typeof(IInputHandler), ip);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            effect = new BasicEffect(GraphicsDevice);
            camera = new Camera(GraphicsDevice);            

            earthEntity = new EarthEntity(this, 25f, this.Content.Load<Texture2D>("Entities/earthTexture"));
            oceanEntity = new OceanEntity(this, 25.15f, this.Content.Load<Texture2D>("Entities/oceanTexture"));
            //skyEntity = new SkyEntity(this, 5f, this.Content.Load<Texture2D>("Entities/skyTexture"));
            tankEntity = new BasicTankEntity(this, this.Content.Load<Model>("Entities/Tank/tank"), earthEntity);

            world = Matrix.Identity;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //Read input
            IInputHandler inputHandler = (IInputHandler)Services.GetService(typeof(IInputHandler));
            inputAction(inputHandler.getUnhandledActions(), gameTime.ElapsedGameTime.Milliseconds);

            //To make the camera mov   
            if (isPlayerMode)
                camera.Update(tankEntity);
            else
                camera.Update(fcamera.Position, fcamera.Rotation);

            //_input.Update();

            earthEntity.Update(gameTime);
            oceanEntity.Update(gameTime);
            if (oceanEntity.GetScale() > 1.01364851)
                oceanEntity.SetScale(1f);
            else
                oceanEntity.SetScale(oceanEntity.GetScale() + 0.000001f);
            tankEntity.Update(gameTime);

            //camera.Update(earthEntity);

            _debug.cameraPos = fcamera.Position;
            _debug.playerPos = tankEntity.GetPosition();
            //_debug.playerDir = tank.GetDirection();

            base.Update(gameTime);
        }

        private void inputAction(List<ActionType> actions, float elapsedTime)
        {
            foreach (var action in actions)
            {
                if (action == ActionType.Quit)
                    this.Exit();
                //if (action == ActionType.IncreaseSealevel && sealevel < 1f)
                ////    sealevel += 0.01f;
                //if (action == ActionType.DecreaseSealevel && sealevel > -5f)
                //    sealevel -= 0.01f;
                if (action == ActionType.SwitchMode)
                {
                    if (this.isPlayerMode) this.isPlayerMode = false;
                    else this.isPlayerMode = true;
                }

                if (isPlayerMode)
                {
                    tankEntity.performAction(action, elapsedTime);
                }
                else
                {

                    fcamera.PerformAction(action, elapsedTime);
                }

                //airplane.PerformAction(action, elapsedTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
       {  
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;   

            world = Matrix.Identity;
            effect.Projection = camera.ViewProjectionMatrix;
            effect.View = camera.ViewMatrix;
            effect.World = world;
            effect.TextureEnabled = true;

            earthEntity.Draw(effect);
            oceanEntity.Draw(effect);
            //skyEntity.Draw(effect);
            //tankEntity.Draw(ref world, ref view, ref projection, ref effect);
            tankEntity.Draw(world, camera.ViewMatrix, camera.ViewProjectionMatrix, effect);
            //tankEntity.Draw(world, camera.ViewMatrix, camera.ViewProjectionMatrix, effect);


            base.Draw(gameTime);
        }
    }
}
